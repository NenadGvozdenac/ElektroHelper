package utils

import (
	"strconv"

	"github.com/gin-gonic/gin"
)

func ConvertParamToUint(c *gin.Context, param_name string) uint {
	param := c.Param(param_name)
	paramUint, _ := strconv.ParseUint(param, 10, 64)
	return uint(paramUint)
}
