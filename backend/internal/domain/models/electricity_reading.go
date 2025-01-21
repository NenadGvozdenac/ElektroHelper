package models

type ElectricityReading struct {
	ID                 uint   `gorm:"primaryKey" json:"id"`
	ElectricityMeterID uint   `json:"electricity_meter_id"`
	LowerReading       string `json:"lower_reading"`
	UpperReading       string `json:"upper_reading"`
	ReadingDate        string `json:"reading_date"`
}
