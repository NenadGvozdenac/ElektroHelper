package dtos

type CreateElectricityMeterDTO struct {
	MeterCode  string `json:"meter_code" binding:"required"`
	LocationID uint   `json:"location_id" binding:"required"`
}
