package models

type ElectricityMeter struct {
	ID                 uint   `gorm:"primary_key"`
	LocationID         uint   `json:"location_id"`
	DateOfRegistration string `json:"date_of_registration"`
}
