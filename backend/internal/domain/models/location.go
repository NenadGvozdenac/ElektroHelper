package models

type Location struct {
	ID         uint   `gorm:"primary_key"`
	Street     string `json:"street"`
	Number     string `json:"number"`
	City       string `json:"city"`
	Country    string `json:"country"`
	PostalCode string `json:"postal_code"`
	UserID     uint   `json:"user_id"`
}
