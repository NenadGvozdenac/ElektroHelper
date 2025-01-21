package test

import (
	"elektrohelper/backend/config"
	"elektrohelper/backend/internal/app/dtos"
	"elektrohelper/backend/internal/app/utils"
	"elektrohelper/backend/internal/domain/models"
	test_setup "elektrohelper/backend/tests"
	"encoding/json"
	"net/http"
	"testing"
)

func TestCreateElectricityReading_Success(t *testing.T) {
	cleanup := test_setup.PrepareTest()
	defer cleanup()

	// Arrange
	user := models.User{
		Name:         "John",
		Surname:      "Doe",
		Email:        "email@gmail.com",
		Phone:        "123456789",
		Password:     "hashedpassword",
		CreationDate: "2025-01-19 12:34:56",
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
	}

	// Create electricity meter
	config.DB.Create(&electricityMeter)

	input := dtos.CreateElectricityReadingDTO{
		ElectricityMeterID: electricityMeter.ID,
		LowerReading:       "100",
		UpperReading:       "200",
	}
	body, _ := json.Marshal(input)

	// Act
	w := test_setup.MakeRequest(http.MethodPost, "/api/electricity_readings?sendMail=false", body)

	// Assert
	test_setup.AssertResponse(t, w, http.StatusCreated, "Electricity reading added successfully")
}

func TestCreateElectricityReading_InvalidMeterOwnership(t *testing.T) {
	cleanup := test_setup.PrepareTest()
	defer cleanup()

	// Arrange
	user := models.User{
		Name:         "John",
		Surname:      "Doe",
		Email:        "email@gmail.com",
		Phone:        "123456789",
		Password:     "hashedpassword",
		CreationDate: "2025-01-19 12:34:56",
		Role:         "user",
	}

	// Create user
	config.DB.Create(&user)

	// Mock gin context
	test_setup.MockGinContext(user.ID, user.Email, user.Role, user.Name)

	// Invalid electricity meter ID (non-existing for the user)
	input := dtos.CreateElectricityReadingDTO{
		ElectricityMeterID: 999,
		LowerReading:       "100",
		UpperReading:       "200",
	}
	body, _ := json.Marshal(input)

	// Act
	w := test_setup.MakeRequest(http.MethodPost, "/api/electricity_readings?sendMail=false", body)

	// Assert
	test_setup.AssertResponse(t, w, http.StatusBadRequest, "user does not own electricity meter")
}

func TestCreateElectricityReading_InvalidRequestBody(t *testing.T) {
	cleanup := test_setup.PrepareTest()
	defer cleanup()

	// Arrange
	user := models.User{
		Name:         "John",
		Surname:      "Doe",
		Email:        "email@gmail.com",
		Phone:        "123456789",
		Password:     "hashedpassword",
		CreationDate: "2025-01-19 12:34:56",
		Role:         "user",
	}

	// Create user
	config.DB.Create(&user)

	// Mock gin context
	test_setup.MockGinContext(user.ID, user.Email, user.Role, user.Name)

	// Invalid JSON format (missing required fields)
	body := []byte(`{"electricity_meter_id": 1}`)

	// Act
	w := test_setup.MakeRequest(http.MethodPost, "/api/electricity_readings", body)

	// Assert
	test_setup.AssertResponse(t, w, http.StatusBadRequest, "Invalid request body")
}

func TestGetAllElectricityReadingsByUserId_Success(t *testing.T) {
	cleanup := test_setup.PrepareTest()
	defer cleanup()

	// Arrange
	user := models.User{
		Name:         "John",
		Surname:      "Doe",
		Email:        "email@gmail.com",
		Phone:        "123456789",
		Password:     "hashedpassword",
		CreationDate: "2025-01-19 12:34:56",
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
	}

	// Create electricity meter
	config.DB.Create(&electricityMeter)

	reading := models.ElectricityReading{
		ElectricityMeterID: electricityMeter.ID,
		LowerReading:       "100",
		UpperReading:       "200",
		ReadingDate:        utils.GetCurrentTimeFormatted(),
	}

	// Create electricity reading
	config.DB.Create(&reading)

	// Act
	w := test_setup.MakeRequest(http.MethodGet, "/api/electricity_readings", nil)

	// Assert
	test_setup.AssertResponse(t, w, http.StatusOK, "Electricity readings fetched successfully")
}
