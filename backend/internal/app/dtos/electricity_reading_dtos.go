package dtos

// Used for default request body
type CreateElectricityReadingDTO struct {
	ElectricityMeterID uint   `json:"electricity_meter_id" binding:"required"`
	LowerReading       string `json:"lower_reading" binding:"required"`
	UpperReading       string `json:"upper_reading" binding:"required"`
}

// Used for request body with date
type CreateElectricityReadingWithDateDTO struct {
	ElectricityMeterID uint   `json:"electricity_meter_id" binding:"required"`
	LowerReading       string `json:"lower_reading" binding:"required"`
	UpperReading       string `json:"upper_reading" binding:"required"`
	ReadingDate        string `json:"reading_date" binding:"required"`
}
