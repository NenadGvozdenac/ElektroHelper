package interfaces

import "elektrohelper/backend/internal/domain/models"

type UserRepositoryInterface interface {
	Create(user *models.User) error
	GetAll() (*[]models.User, error)
	GetByID(id uint) (*models.User, error)
	GetByEmail(email string) (*models.User, error)
	DeleteByID(id uint) error
	UpdateByID(id uint, user *models.User) error
}
