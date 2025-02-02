package test

import (
	"elektrohelper/backend/config"
	"elektrohelper/backend/internal/app/dtos"
	"elektrohelper/backend/internal/app/utils"
	"elektrohelper/backend/internal/domain/models"
	test_setup "elektrohelper/backend/tests"
	"encoding/json"
	"fmt"
	"net/http"
	"testing"
)

func TestCreateElectricityMeter_Success(t *testing.T) {
	cleanup := test_setup.PrepareTest()
	defer cleanup()

	// Arrange
	user := models.User{
		Name:         "John",
		Surname:      "Doe",
		Email:        "email@gmail.com",
		Phone:        "123456789",
		Password:     "hashedpassword",
		CreationDate: "2006-01-02T15:04:05",
		Role:         "user",
	}

	// Create user
	config.DB.Create(&user)

	// Mock gin context
	test_setup.MockGinContext(user.ID, user.Email, user.Role, user.Name)

	location := models.Location{
		ID:         1,
		Street:     "Test street",
		Number:     "1",
		City:       "Test city",
		Country:    "Test country",
		PostalCode: "12345",
		UserID:     user.ID,
	}

	// Create location
	config.DB.Create(&location)

	input := dtos.CreateElectricityMeterDTO{
		LocationID: location.ID,
		MeterCode:  "123456789",
	}
	body, _ := json.Marshal(input)

	// Act
	w := test_setup.MakeRequest(http.MethodPost, "/api/electricity_meters?sendMail=false&sendMailToEPS=false", body)

	// Assert
	test_setup.AssertResponse(t, w, http.StatusCreated, "Electricity meter created successfully")
}

func TestCreateElectricityMeter_InvalidLocation(t *testing.T) {
	cleanup := test_setup.PrepareTest()
	defer cleanup()

	// Arrange
	user := models.User{
		Name:         "John",
		Surname:      "Doe",
		Email:        "email@gmail.com",
		Phone:        "123456789",
		Password:     "hashedpassword",
		CreationDate: "2006-01-02T15:04:05",
		Role:         "user",
	}

	// Create user
	config.DB.Create(&user)

	// Mock gin context
	test_setup.MockGinContext(user.ID, user.Email, user.Role, user.Name)

	// Invalid location ID (non-existing)
	input := dtos.CreateElectricityMeterDTO{
		LocationID: 999,
		MeterCode:  "123456789",
	}
	body, _ := json.Marshal(input)

	// Act
	w := test_setup.MakeRequest(http.MethodPost, "/api/electricity_meters?sendMail=false&sendMailToEPS=false", body)

	// Assert
	test_setup.AssertResponse(t, w, http.StatusBadRequest, "location does not exist")
}

func TestCreateElectricityMeter_NoMeterCode(t *testing.T) {
	cleanup := test_setup.PrepareTest()
	defer cleanup()

	// Arrange
	user := models.User{
		Name:         "John",
		Surname:      "Doe",
		Email:        "email@gmail.com",
		Phone:        "123456789",
		Password:     "hashedpassword",
		CreationDate: "2006-01-02T15:04:05",
		Role:         "user",
	}

	// Create user
	config.DB.Create(&user)

	// Mock gin context
	test_setup.MockGinContext(user.ID, user.Email, user.Role, user.Name)

	location := models.Location{
		ID:         1,
		Street:     "Test street",
		Number:     "1",
		City:       "Test city",
		Country:    "Test country",
		PostalCode: "12345",
		UserID:     user.ID,
	}

	// Create location
	config.DB.Create(&location)

	// Missing meter code
	input := dtos.CreateElectricityMeterDTO{
		LocationID: location.ID,
	}

	body, _ := json.Marshal(input)

	// Act
	w := test_setup.MakeRequest(http.MethodPost, "/api/electricity_meters?sendMail=false&sendMailToEPS=false", body)

	// Assert
	test_setup.AssertResponse(t, w, http.StatusBadRequest, "Invalid request body")
}

