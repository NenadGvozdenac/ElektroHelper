package handlers

import (
	"elektrohelper/backend/config"
	"elektrohelper/backend/models"
	"elektrohelper/backend/utils"
	"net/http"

	"github.com/gin-gonic/gin"
)

func GetAllUsers(c *gin.Context) {
	// Get all users from the database
	var users []models.User
	config.DB.Find(&users)

	response := utils.CreateResponse("Users retrieved successfully", http.StatusOK, users)

	// Return the users
	c.JSON(http.StatusOK, response)
}
