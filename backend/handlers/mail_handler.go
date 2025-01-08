package handlers

import (
	"bytes"
	"elektrohelper/backend/config"
	"elektrohelper/backend/models"
	"elektrohelper/backend/utils"
	"fmt"
	"sync"
	"text/template"

	"github.com/go-gomail/gomail"
)

var templateFolder = "static/"

// sendEmail sends an email with the given template, subject, and recipient details.
func sendEmail(to string, subject string, templatePath string, data interface{}, wg *sync.WaitGroup, errChan chan<- error) {
	defer wg.Done()

	// Load the HTML template
	tmpl, err := template.ParseFiles(templatePath)
	if err != nil {
		fmt.Println("Failed to load template:", err)
		errChan <- err
		return
	}

	// Populate the template with data
	var body bytes.Buffer
	if err := tmpl.Execute(&body, data); err != nil {
		fmt.Println("Failed to populate template:", err)
		errChan <- err
		return
	}

	// Email setup
	mailer := gomail.NewMessage()
	mailer.SetHeader("From", "elektrohelper.no-reply@gmail.com")
	mailer.SetHeader("To", to)
	mailer.SetHeader("Subject", subject)
	mailer.SetBody("text/html", body.String())

	// SMTP configuration
	dialer := gomail.NewDialer("smtp.gmail.com", 587, "noreply.onlybuns@gmail.com", "honv bmtg xumo kbxj")

	// Send the email
	if err := dialer.DialAndSend(mailer); err != nil {
		errChan <- err
		return
	}
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
