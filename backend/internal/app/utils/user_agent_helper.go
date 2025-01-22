package utils

import "github.com/mssola/user_agent"

func ParseDeviceFromUserAgent(userAgent string) string {
	ua := user_agent.New(userAgent)
	if ua.Mobile() {
		return "Mobile"
	} else if ua.Bot() {
		return "Bot"
	}
	return "Desktop"
}
