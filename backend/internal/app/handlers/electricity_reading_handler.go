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

func CreateElectricityReading(c *gin.Context) {
	var electricityReadingDTO dtos.CreateElectricityReadingDTO
	if err := c.ShouldBindJSON(&electricityReadingDTO); err != nil {
		utils.CreateGinResponse(c, "Invalid request body", http.StatusBadRequest, nil)
		return
	}

	userId, userEmail, _, userName := utils.ExtractDataFromContext(c)

	// Validate electricity meter
	if err := validateElectricityMeterOwnership(electricityReadingDTO.ElectricityMeterID, userId); err != nil {
		utils.CreateGinResponse(c, err.Error(), http.StatusBadRequest, nil)
		return
	}

	// Create new electricity reading
	electricityReading := models.ElectricityReading{
		ElectricityMeterID: electricityReadingDTO.ElectricityMeterID,
		LowerReading:       electricityReadingDTO.LowerReading,
		UpperReading:       electricityReadingDTO.UpperReading,
		ReadingDate:        utils.GetCurrentTimeFormatted(),
	}

	if err := repositories.NewElectricityReadingRepository().Create(&electricityReading); err != nil {
		utils.CreateGinResponse(c, "Failed to create electricity reading", http.StatusInternalServerError, nil)
		return
	}

	sendMail := c.DefaultQuery("sendMail", "true") == "true"

	if sendMail {
		if err := SendNotificationMail(userId, userEmail, userName, "New electricity reading has been added"); err != nil {
			utils.CreateGinResponse(c, "Failed to send notification mail", http.StatusInternalServerError, nil)
			return
		}
	}

	utils.CreateGinResponse(c, "Electricity reading added successfully", http.StatusCreated, electricityReading)
}

func GetAllElectricityReadingsByUserId(c *gin.Context) {
	userId, _, _, _ := utils.ExtractDataFromContext(c)

	electricityReadings, err := repositories.NewElectricityReadingRepository().GetByUserId(userId)

	if err != nil {
		utils.CreateGinResponse(c, "Failed to fetch electricity readings", http.StatusInternalServerError, nil)
		return
	}

	utils.CreateGinResponse(c, "Electricity readings fetched successfully", http.StatusOK, electricityReadings)
}

func validateElectricityMeterOwnership(electricityMeterId uint, userId uint) error {
	electricityMeters, err := repositories.NewElectricityMeterRepository().GetByUserId(userId)

	if err != nil {
		return fmt.Errorf("failed to validate electricity meter ownership")
	}

	for _, electricityMeter := range *electricityMeters {
		if electricityMeter.ID == electricityMeterId {
			return nil
		}
	}

	return fmt.Errorf("user does not own electricity meter")
}
