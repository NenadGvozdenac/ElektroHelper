package interfaces

import "elektrohelper/backend/internal/domain/models"

type ElectricityReadingRepositoryInterface interface {
	Create(electricityReading *models.ElectricityReading) error
	GetAll() (*[]models.ElectricityReading, error)
	GetByID(id uint) (*models.ElectricityReading, error)
	GetByUserId(userId uint) (*[]models.ElectricityReading, error)
	GetByMeterId(meterId uint) (*[]models.ElectricityReading, error)
	DeleteByID(id uint) error
}
