package handlers

import (
	"elektrohelper/backend/internal/app/repositories"
	"elektrohelper/backend/internal/app/utils"
	"net/http"
	"strconv"

	"github.com/gin-gonic/gin"
)

func GetAllUsers(c *gin.Context) {
	// Get all users from the database
	users, err := repositories.NewUserRepository().GetAll()

	if err != nil {
		utils.CreateGinResponse(c, "Failed to retrieve users", http.StatusInternalServerError, nil)
		return
	}

	utils.CreateGinResponse(c, "Users retrieved successfully", http.StatusOK, users)
}

func GetUserById(c *gin.Context) {
	idStr := c.Param("id")

	idParsed, err := strconv.ParseUint(idStr, 10, 0)
	if err != nil {
		// handle the error (e.g., return 400 Bad Request)
		c.JSON(http.StatusBadRequest, gin.H{"error": "Invalid ID"})
		return
	}

	userId := uint(idParsed)

	// Get user by ID from the database
	user, err := repositories.NewUserRepository().GetByID(userId)

	if err != nil {
		utils.CreateGinResponse(c, "Failed to retrieve user", http.StatusInternalServerError, nil)
		return
	}

	if user == nil {
		utils.CreateGinResponse(c, "User not found", http.StatusNotFound, nil)
		return
	}

	utils.CreateGinResponse(c, "User retrieved successfully", http.StatusOK, user)
}

func GetActiveUser(c *gin.Context) {
	userId := utils.ExtractUserIdFromContext(c)

	user, err := repositories.NewUserRepository().GetByID(userId)

	if err != nil {
		utils.CreateGinResponse(c, "Failed to retrieve user", http.StatusInternalServerError, nil)
		return
	}

	if user == nil {
		utils.CreateGinResponse(c, "User not found", http.StatusNotFound, nil)
		return
	}

	utils.CreateGinResponse(c, "User retrieved successfully", http.StatusOK, user)
}
