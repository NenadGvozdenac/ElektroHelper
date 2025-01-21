package handlers

import (
	"elektrohelper/backend/internal/app/dtos"
	"elektrohelper/backend/internal/app/repositories"
	"elektrohelper/backend/internal/app/utils"
	"elektrohelper/backend/internal/domain/models"
	"net/http"

	"github.com/gin-gonic/gin"
)

func CreateLocation(c *gin.Context) {
	var locationDTO dtos.CreateLocationDTO

	if err := c.ShouldBindJSON(&locationDTO); err != nil {
		utils.CreateGinResponse(c, "Invalid request body", http.StatusBadRequest, nil)
		return
	}

	userId, userEmail, _, userName := utils.ExtractDataFromContext(c)

	location := models.Location{
		Street:     locationDTO.Street,
		Number:     locationDTO.Number,
		City:       locationDTO.City,
		Country:    locationDTO.Country,
		PostalCode: locationDTO.PostalCode,
		UserID:     userId,
	}

	newLocationAddedMessage := "New location has been added: " + locationDTO.Street + " " + locationDTO.Number + ", " + locationDTO.City + ", " + locationDTO.Country + ", " + locationDTO.PostalCode

	if err := SendNotificationMail(userId, userEmail, userName, newLocationAddedMessage); err != nil {
		utils.CreateGinResponse(c, "Failed to send notification mail", http.StatusInternalServerError, nil)
		return
	}

	if err := repositories.NewLocationRepository().Create(&location); err != nil {
		utils.CreateGinResponse(c, "Failed to create location", http.StatusInternalServerError, nil)
		return
	}

	utils.CreateGinResponse(c, "Location created successfully", http.StatusCreated, location)
}

func GetAllLocationsByUser(c *gin.Context) {
	userId := utils.ExtractUserIdFromContext(c)

	locations, err := repositories.NewLocationRepository().GetByUserId(userId)

	if err != nil {
		utils.CreateGinResponse(c, "Failed to retrieve locations", http.StatusInternalServerError, nil)
		return
	}

	utils.CreateGinResponse(c, "Locations retrieved successfully", http.StatusOK, locations)
}
