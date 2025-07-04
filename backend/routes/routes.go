package routes

import (
	"elektrohelper/backend/internal/app/handlers"
	"elektrohelper/backend/internal/app/middleware"

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
	api.GET("/health", handlers.HealthCheck)
	api.POST("/register", handlers.Register)
	api.POST("/login", handlers.Login)
	api.POST("/logout", handlers.Logout)
	api.POST("/refresh_token", handlers.RefreshAccessToken)
}

func setupProtectedRoutes(api *gin.RouterGroup) {
	protected := api.Group("/")
	protected.Use(middleware.AuthMiddleware())

	protected.GET("/users", handlers.GetAllUsers)
	protected.GET("/users/:id", handlers.GetUserById)
	protected.GET("/user", handlers.GetActiveUser)

	protected.POST("/locations", handlers.CreateLocation)
	protected.GET("/locations", handlers.GetAllLocationsByUser)

	protected.POST("/electricity_meters", handlers.CreateElectricityMeter)
	protected.DELETE("/electricity_meters/:id", handlers.DeleteElectricityMeter)
	protected.GET("/electricity_meters", handlers.GetAllElectricityMeters)
	protected.GET("/electricity_meters/user", handlers.GetElectricityMetersByUserId)

	protected.POST("/electricity_readings", handlers.CreateElectricityReading)
	protected.POST("/electricity_readings/date", handlers.CreateElectricityReadingWithDate)
	protected.GET("/electricity_readings", handlers.GetAllElectricityReadingsByUserId)
}
