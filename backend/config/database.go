package config

import (
	"fmt"
	"log"

	"elektrohelper/backend/utils"

	"gorm.io/driver/postgres"
	"gorm.io/gorm"
)

var DB *gorm.DB

func ConnectDatabase() error {
	host := utils.GetEnvOrDefault("POSTGRES_HOST", "localhost")
	port := utils.GetEnvOrDefault("POSTGRES_PORT", "5432")
	user := utils.GetEnvOrDefault("POSTGRES_USER", "postgres")
	password := utils.GetEnvOrDefault("POSTGRES_PASSWORD", "password")
	dbname := utils.GetEnvOrDefault("POSTGRES_DB", "elektrohelper")

	// Construct the connection string
	dsn := fmt.Sprintf("host=%s user=%s password=%s dbname=%s port=%s sslmode=disable",
		host, user, password, dbname, port)

	// Open the connection to the database
	var err error
	DB, err = gorm.Open(postgres.Open(dsn), &gorm.Config{})
	if err != nil {
		log.Fatal("Failed to connect to database: ", err)
		return err
	}

	log.Println("Database connection successful.")
	return nil
}
