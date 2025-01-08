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

		c.Set("userID", claims["userID"])
		c.Next()
	}
}
