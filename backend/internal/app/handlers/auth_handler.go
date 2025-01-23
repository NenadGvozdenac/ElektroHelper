package handlers

import (
	"elektrohelper/backend/config"
	"elektrohelper/backend/internal/app/dtos"
	"elektrohelper/backend/internal/app/repositories"
	"elektrohelper/backend/internal/app/utils"
	"elektrohelper/backend/internal/domain/models"
	"net/http"
	"time"

	"github.com/gin-gonic/gin"
	"golang.org/x/crypto/bcrypt"
)

// Register a new user and return a JWT
func Register(c *gin.Context) {
	var user dtos.RegisterDTO
	if err := c.ShouldBindJSON(&user); err != nil {
		utils.CreateGinResponse(c, "Invalid input", http.StatusBadRequest, nil)
		return
	}

	// Check if the email is already in use
	var existingUser models.User
	if err := config.DB.Where("email = ?", user.Email).First(&existingUser).Error; err == nil {
		utils.CreateGinResponse(c, "Email already in use", http.StatusBadRequest, nil)
		return
	}

	// Hash the password
	hashedPassword, err := bcrypt.GenerateFromPassword([]byte(user.Password), bcrypt.DefaultCost)
	if err != nil {
		utils.CreateGinResponse(c, "Failed to hash password", http.StatusInternalServerError, nil)
		return
	}

	newUser := models.User{
		Name:         user.Name,
		Surname:      user.Surname,
		Email:        user.Email,
		Phone:        user.Phone,
		Password:     string(hashedPassword),
		Role:         "user",
		CreationDate: utils.GetCurrentTimeFormatted(),
	}

	if err := config.DB.Create(&newUser).Error; err != nil {
		utils.CreateGinResponse(c, "Failed to create user", http.StatusInternalServerError, nil)
		return
	}

	sendMail := c.DefaultQuery("sendMail", "true") == "true"

	if sendMail {
		if err := SendRegistrationMail(newUser.ID, newUser.Email, newUser.Name); err != nil {
			utils.CreateGinResponse(c, "Failed to send registration mail", http.StatusInternalServerError, nil)
			return
		}
	}

	// Generate access and refresh tokens
	token, err := utils.GenerateToken(newUser.ID, newUser.Email, newUser.Role, newUser.Name)
	if err != nil {
		utils.CreateGinResponse(c, "Failed to generate token", http.StatusInternalServerError, nil)
		return
	}

	refreshToken, err := utils.GenerateRefreshToken(newUser.ID)
	if err != nil {
		utils.CreateGinResponse(c, "Failed to generate refresh token", http.StatusInternalServerError, nil)
		return
	}

	// Extract device, IP, and User-Agent info
	ipAddress := c.ClientIP()
	userAgent := c.GetHeader("User-Agent")
	device := utils.ParseDeviceFromUserAgent(userAgent)

	// Save the refresh token
	expiresAt := time.Now().Add(30 * 24 * time.Hour)
	if err := saveRefreshToken(newUser.ID, refreshToken, device, ipAddress, userAgent, expiresAt); err != nil {
		utils.CreateGinResponse(c, "Failed to save refresh token", http.StatusInternalServerError, nil)
		return
	}

	tokenDTO := dtos.TokenDTO{
		Token:        token,
		RefreshToken: refreshToken,
	}

	utils.CreateGinResponse(c, "User registered successfully", http.StatusCreated, tokenDTO)
}

