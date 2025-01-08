package interfaces

import "elektrohelper/backend/internal/domain/models"

type ElectricityMeterRepositoryInterface interface {
	Create(electricityMeter *models.ElectricityMeter) error
	GetAll() (*[]models.ElectricityMeter, error)
	GetByID(id uint) (*models.ElectricityMeter, error)
	GetByLocationId(locationId uint) (*[]models.ElectricityMeter, error)
	GetByUserId(userId uint) (*[]models.ElectricityMeter, error)
	DeleteByID(id uint) error
}
