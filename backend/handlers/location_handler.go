package handlers

import (
	"elektrohelper/backend/config"
	"elektrohelper/backend/dtos"
	"elektrohelper/backend/models"
	"elektrohelper/backend/utils"
	"net/http"

	"github.com/gin-gonic/gin"
)

func CreateLocation(c *gin.Context) {
	var locationDTO dtos.LocationDTO

	if err := c.ShouldBindJSON(&locationDTO); err != nil {
		utils.CreateGinResponse(c, "Invalid request body", http.StatusBadRequest, nil)
		return
	}

	// Extract userId from the context
	userId := utils.ExtractUserIdFromContext(c)

	location := models.Location{
		Street:     locationDTO.Street,
		Number:     locationDTO.Number,
		City:       locationDTO.City,
		Country:    locationDTO.Country,
		PostalCode: locationDTO.PostalCode,
		UserID:     userId,
	}

	if err := config.DB.Create(&location).Error; err != nil {
		utils.CreateGinResponse(c, "Failed to create location", http.StatusInternalServerError, nil)
		return
	}

	utils.CreateGinResponse(c, "Location created successfully", http.StatusCreated, location)
}

func GetAllLocationsByUser(c *gin.Context) {
	var locations []dtos.LocationResponseDTO

	// Extract userId from the context
	userId := utils.ExtractUserIdFromContext(c)

	// Get all locations from the database
	config.DB.Table("locations").Select("locations.id, locations.street, locations.number, locations.city, locations.country, locations.postal_code").Where("locations.user_id = ?", userId).Scan(&locations)

	utils.CreateGinResponse(c, "Locations retrieved successfully", http.StatusOK, locations)
}
