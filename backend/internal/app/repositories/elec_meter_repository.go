package repositories

import (
	"elektrohelper/backend/config"
	"elektrohelper/backend/internal/domain/interfaces"
	"elektrohelper/backend/internal/domain/models"
)

type ElectricityMeterRepository struct{}

func NewElectricityMeterRepository() interfaces.ElectricityMeterRepositoryInterface {
	return &ElectricityMeterRepository{}
}

func (e *ElectricityMeterRepository) Create(electricityMeter *models.ElectricityMeter) error {
	if err := config.DB.Create(electricityMeter).Error; err != nil {
		return err
	}
	return nil
}

func (e *ElectricityMeterRepository) DeleteByID(id uint) error {
	if err := config.DB.Delete(&models.ElectricityMeter{}, id).Error; err != nil {
		return err
	}
	return nil
}

func (e *ElectricityMeterRepository) GetAll() (*[]models.ElectricityMeter, error) {
	var electricityMeters []models.ElectricityMeter
	if err := config.DB.Find(&electricityMeters).Error; err != nil {
		return nil, err
	}
	return &electricityMeters, nil
}

func (e *ElectricityMeterRepository) GetByID(id uint) (*models.ElectricityMeter, error) {
	var electricityMeter models.ElectricityMeter
	if err := config.DB.Where("id = ?", id).First(&electricityMeter).Error; err != nil {
		return nil, err
	}
	return &electricityMeter, nil
}

func (e *ElectricityMeterRepository) GetByLocationId(locationId uint) (*[]models.ElectricityMeter, error) {
	var electricityMeters []models.ElectricityMeter
	if err := config.DB.Where("location_id = ?", locationId).Find(&electricityMeters).Error; err != nil {
		return nil, err
	}
	return &electricityMeters, nil
}

func (e *ElectricityMeterRepository) GetByUserId(userId uint) (*[]models.ElectricityMeter, error) {
	var locationsOfUser []models.Location
	if err := config.DB.Where("user_id = ?", userId).Find(&locationsOfUser).Error; err != nil {
		return nil, err
	}

	var electricityMeters []models.ElectricityMeter
	for _, location := range locationsOfUser {
		meters, err := e.GetByLocationId(location.ID)
		if err != nil {
			return nil, err
		}
		electricityMeters = append(electricityMeters, *meters...)
	}

	return &electricityMeters, nil
}
