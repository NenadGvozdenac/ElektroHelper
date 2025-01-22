package repositories

import (
	"elektrohelper/backend/config"
	"elektrohelper/backend/internal/domain/interfaces"
	"elektrohelper/backend/internal/domain/models"
)

type TokenRepository struct{}

func NewTokenRepository() interfaces.RefreshTokensRepositoryInterface {
	return &TokenRepository{}
}

// Create implements interfaces.RefreshTokensRepositoryInterface.
func (t *TokenRepository) Create(refreshToken *models.Token) error {
	if err := config.DB.Create(refreshToken).Error; err != nil {
		return err
	}
	return nil
}

// GetByToken implements interfaces.RefreshTokensRepositoryInterface.
func (t *TokenRepository) GetByToken(token string) (*models.Token, error) {
	var tokenModel models.Token
	if err := config.DB.Where("refresh_token = ? AND revoked = ?", token, false).First(&tokenModel).Error; err != nil {
		return nil, err
	}
	return &tokenModel, nil
}

// RevokeToken implements interfaces.RefreshTokensRepositoryInterface.
func (t *TokenRepository) RevokeToken(token string) error {
	if err := config.DB.Model(&models.Token{}).Where("refresh_token = ?", token).Update("revoked", true).Error; err != nil {
		return err
	}
	return nil
}
