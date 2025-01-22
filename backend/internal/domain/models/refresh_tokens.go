package models

import "time"

type Token struct {
	ID           uint      `gorm:"primaryKey"`      // Unique identifier for each token
	UserID       uint      `gorm:"not null"`        // Associated user ID (foreign key)
	RefreshToken string    `gorm:"not null;unique"` // The refresh token value
	ExpiresAt    time.Time `gorm:"not null"`        // Expiry date and time of the refresh token
	IssuedAt     time.Time `gorm:"not null"`        // Timestamp when the token was issued
	Revoked      bool      `gorm:"default:false"`   // Flag to mark if the token has been revoked
	Device       string    `gorm:"size:255"`        // Device information (optional)
	IPAddress    string    `gorm:"size:255"`        // IP address of the user (optional)
	UserAgent    string    `gorm:"size:500"`        // User agent string (optional, for browser/device info)
}
