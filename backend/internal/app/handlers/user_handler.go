package handlers

import (
	"elektrohelper/backend/internal/app/repositories"
	"elektrohelper/backend/internal/app/utils"
	"net/http"

	"github.com/gin-gonic/gin"
)

func GetAllUsers(c *gin.Context) {
	// Get all users from the database
	users, err := repositories.NewUserRepository().GetAll()

	if err != nil {
		response := utils.CreateResponse("Failed to retrieve users", http.StatusInternalServerError, nil)
		c.JSON(http.StatusInternalServerError, response)
		return
	}

	response := utils.CreateResponse("Users retrieved successfully", http.StatusOK, users)

	// Return the users
	c.JSON(http.StatusOK, response)
}
