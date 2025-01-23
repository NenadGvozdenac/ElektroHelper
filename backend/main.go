package main

import (
	"elektrohelper/backend/config"
	"elektrohelper/backend/internal/app/utils/logger"
	"elektrohelper/backend/routes"
	"sync"

	"github.com/gin-contrib/cors"
	"github.com/gin-gonic/gin"
)

func startDatabase() error {
	logger.Info("Connecting to database...")

	err := config.ConnectDatabase()

	if err != nil {
		logger.Fatal("Failed to connect to database")
		return err
	}

	logger.Info("Connected to database")
	return nil
}

func startServer() error {
	gin.SetMode(gin.ReleaseMode)
	router := gin.Default()

	// Configure CORS middleware
	router.Use(cors.New(cors.Config{
		AllowOrigins:     []string{"http://localhost:5173"}, // Allow requests from localhost:5173
		AllowMethods:     []string{"GET", "POST", "PUT", "DELETE", "OPTIONS"},
		AllowHeaders:     []string{"Origin", "Content-Type", "Authorization"},
		ExposeHeaders:    []string{"Content-Length"},
		AllowCredentials: true,
	}))

	// Set up routes
	routes.SetupRoutes(router)

	logger.Info("Starting server on port 8080")
	err := router.Run(":8080")
	if err != nil {
		return err
	}

	return nil
}

func addToGoroutineGroup(f func() error, wg *sync.WaitGroup, errChan chan error) {
	wg.Add(1)
	go func() {
		defer wg.Done()
		err := f()
		if err != nil {
			errChan <- err
		}
	}()
}

func endApplication(errChan chan error) {
	for err := range errChan {
		if err != nil {
			logger.Fatal(err.Error())
		}
	}

	logger.Info("Application shut down")
}

func main() {
	var wg sync.WaitGroup

	// Use a channel to capture errors from goroutines
	errChan := make(chan error, 1)

	// Goroutine for database connection
	addToGoroutineGroup(startDatabase, &wg, errChan)

	// Goroutine for starting the server
	addToGoroutineGroup(startServer, &wg, errChan)

	// Wait for all goroutines to finish
	go func() {
		wg.Wait()
		close(errChan)
	}()

	// Listen for errors and log them
	endApplication(errChan)
}
