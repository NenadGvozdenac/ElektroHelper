package main

import (
	"elektrohelper/backend/config"
	"elektrohelper/backend/internal/app/utils/logger"
	"elektrohelper/backend/routes"
	"io"
	"sync"

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
	gin.DefaultWriter = io.Discard
	router := gin.New()
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
