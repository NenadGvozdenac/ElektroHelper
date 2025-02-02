package utils

import "time"

func GetCurrentTime() time.Time {
	return time.Now()
}

// GetCurrentTimeFormatted returns the current time as a formatted string
var GetCurrentTimeFormatted = func() string {
	const layout = "2006-01-02T15:04:05Z07:00"
	return time.Now().Format(layout)
}
