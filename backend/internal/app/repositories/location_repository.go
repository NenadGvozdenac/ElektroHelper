package repositories

import (
	"elektrohelper/backend/config"
	"elektrohelper/backend/internal/domain/interfaces"
	"elektrohelper/backend/internal/domain/models"
)

type LocationRepository struct{}

func NewLocationRepository() interfaces.LocationRepositoryInterface {
	return &LocationRepository{}
}

func (repo *LocationRepository) Create(location *models.Location) error {
	if err := config.DB.Create(location).Error; err != nil {
		return err
	}
	return nil
}

func (repo *LocationRepository) GetByID(id uint) (*models.Location, error) {
	var location models.Location
	if err := config.DB.Where("id = ?", id).First(&location).Error; err != nil {
		return nil, err
	}
	return &location, nil
}

func (repo *LocationRepository) GetByUserId(userId uint) (*[]models.Location, error) {
	var locations []models.Location
	if err := config.DB.Where("user_id = ?", userId).Find(&locations).Error; err != nil {
		return nil, err
	}
	return &locations, nil
}

func (repo *LocationRepository) GetAll() (*[]models.Location, error) {
	var locations []models.Location
	if err := config.DB.Find(&locations).Error; err != nil {
		return nil, err
	}
	return &locations, nil
}

func (repo *LocationRepository) DeleteByID(id uint) error {
	if err := config.DB.Delete(&models.Location{}, id).Error; err != nil {
		return err
	}
	return nil
}

func (repo *LocationRepository) UpdateByID(id uint, location *models.Location) error {
	if err := config.DB.Model(&models.Location{}).Where("id = ?", id).Updates(location).Error; err != nil {
		return err
	}
	return nil
}
