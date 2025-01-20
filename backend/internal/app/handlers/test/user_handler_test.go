package test

import (
	"elektrohelper/backend/config"
	"elektrohelper/backend/internal/domain/models"
	"encoding/json"
	"net/http"
	"testing"

	"github.com/stretchr/testify/assert"
)

func TestGetAllUsers_Success(t *testing.T) {
	cleanup := prepareTest()
	defer cleanup()

	// Arrange
	users := []models.User{
		{
			Name:         "Alice",
			Surname:      "Smith",
			Email:        "alice.smith@example.com",
			Phone:        "111111111",
			Password:     "hashedpassword1",
			Role:         "user",
			CreationDate: "2025-01-19 10:00:00",
		},
		{
			Name:         "Bob",
			Surname:      "Jones",
			Email:        "bob.jones@example.com",
			Phone:        "222222222",
			Password:     "hashedpassword2",
			Role:         "admin",
			CreationDate: "2025-01-20 12:00:00",
		},
	}

	for _, user := range users {
		config.DB.Create(&user)
	}

	// Act
	w := makeRequest(router, http.MethodGet, "/api/users", nil)

	// Assert
	var response map[string]interface{}
	json.Unmarshal(w.Body.Bytes(), &response)

	assert.Equal(t, http.StatusOK, w.Code)
	assert.Equal(t, "Users retrieved successfully", response["message"])
	assert.NotNil(t, response["data"])
	assert.Equal(t, 2, len(response["data"].([]interface{})))
}
