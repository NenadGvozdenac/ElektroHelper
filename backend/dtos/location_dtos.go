package dtos

type LocationDTO struct {
	Street     string `json:"street"`
	Number     string `json:"number"`
	City       string `json:"city"`
	Country    string `json:"country"`
	PostalCode string `json:"postal_code"`
}

type LocationResponseDTO struct {
	ID         uint   `json:"id"`
	Street     string `json:"street"`
	Number     string `json:"number"`
	City       string `json:"city"`
	Country    string `json:"country"`
	PostalCode string `json:"postal_code"`
}
