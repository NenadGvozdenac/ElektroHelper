package interfaces

import "elektrohelper/backend/internal/domain/models"

type RefreshTokensRepositoryInterface interface {
	Create(refreshToken *models.Token) error
	GetByToken(token string) (*models.Token, error)
	RevokeToken(token string) error
}
