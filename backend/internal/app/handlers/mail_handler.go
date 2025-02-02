package handlers

import (
	"bytes"
	"elektrohelper/backend/config"
	"elektrohelper/backend/internal/app/utils"
	"elektrohelper/backend/internal/app/utils/logger"
	"elektrohelper/backend/internal/domain/models"
	"fmt"
	"os"
	"strconv"
	"sync"
	"text/template"

	"github.com/go-gomail/gomail"
)

var templateFolder = "static/"

// sendEmail sends an email concurrently.
func sendEmail(to string, subject string, templatePath string, data interface{}, wg *sync.WaitGroup, errChan chan<- error) {
	defer wg.Done()

	// Load the HTML template
	tmpl, err := template.ParseFiles(templatePath)
	if err != nil {
		logger.Error("Failed to parse template:", err.Error())
		errChan <- err
		return
	}

	// Populate the template with data
	var body bytes.Buffer
	if err := tmpl.Execute(&body, data); err != nil {
		logger.Error("Failed to execute template:", err.Error())
		errChan <- err
		return
	}

	mailHost := os.Getenv("MAIL_HOST")
	mailEmail := os.Getenv("MAIL_EMAIL")
	mailPort, err := strconv.Atoi(os.Getenv("MAIL_PORT"))

	if err != nil {
		logger.Error("Failed to convert MAIL_PORT to integer:", err.Error())
		errChan <- err
		return
	}

	// Email setup
	mailer := gomail.NewMessage()
	mailer.SetHeader("From", mailEmail)
	mailer.SetHeader("To", to)
	mailer.SetHeader("Subject", subject)
	mailer.SetBody("text/html", body.String())

	// SMTP configuration for MailHog
	dialer := gomail.NewDialer(mailHost, mailPort, "", "")

	// Send the email
	if err := dialer.DialAndSend(mailer); err != nil {
		logger.Error("Failed to send email:", err.Error())
		errChan <- err
		return
	}

	logger.Info("Email sent successfully to", to)
}

// sendRegistrationMail sends a registration email concurrently.
func sendRegistrationMail(user_id uint, to string, name string, wg *sync.WaitGroup, errChan chan<- error) {
	data := struct {
		Name string
	}{
		Name: name,
	}
	wg.Add(1)
	go sendEmail(to, "Welcome to ElektroHelper", templateFolder+"registration_mail.html", data, wg, errChan)

	// Add the notification to the database
	wg.Add(1)
	go addNotificationToDatabase(user_id, "Welcome to ElektroHelper!", "Registration", wg, errChan)
}

// sendNotificationMail sends a notification email concurrently.
func sendNotificationMail(user_id uint, to string, name string, message string, wg *sync.WaitGroup, errChan chan<- error) {
	data := struct {
		Name      string
		Message   string
		Timestamp string
	}{
		Name:      name,
		Message:   message,
		Timestamp: utils.GetCurrentTimeFormatted(),
	}
	wg.Add(1)
	go sendEmail(to, "Notification from ElektroHelper", templateFolder+"notification_mail.html", data, wg, errChan)

	// Add the notification to the database
	wg.Add(1)
	go addNotificationToDatabase(user_id, message, "Notification", wg, errChan)
}

func sendInformationMail(user_id uint, userEmail string, to string, firstName string, lastName string, lowerReading string, meterCode string, upperReading string, readingDate string, wg *sync.WaitGroup, errChan chan<- error) {
	data := struct {
		FirstName     string
		LastName      string
		UserEmail     string
		MeterCode     string
		LowerReading  string
		UpperReading  string
		DateOfReading string
	}{
		FirstName:     firstName,
		LastName:      lastName,
		UserEmail:     userEmail,
		MeterCode:     meterCode,
		LowerReading:  lowerReading,
		UpperReading:  upperReading,
		DateOfReading: readingDate,
	}

	wg.Add(1)
	go sendEmail(to, "Očitavanje brojila / Meter reading", templateFolder+"information_mail.html", data, wg, errChan)

	// Add the notification to the database
	wg.Add(1)
	go addNotificationToDatabase(user_id, "Meter reading information sent to "+to, "Information", wg, errChan)
}

// addNotificationToDatabase adds a notification to the database.
func addNotificationToDatabase(user_id uint, message string, subject string, wg *sync.WaitGroup, errChan chan<- error) {
	defer wg.Done()

	notification := models.Notification{
		UserID:           user_id,
		Subject:          subject,
		Message:          message,
		NotificationDate: utils.GetCurrentTimeFormatted(),
	}

	if err := config.DB.Create(&notification).Error; err != nil {
		errChan <- err
		return
	}
}

func SendRegistrationMail(user_id uint, to string, name string) error {
	var wg sync.WaitGroup
	errChan := make(chan error, 1) // Buffer size set to 1 for now; increase it if needed.

	// Call the goroutine to send the email
	sendRegistrationMail(user_id, to, name, &wg, errChan)

	// Wait for the email sending to complete
	wg.Wait()

	// Close the error channel after waiting for all goroutines
	close(errChan)

	// Process any errors from the channel
	for err := range errChan {
		if err != nil {
			return fmt.Errorf("failed to send registration email: %v", err)
		}
	}

	return nil
}

func SendNotificationMail(user_id uint, to string, name string, message string) error {
	var wg sync.WaitGroup
	errChan := make(chan error, 2) // Buffer size set to 1 for now; increase it if needed.

	// Call the goroutine to send the email
	sendNotificationMail(user_id, to, name, message, &wg, errChan)

	// Wait for the email sending to complete
	wg.Wait()

	// Close the error channel after waiting for all goroutines
	close(errChan)

	// Process any errors from the channel
	for err := range errChan {
		if err != nil {
			return fmt.Errorf("failed to send notification email: %v", err)
		}
	}

	return nil
}

func SendInformationMail(user_id uint, userEmail string, to string, firstName string, lastName string, meterCode string, lowerReading string, upperReading string, readingDate string) error {
	var wg sync.WaitGroup
	errChan := make(chan error, 1) // Buffer size set to 1 for now; increase it if needed.

	// Call the goroutine to send the email
	sendInformationMail(user_id, userEmail, to, firstName, lastName, lowerReading, meterCode, upperReading, readingDate, &wg, errChan)

	// Wait for the email sending to complete
	wg.Wait()

	// Close the error channel after waiting for all goroutines
	close(errChan)

	// Process any errors from the channel
	for err := range errChan {
		if err != nil {
			return fmt.Errorf("failed to send information email: %v", err)
		}
	}

	return nil
}
