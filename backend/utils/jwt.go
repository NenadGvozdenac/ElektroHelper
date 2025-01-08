package utils

import (
	"errors"
	"time"

	"github.com/gin-gonic/gin"
	"github.com/golang-jwt/jwt/v5"
)

var jwtSecret = []byte("my_secret_key")

// GenerateToken generates a JWT for a given user ID
func GenerateToken(userID uint, userEmail string, userRole string, userName string) (string, error) {
	claims := jwt.MapClaims{
		"userID":    userID,
		"userEmail": userEmail,
		"userRole":  userRole,
		"userName":  userName,
		"exp":       time.Now().Add(time.Hour * 24).Unix(),
	}

	token := jwt.NewWithClaims(jwt.SigningMethodHS256, claims)
	return token.SignedString(jwtSecret)
}

// ValidateToken validates a JWT and returns the claims
func ValidateToken(tokenString string) (jwt.MapClaims, error) {
	token, err := jwt.Parse(tokenString, func(token *jwt.Token) (interface{}, error) {
		if _, ok := token.Method.(*jwt.SigningMethodHMAC); !ok {
			return nil, errors.New("unexpected signing method")
		}
		return jwtSecret, nil
	})

	if err != nil || !token.Valid {
		return nil, errors.New("invalid token")
	}

	claims, ok := token.Claims.(jwt.MapClaims)
	if !ok {
		return nil, errors.New("invalid claims")
	}

	return claims, nil
}

func ExtractUserIdFromContext(c *gin.Context) uint {
	userId := c.MustGet("userID").(uint)
	return userId
}

func ExtractUserEmailFromContext(c *gin.Context) string {
	userEmail := c.MustGet("userEmail").(string)
	return userEmail
}

func ExtractUserRoleFromContext(c *gin.Context) string {
	userRole := c.MustGet("userRole").(string)
	return userRole
}

func ExtractUserNameFromContext(c *gin.Context) string {
	userName := c.MustGet("userName").(string)
	return userName
}
