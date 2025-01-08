package main

import (
	"elektrohelper/backend/config"
	"elektrohelper/backend/routes"

	"github.com/gin-gonic/gin"
)

func main() {
	config.ConnectDatabase()

	router := gin.Default()
	routes.SetupRoutes(router)

	router.Run(":8080")
}
