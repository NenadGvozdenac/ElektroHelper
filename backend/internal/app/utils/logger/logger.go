package logger

import (
	"fmt"
	"os"
	"time"
)

type LogLevel string

const (
	INFO  LogLevel = "INFO"
	WARN  LogLevel = "WARN"
	ERROR LogLevel = "ERROR"
	FATAL LogLevel = "FATAL"
)

var environment string = os.Getenv("GO_ENV")

func logMessage(level LogLevel, message string) {
	if environment == "test" {
		return
	}

	currentTime := time.Now().Format("2.1.2006 15:04:05")
	fmt.Printf("[%s] [%s] %s\n", currentTime, level, message)
}

func combineMessages(message []string) string {
	messageString := ""
	for _, m := range message {
		messageString += m + " "
	}
	return messageString
}

func Info(message ...string) {
	logMessage(INFO, combineMessages(message))
}

func Warn(message ...string) {
	logMessage(WARN, combineMessages(message))
}

func Error(message ...string) {
	logMessage(ERROR, combineMessages(message))
}

func Fatal(message ...string) {
	logMessage(FATAL, combineMessages(message))
}

func LogMessage(message string, code int) {
	if code >= 500 {
		Error(message)
	} else if code >= 400 {
		Warn(message)
	} else {
		Info(message)
	}
}
