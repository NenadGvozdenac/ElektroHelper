package dtos

type CreateLocationDTO struct {
	Street     string `json:"street" binding:"required"`
	Number     string `json:"number" binding:"required"`
	City       string `json:"city" binding:"required"`
	Country    string `json:"country" binding:"required"`
	PostalCode string `json:"postal_code" binding:"required"`
}
