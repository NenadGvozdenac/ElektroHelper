package test_setup

import (
	"bytes"
	"elektrohelper/backend/config"
	"elektrohelper/backend/internal/app/handlers"
	"elektrohelper/backend/internal/app/utils"
	"encoding/json"
	"fmt"
	"net/http"
	"net/http/httptest"
	"os"
	"testing"

	"github.com/gin-gonic/gin"
	"github.com/stretchr/testify/assert"
	"gorm.io/driver/postgres"
	"gorm.io/gorm"
)

var router *gin.Engine

func setupTestDB() (*gorm.DB, func()) {
	host := os.Getenv("POSTGRES_HOST")
	port := os.Getenv("POSTGRES_PORT")
	user := os.Getenv("POSTGRES_USER")
	password := os.Getenv("POSTGRES_PASSWORD")
	dbname := os.Getenv("POSTGRES_DB")
	dsn := fmt.Sprintf("host=%s port=%s user=%s password=%s dbname=%s sslmode=disable", host, port, user, password, dbname)

	db, err := gorm.Open(postgres.Open(dsn), &gorm.Config{})
	if err != nil {
		panic("failed to connect to test database")
	}

	tx := db.Begin()

	return tx, func() {
		tx.Rollback()
	}
}

func mockUtils() {
	utils.GenerateToken = func(id uint, email, role, name string) (string, error) {
		return "mocked-token", nil
	}
	utils.GetCurrentTimeFormatted = func() string {
		return "2025-01-19 12:34:56"
	}
}

// Important: add routes to the router as you are testing them
func mockGinInstance() {
	if router == nil {
		router = gin.Default()
		gin.SetMode(gin.TestMode)

		// Register endpoints
		router.POST("/api/register", handlers.Register)
		router.POST("/api/login", handlers.Login)
		router.GET("/api/users", handlers.GetAllUsers)
		router.POST("/api/electricity_meters", handlers.CreateElectricityMeter)
		router.DELETE("/api/electricity_meters/:id", handlers.DeleteElectricityMeter)
		router.GET("/api/electricity_meters", handlers.GetAllElectricityMeters)
		router.GET("/api/electricity_meters/user", handlers.GetElectricityMetersByUserId)
	}
}

func AssertResponse(t *testing.T, w *httptest.ResponseRecorder, expectedCode int, expectedMessage string) {
	assert.Equal(t, expectedCode, w.Code)
	var response map[string]interface{}
	json.Unmarshal(w.Body.Bytes(), &response)
	assert.Equal(t, expectedMessage, response["message"])
}

func MakeRequest(method, url string, body []byte) *httptest.ResponseRecorder {
	req, _ := http.NewRequest(method, url, bytes.NewReader(body))
	req.Header.Set("Content-Type", "application/json")
	w := httptest.NewRecorder()
	router.ServeHTTP(w, req)

	return w
}

func MockGinContext(userId uint, email, role, name string) {
	utils.ExtractDataFromContext = func(c *gin.Context) (uint, string, string, string) {
		return userId, email, role, name
	}

	utils.ExtractUserEmailFromContext = func(c *gin.Context) string {
		return email
	}

	utils.ExtractUserRoleFromContext = func(c *gin.Context) string {
		return role
	}

	utils.ExtractUserNameFromContext = func(c *gin.Context) string {
		return name
	}

	utils.ExtractUserIdFromContext = func(c *gin.Context) uint {
		return userId
	}
}

// Important: call before every test so it setups the database
func PrepareTest() func() {
	mockUtils()
	db, cleanup := setupTestDB()

	config.DB = db
	mockGinInstance()

	return cleanup
}
