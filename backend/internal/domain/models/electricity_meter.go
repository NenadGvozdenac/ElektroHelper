package models

type ElectricityMeter struct {
	ID                 uint   `gorm:"primary_key"`
	LocationID         uint   `json:"location_id"`
	MeterCode          string `json:"meter_code"`
	DateOfRegistration string `json:"date_of_registration"`
}
