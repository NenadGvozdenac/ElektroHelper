package repositories

import (
	"elektrohelper/backend/config"
	"elektrohelper/backend/internal/domain/interfaces"
	"elektrohelper/backend/internal/domain/models"
)

type ElectricityReadingRepository struct{}

func NewElectricityReadingRepository() interfaces.ElectricityReadingRepositoryInterface {
	return &ElectricityReadingRepository{}
}

// Create implements interfaces.ElectricityReadingRepositoryInterface.
func (e *ElectricityReadingRepository) Create(electricityReading *models.ElectricityReading) error {
	if err := config.DB.Create(electricityReading).Error; err != nil {
		return err
	}
	return nil
}

// DeleteByID implements interfaces.ElectricityReadingRepositoryInterface.
func (e *ElectricityReadingRepository) DeleteByID(id uint) error {
	panic("unimplemented")
}

// GetAll implements interfaces.ElectricityReadingRepositoryInterface.
func (e *ElectricityReadingRepository) GetAll() (*[]models.ElectricityReading, error) {
	var electricityReadings []models.ElectricityReading
	if err := config.DB.Find(&electricityReadings).Error; err != nil {
		return nil, err
	}
	return &electricityReadings, nil
}

// GetByID implements interfaces.ElectricityReadingRepositoryInterface.
func (e *ElectricityReadingRepository) GetByID(id uint) (*models.ElectricityReading, error) {
	var electricityReading models.ElectricityReading
	if err := config.DB.Where("id = ?", id).First(&electricityReading).Error; err != nil {
		return nil, err
	}
	return &electricityReading, nil
}

// GetByMeterId implements interfaces.ElectricityReadingRepositoryInterface.
func (e *ElectricityReadingRepository) GetByMeterId(meterId uint) (*[]models.ElectricityReading, error) {
	var electricityReadings []models.ElectricityReading
	if err := config.DB.Where("electricity_meter_id = ?", meterId).Find(&electricityReadings).Error; err != nil {
		return nil, err
	}
	return &electricityReadings, nil
}

// GetByUserId implements interfaces.ElectricityReadingRepositoryInterface.
func (e *ElectricityReadingRepository) GetByUserId(userId uint) (*[]models.ElectricityReading, error) {
	var locationsOfUser []models.Location
	if err := config.DB.Where("user_id = ?", userId).Find(&locationsOfUser).Error; err != nil {
		return nil, err
	}

	var electricityMeters []models.ElectricityMeter
	for _, location := range locationsOfUser {
		meters, err := NewElectricityMeterRepository().GetByLocationId(location.ID)
		if err != nil {
			return nil, err
		}
		electricityMeters = append(electricityMeters, *meters...)
	}

	var electricityReadings []models.ElectricityReading
	for _, meter := range electricityMeters {
		readings, err := e.GetByMeterId(meter.ID)
		if err != nil {
			return nil, err
		}
		electricityReadings = append(electricityReadings, *readings...)
	}

	return &electricityReadings, nil
}
