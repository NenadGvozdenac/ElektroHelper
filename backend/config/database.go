package config

import (
	"elektrohelper/backend/internal/app/utils"
	"fmt"

	"gorm.io/driver/postgres"
	"gorm.io/gorm"
	"gorm.io/gorm/logger"
)

var DB *gorm.DB

func ConnectDatabase() error {
	host := utils.GetEnvOrDefault("POSTGRES_HOST", "localhost")
	port := utils.GetEnvOrDefault("POSTGRES_PORT", "5432")
	user := utils.GetEnvOrDefault("POSTGRES_USER", "postgres")
	password := utils.GetEnvOrDefault("POSTGRES_PASSWORD", "password")
	dbname := utils.GetEnvOrDefault("POSTGRES_DB", "elektrohelper")

	// Comment out or remove logger calls
	// logger.Info("Connecting to database...")

	// Construct the connection string
	dsn := fmt.Sprintf("host=%s user=%s password=%s dbname=%s port=%s sslmode=disable",
		host, user, password, dbname, port)

	// Open the connection to the database with no SQL logging
	var err error
	DB, err = gorm.Open(postgres.Open(dsn), &gorm.Config{
		Logger: logger.Discard,
	})
	if err != nil {
		return err
	}

	return nil
}
