package utils

import (
	"encoding/json"
	"fmt"
)

func PrettyPrint(v interface{}) string {
	// Convert the interface to JSON with indentation
	prettyJSON, err := json.MarshalIndent(v, "", "  ")
	if err != nil {
		fmt.Println("Error marshalling object:", err)
		return ""
	}

	// Print the pretty JSON to the console
	fmt.Println(string(prettyJSON))

	// Return the pretty JSON as a string
	return string(prettyJSON)
}
