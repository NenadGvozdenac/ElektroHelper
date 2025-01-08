package models

type User struct {
	ID           uint   `gorm:"primary_key"`
	Name         string `json:"name"`
	Surname      string `json:"surname"`
	Email        string `json:"email"`
	Phone        string `json:"phone"`
	Password     string `json:"-"`
	CreationDate string `json:"creation_date"`
	Role         string `json:"role"`
}
