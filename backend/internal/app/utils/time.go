package utils

import "time"

func GetCurrentTime() time.Time {
	return time.Now()
}

// GetCurrentTimeFormatted returns the current time as a formatted string
// in the "DD-MM-YYYY HH:MM:SS" format.
var GetCurrentTimeFormatted = func() string {
	const layout = "02-01-2006 15:04:05" // DD-MM-YYYY HH:MM:SS
	return time.Now().Format(layout)
}
