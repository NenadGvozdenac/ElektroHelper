package models

type Notification struct {
	ID               uint   `gorm:"primary_key"`
	Subject          string `json:"subject"`
	Message          string `json:"message"`
	NotificationDate string `json:"notification_date"`
	UserID           uint   `json:"user_id"`
}
