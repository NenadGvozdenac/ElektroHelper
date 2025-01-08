package routes

import (
	"elektrohelper/backend/handlers"
	"elektrohelper/backend/middleware"

	"github.com/gin-gonic/gin"
)

func SetupRoutes(router *gin.Engine) {
	// Define the main API group with the prefix "/api"
	api := router.Group("/api")

	// Setup public routes
	setupPublicRoutes(api)

	// Setup protected routes
	setupProtectedRoutes(api)
}

func setupPublicRoutes(api *gin.RouterGroup) {
	api.POST("/register", handlers.Register)
	api.POST("/login", handlers.Login)
}

func setupProtectedRoutes(api *gin.RouterGroup) {
	protected := api.Group("/")
	protected.Use(middleware.AuthMiddleware())

	protected.GET("/users", handlers.GetAllUsers)
	protected.POST("/locations", handlers.CreateLocation)
	protected.GET("/locations", handlers.GetAllLocationsByUser)
}
