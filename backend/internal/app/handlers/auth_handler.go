package handlers

import (
	"elektrohelper/backend/config"
	"elektrohelper/backend/internal/app/dtos"
	"elektrohelper/backend/internal/app/utils"
	"elektrohelper/backend/internal/domain/models"
	"net/http"

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

	if err := SendRegistrationMail(newUser.ID, newUser.Email, newUser.Name); err != nil {
		utils.CreateGinResponse(c, "Failed to send registration mail", http.StatusInternalServerError, nil)
		return
	}

	token, err := utils.GenerateToken(newUser.ID, newUser.Email, newUser.Role, newUser.Name)
	if err != nil {
		utils.CreateGinResponse(c, "Failed to generate token", http.StatusInternalServerError, nil)
		return
	}

	tokenDTO := dtos.TokenDTO{
		Token: token,
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

	// Generate JWT
	token, err := utils.GenerateToken(user.ID, user.Email, user.Role, user.Name)
	if err != nil {
		utils.CreateGinResponse(c, "Failed to generate token", http.StatusInternalServerError, nil)
		return
	}

	tokenDTO := dtos.TokenDTO{
		Token: token,
	}

	utils.CreateGinResponse(c, "User logged in successfully", http.StatusOK, tokenDTO)
}