// Login a user and return a JWT
func Login(c *gin.Context) {
	var input dtos.LoginDTO

	if err := c.ShouldBindJSON(&input); err != nil {
		utils.CreateGinResponse(c, "Invalid input", http.StatusBadRequest, nil)
		return
	}

	// Find the user
	var user models.User
	if err := config.DB.Where("email = ?", input.Email).First(&user).Error; err != nil {
		utils.CreateGinResponse(c, "Invalid email or password", http.StatusUnauthorized, nil)
		return
	}

	// Compare passwords
	if err := bcrypt.CompareHashAndPassword([]byte(user.Password), []byte(input.Password)); err != nil {
		utils.CreateGinResponse(c, "Invalid email or password", http.StatusUnauthorized, nil)
		return
	}

	// Generate access and refresh tokens
	token, err := utils.GenerateToken(user.ID, user.Email, user.Role, user.Name)
	if err != nil {
		utils.CreateGinResponse(c, "Failed to generate token", http.StatusInternalServerError, nil)
		return
	}

	refreshToken, err := utils.GenerateRefreshToken(user.ID)
	if err != nil {
		utils.CreateGinResponse(c, "Failed to generate refresh token", http.StatusInternalServerError, nil)
		return
	}

	// Extract device, IP, and User-Agent info
	ipAddress := c.ClientIP()
	userAgent := c.GetHeader("User-Agent")
	device := utils.ParseDeviceFromUserAgent(userAgent)

	// Save the refresh token
	expiresAt := time.Now().Add(30 * 24 * time.Hour)

	if err := saveRefreshToken(user.ID, refreshToken, device, ipAddress, userAgent, expiresAt); err != nil {
		utils.CreateGinResponse(c, "Failed to save refresh token", http.StatusInternalServerError, nil)
		return
	}

	tokenDTO := dtos.TokenDTO{
		Token:        token,
		RefreshToken: refreshToken,
	}

	utils.CreateGinResponse(c, "User logged in successfully", http.StatusOK, tokenDTO)
}

func Logout(c *gin.Context) {
	type RequestBody struct {
		refresh_token string `json:"refresh_token"`
	}

	var input RequestBody

	if err := c.ShouldBindJSON(&input); err != nil {
		utils.CreateGinResponse(c, "Invalid input", http.StatusBadRequest, nil)
		return
	}

	if err := invalidateRefreshToken(input.refresh_token); err != nil {
		utils.CreateGinResponse(c, "Failed to invalidate refresh token", http.StatusInternalServerError, nil)
		return
	}

	utils.CreateGinResponse(c, "User logged out successfully", http.StatusOK, nil)
}

func RefreshAccessToken(c *gin.Context) {
	var requestBody struct {
		RefreshToken string `json:"refresh_token"`
	}

	if err := c.ShouldBindJSON(&requestBody); err != nil {
		utils.CreateGinResponse(c, "Invalid input", http.StatusBadRequest, nil)
		return
	}

	// Validate the refresh token
	claims, err := utils.ValidateRefreshToken(requestBody.RefreshToken)
	if err != nil {
		utils.CreateGinResponse(c, "Invalid or expired refresh token", http.StatusUnauthorized, nil)
		return
	}

	// Extract user ID from claims
	userID, ok := claims["userID"].(float64) // JWT stores numeric values as float64
	if !ok {
		utils.CreateGinResponse(c, "Invalid refresh token claims", http.StatusUnauthorized, nil)
		return
	}

	// TODO: Check if the token is still valid in the database
	if _, err := repositories.NewTokenRepository().GetByToken(requestBody.RefreshToken); err != nil {
		if err := invalidateRefreshToken(requestBody.RefreshToken); err != nil {
			utils.CreateGinResponse(c, "Failed to invalidate refresh token", http.StatusInternalServerError, nil)
		} else {
			utils.CreateGinResponse(c, "Invalid refresh token", http.StatusUnauthorized, nil)
		}
		return
	}

	// Generate a new access token
	var user models.User
	if err := config.DB.Where("id = ?", uint(userID)).First(&user).Error; err != nil {
		utils.CreateGinResponse(c, "User not found", http.StatusUnauthorized, nil)
		return
	}

	newAccessToken, err := utils.GenerateToken(user.ID, user.Email, user.Role, user.Name)
	if err != nil {
		utils.CreateGinResponse(c, "Failed to generate new access token", http.StatusInternalServerError, nil)
		return
	}

	tokenDTO := dtos.TokenDTO{
		Token:        newAccessToken,
		RefreshToken: requestBody.RefreshToken,
	}

	utils.CreateGinResponse(c, "Access token refreshed successfully", http.StatusOK, tokenDTO)
}

func saveRefreshToken(userID uint, refreshToken, device, ipAddress, userAgent string, expiresAt time.Time) error {
	newToken := models.Token{
		UserID:       userID,
		RefreshToken: refreshToken,
		ExpiresAt:    expiresAt,
		IssuedAt:     time.Now(),
		Device:       device,
		IPAddress:    ipAddress,
		UserAgent:    userAgent,
	}

	if err := repositories.NewTokenRepository().Create(&newToken); err != nil {
		return err
	}

	return nil
}

func invalidateRefreshToken(refreshToken string) error {
	if err := repositories.NewTokenRepository().RevokeToken(refreshToken); err != nil {
		return err
	}
	return nil
}
