package interfaces

import "elektrohelper/backend/internal/domain/models"

type LocationRepositoryInterface interface {
	Create(location *models.Location) error
	GetAll() (*[]models.Location, error)
	GetByID(id uint) (*models.Location, error)
	GetByUserId(userId uint) (*[]models.Location, error)
	DeleteByID(id uint) error
	UpdateByID(id uint, location *models.Location) error
}
