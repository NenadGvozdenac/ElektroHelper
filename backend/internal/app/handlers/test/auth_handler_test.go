package test

import (
	"elektrohelper/backend/config"
	"elektrohelper/backend/internal/app/dtos"
	"elektrohelper/backend/internal/app/handlers"
	"elektrohelper/backend/internal/domain/models"
	"encoding/json"
	"net/http"
	"testing"

	"github.com/gin-gonic/gin"
	"github.com/stretchr/testify/assert"
	"golang.org/x/crypto/bcrypt"
)

func TestRegister_Success(t *testing.T) {
	cleanup := prepareTest()
	defer cleanup()

	// Arrange
	input := dtos.RegisterDTO{
		Name:            "John",
		Surname:         "Doe",
		Email:           "john.doe@example.com",
		Phone:           "123456789",
		Password:        "securepassword",
		ConfirmPassword: "securepassword",
	}
	body, _ := json.Marshal(input)

	// Act
	w := makeRequest(router, http.MethodPost, "/api/register?sendMail=false", body)

	// Assert
	assertResponse(t, w, http.StatusCreated, "User registered successfully")
	assert.NotNil(t, w.Body.String())
}

func TestRegister_EmailAlreadyInUse(t *testing.T) {
	cleanup := prepareTest()
	defer cleanup()

	// Arrange
	existingUser := models.User{
		Name:         "Existing",
		Surname:      "User",
		Email:        "existing.user@example.com",
		Phone:        "987654321",
		Password:     "hashedpassword",
		Role:         "user",
		CreationDate: "2025-01-19 12:34:56",
	}
	config.DB.Create(&existingUser)

	input := dtos.RegisterDTO{
		Name:            "John",
		Surname:         "Doe",
		Email:           "existing.user@example.com",
		Phone:           "123456789",
		Password:        "securepassword",
		ConfirmPassword: "securepassword",
	}
	body, _ := json.Marshal(input)

	// Act
	w := makeRequest(router, http.MethodPost, "/api/register?sendMail=false", body)

	// Assert
	assertResponse(t, w, http.StatusBadRequest, "Email already in use")
}

func TestLogin_InvalidCredentials(t *testing.T) {
	cleanup := prepareTest()
	defer cleanup()

	// Arrange
	user := models.User{
		Name:         "John",
		Surname:      "Doe",
		Email:        "john.doe@example.com",
		Phone:        "123456789",
		Password:     "$2a$10$yH9ATy8iAWtQgHFu5kTjGeCEmiv1Yy.Pti.s4.lG55mhprfwz5U6y", // "securepassword"
		Role:         "user",
		CreationDate: "2025-01-19 12:34:56",
	}
	config.DB.Create(&user)

	input := dtos.LoginDTO{
		Email:    "john.doe@example.com",
		Password: "wrongpassword",
	}
	body, _ := json.Marshal(input)

	// Act
	w := makeRequest(router, http.MethodPost, "/api/login", body)

	// Assert
	assertResponse(t, w, http.StatusUnauthorized, "Invalid email or password")
}

func TestLogin_NonExistentEmail(t *testing.T) {
	cleanup := prepareTest()
	defer cleanup()

	// Arrange
	input := dtos.LoginDTO{
		Email:    "nonexistent@example.com",
		Password: "securepassword",
	}
	body, _ := json.Marshal(input)

	// Act
	router := gin.Default()
	router.POST("/api/login", handlers.Login)
	w := makeRequest(router, http.MethodPost, "/api/login", body)

	// Assert
	assertResponse(t, w, http.StatusUnauthorized, "Invalid email or password")
}

func TestLogin_Success(t *testing.T) {
	cleanup := prepareTest()
	defer cleanup()

	// Arrange
	hashedPassword, _ := bcrypt.GenerateFromPassword([]byte("securepassword"), bcrypt.DefaultCost)

	user := models.User{
		Name:         "John",
		Surname:      "Doe",
		Email:        "john.doe@example.com",
		Phone:        "123456789",
		Password:     string(hashedPassword),
		Role:         "user",
		CreationDate: "2025-01-19 12:34:56",
	}
	config.DB.Create(&user)

	input := dtos.LoginDTO{
		Email:    "john.doe@example.com",
		Password: "securepassword",
	}
	body, _ := json.Marshal(input)

	w := makeRequest(router, http.MethodPost, "/api/login", body)

	// Assert
	assertResponse(t, w, http.StatusOK, "User logged in successfully")
	assert.NotNil(t, w.Body.String())
}
