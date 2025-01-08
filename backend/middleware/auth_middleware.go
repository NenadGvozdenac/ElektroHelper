package middleware

import (
	"elektrohelper/backend/utils"
	"net/http"
	"strings"

	"github.com/gin-gonic/gin"
)

func AuthMiddleware() gin.HandlerFunc {
	return func(c *gin.Context) {
		authHeader := c.GetHeader("Authorization")
		if authHeader == "" || !strings.HasPrefix(authHeader, "Bearer ") {
			utils.CreateGinResponse(c, "Authorization header is required", http.StatusUnauthorized, nil)
			c.Abort()
			return
		}

		token := strings.TrimPrefix(authHeader, "Bearer ")
		claims, err := utils.ValidateToken(token)
		if err != nil {
			utils.CreateGinResponse(c, "Invalid token", http.StatusUnauthorized, nil)
			c.Abort()
			return
		}

		userIDFloat, ok := claims["userID"].(float64)
		if !ok {
			utils.CreateGinResponse(c, "Invalid user ID format in token", http.StatusUnauthorized, nil)
			c.Abort()
			return
		}

		userID := uint(userIDFloat)
		c.Set("userID", userID)
		c.Next()
	}
}
