package handlers

import (
	"elektrohelper/backend/internal/app/dtos"
	"elektrohelper/backend/internal/app/repositories"
	"elektrohelper/backend/internal/app/utils"
	"elektrohelper/backend/internal/domain/models"
	"fmt"
	"net/http"

	"github.com/gin-gonic/gin"
)

func CreateElectricityMeter(c *gin.Context) {
	var electricityMeterDTO dtos.CreateElectricityMeterDTO
	if err := c.ShouldBindJSON(&electricityMeterDTO); err != nil {
		utils.CreateGinResponse(c, "Invalid request body", http.StatusBadRequest, nil)
		return
	}

	userId, userEmail, _, userName := utils.ExtractDataFromContext(c)

	// Validate location
	if err := validateLocationOwnership(electricityMeterDTO.LocationID, userId); err != nil {
		utils.CreateGinResponse(c, err.Error(), http.StatusBadRequest, nil)
		return
	}

	// Ensure no electricity meter exists for this location
	if err := validateUniqueElectricityMeter(electricityMeterDTO.LocationID, userId); err != nil {
		utils.CreateGinResponse(c, err.Error(), http.StatusBadRequest, nil)
		return
	}

	// Create new electricity meter
	electricityMeter := models.ElectricityMeter{
		LocationID:         electricityMeterDTO.LocationID,
		DateOfRegistration: utils.GetCurrentTimeFormatted(),
	}

	if err := repositories.NewElectricityMeterRepository().Create(&electricityMeter); err != nil {
		utils.CreateGinResponse(c, "Failed to create electricity meter", http.StatusInternalServerError, nil)
		return
	}

	// Send notification email
	if err := SendNotificationMail(userId, userEmail, userName, "New electricity meter has been added"); err != nil {
		utils.CreateGinResponse(c, "Failed to send notification mail", http.StatusInternalServerError, nil)
		return
	}

	utils.CreateGinResponse(c, "Electricity meter created successfully", http.StatusCreated, electricityMeter)
}

func DeleteElectricityMeter(c *gin.Context) {
	electricityMeterId := utils.ConvertParamToUint(c, "id")

	electricityMeter, err := repositories.NewElectricityMeterRepository().GetByID(electricityMeterId)
	if err != nil {
		utils.CreateGinResponse(c, "Failed to retrieve electricity meter", http.StatusInternalServerError, nil)
		return
	}

	userId := utils.ExtractUserIdFromContext(c)

	if err := validateLocationOwnership(electricityMeter.LocationID, userId); err != nil {
		utils.CreateGinResponse(c, "You are not authorized to delete this electricity meter", http.StatusBadRequest, nil)
		return
	}

	if err := repositories.NewElectricityMeterRepository().DeleteByID(electricityMeterId); err != nil {
		utils.CreateGinResponse(c, "Failed to delete electricity meter", http.StatusInternalServerError, nil)
		return
	}

	utils.CreateGinResponse(c, "Electricity meter deleted successfully", http.StatusOK, nil)
}

func GetAllElectricityMeters(c *gin.Context) {
	electricityMeters, err := repositories.NewElectricityMeterRepository().GetAll()
	if err != nil {
		utils.CreateGinResponse(c, "Failed to retrieve electricity meters", http.StatusInternalServerError, nil)
		return
	}
	utils.CreateGinResponse(c, "Electricity meters retrieved successfully", http.StatusOK, electricityMeters)
}

func GetElectricityMetersByUserId(c *gin.Context) {
	userId := utils.ExtractUserIdFromContext(c)

	electricityMeters, err := repositories.NewElectricityMeterRepository().GetByUserId(userId)
	if err != nil {
		utils.CreateGinResponse(c, "Failed to retrieve electricity meters", http.StatusInternalServerError, nil)
		return
	}
	utils.CreateGinResponse(c, "Electricity meters retrieved successfully", http.StatusOK, electricityMeters)
}

// validateLocationOwnership checks if the user owns the given location.
func validateLocationOwnership(locationID uint, userID uint) error {
	allLocations, err := repositories.NewLocationRepository().GetAll()
	if err != nil {
		return fmt.Errorf("failed to retrieve locations")
	}

	for _, location := range *allLocations {
		if location.ID == locationID {
			if location.UserID != userID {
				return fmt.Errorf("you are not authorized to add an electricity meter for this location")
			}
			return nil
		}
	}
	return fmt.Errorf("location does not exist")
}

// validateUniqueElectricityMeter ensures that no electricity meter exists for the given location and user.
func validateUniqueElectricityMeter(locationID uint, userID uint) error {
	electricityMeters, err := repositories.NewElectricityMeterRepository().GetByUserId(userID)
	if err != nil {
		return fmt.Errorf("failed to retrieve electricity meters")
	}

	for _, meter := range *electricityMeters {
		if meter.LocationID == locationID {
			return fmt.Errorf("electricity meter for this location already exists")
		}
	}
	return nil
}