func TestDeleteElectricityMeter_Success(t *testing.T) {
	cleanup := test_setup.PrepareTest()
	defer cleanup()

	// Arrange
	user := models.User{
		Name:         "John",
		Surname:      "Doe",
		Email:        "email@gmail.com",
		Phone:        "123456789",
		Password:     "hashedpassword",
		CreationDate: "2006-01-02T15:04:05",
		Role:         "user",
	}

	// Create user
	config.DB.Create(&user)

	// Mock gin context
	test_setup.MockGinContext(user.ID, user.Email, user.Role, user.Name)

	location := models.Location{
		ID:         1,
		Street:     "Test street",
		Number:     "1",
		City:       "Test city",
		Country:    "Test country",
		PostalCode: "12345",
		UserID:     user.ID,
	}

	// Create location
	config.DB.Create(&location)

	electricityMeter := models.ElectricityMeter{
		LocationID:         location.ID,
		DateOfRegistration: utils.GetCurrentTimeFormatted(),
		MeterCode:          "123456789",
	}

	// Create electricity meter
	config.DB.Create(&electricityMeter)

	// Act
	w := test_setup.MakeRequest(http.MethodDelete, fmt.Sprintf("/api/electricity_meters/%d", electricityMeter.ID), nil)

	// Assert
	test_setup.AssertResponse(t, w, http.StatusOK, "Electricity meter deleted successfully")
}

func TestDeleteElectricityMeter_FailedToRetrieve(t *testing.T) {
	cleanup := test_setup.PrepareTest()
	defer cleanup()

	// Arrange
	user := models.User{
		Name:         "John",
		Surname:      "Doe",
		Email:        "email@gmail.com",
		Phone:        "123456789",
		Password:     "hashedpassword",
		CreationDate: "2006-01-02T15:04:05",
		Role:         "user",
	}

	// Create user
	config.DB.Create(&user)

	// Mock gin context
	test_setup.MockGinContext(user.ID, user.Email, user.Role, user.Name)

	// Invalid electricity meter ID (non-existing or wrong location)
	// Act
	w := test_setup.MakeRequest(http.MethodDelete, "/api/electricity_meters/999", nil)

	// Assert
	test_setup.AssertResponse(t, w, http.StatusNotFound, "Failed to retrieve electricity meter")
}

func TestGetAllElectricityMeters_Success(t *testing.T) {
	cleanup := test_setup.PrepareTest()
	defer cleanup()

	// Arrange
	user := models.User{
		Name:         "John",
		Surname:      "Doe",
		Email:        "email@gmail.com",
		Phone:        "123456789",
		Password:     "hashedpassword",
		CreationDate: "2006-01-02T15:04:05",
		Role:         "user",
	}

	// Create user
	config.DB.Create(&user)

	// Mock gin context
	test_setup.MockGinContext(user.ID, user.Email, user.Role, user.Name)

	location := models.Location{
		ID:         1,
		Street:     "Test street",
		Number:     "1",
		City:       "Test city",
		Country:    "Test country",
		PostalCode: "12345",
		UserID:     user.ID,
	}

	// Create location
	config.DB.Create(&location)

	electricityMeter := models.ElectricityMeter{
		LocationID:         location.ID,
		DateOfRegistration: utils.GetCurrentTimeFormatted(),
		MeterCode:          "123456789",
	}

	// Create electricity meter
	config.DB.Create(&electricityMeter)

	// Get all electricity meters
	// Act
	w := test_setup.MakeRequest(http.MethodGet, "/api/electricity_meters", nil)

	// Assert
	test_setup.AssertResponse(t, w, http.StatusOK, "Electricity meters retrieved successfully")
}

func TestGetElectricityMetersByUserId_Success(t *testing.T) {
	cleanup := test_setup.PrepareTest()
	defer cleanup()

	// Arrange
	user := models.User{
		Name:         "John",
		Surname:      "Doe",
		Email:        "email@gmail.com",
		Phone:        "123456789",
		Password:     "hashedpassword",
		CreationDate: "2006-01-02T15:04:05",
		Role:         "user",
	}

	// Create user
	config.DB.Create(&user)

	// Mock gin context
	test_setup.MockGinContext(user.ID, user.Email, user.Role, user.Name)

	location := models.Location{
		ID:         1,
		Street:     "Test street",
		Number:     "1",
		City:       "Test city",
		Country:    "Test country",
		PostalCode: "12345",
		UserID:     user.ID,
	}

	// Create location
	config.DB.Create(&location)

	electricityMeter := models.ElectricityMeter{
		LocationID:         location.ID,
		DateOfRegistration: utils.GetCurrentTimeFormatted(),
		MeterCode:          "123456789",
	}

	// Create electricity meter
	config.DB.Create(&electricityMeter)

	// Get electricity meters by user ID
	// Act
	w := test_setup.MakeRequest(http.MethodGet, "/api/electricity_meters/user", nil)

	// Assert
	test_setup.AssertResponse(t, w, http.StatusOK, "Electricity meters retrieved successfully")
}
