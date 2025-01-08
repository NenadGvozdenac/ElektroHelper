package main

import (
	"elektrohelper/backend/config"
	"elektrohelper/backend/routes"
	"log"
	"sync"

	"github.com/gin-gonic/gin"
)

func startDatabase() error {
	err := config.ConnectDatabase()
	if err != nil {
		return err
	}

	return nil
}

func startServer() error {
	router := gin.Default()
	routes.SetupRoutes(router)

	log.Println("Server is running on port 8080")
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
			log.Fatalf("Error: %v", err)
		}
	}

	log.Println("Application shut down gracefully")
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
