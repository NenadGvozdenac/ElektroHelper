package dtos

type CreateLocationDTO struct {
	Street     string `json:"street"`
	Number     string `json:"number"`
	City       string `json:"city"`
	Country    string `json:"country"`
	PostalCode string `json:"postal_code"`
}
