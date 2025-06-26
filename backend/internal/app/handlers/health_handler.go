// Health endpoint for the main Go API
package handlers

import (
	"elektrohelper/backend/internal/app/utils"
	"net/http"

	"github.com/gin-gonic/gin"
)

// HealthCheck returns the health status of the application
func HealthCheck(c *gin.Context) {
	utils.CreateGinResponse(c, "API is healthy", http.StatusOK, map[string]string{
		"status":    "healthy",
		"service":   "elektrohelper-main-api",
		"timestamp": utils.GetCurrentTimeFormatted(),
	})
}
