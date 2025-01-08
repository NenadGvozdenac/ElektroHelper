# ElektroHelper

ElektroHelper is a full-stack application designed to manage and monitor electricity-related data efficiently. It includes features for user management, location tracking, electricity meter readings, and notifications. The project leverages Docker for containerization, Go for backend development, Next.js for frontend development, and PostgreSQL as the database solution.

---

## Table of Contents

- [Description](#description)
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Dependencies](#dependencies)
- [Database Schema](#database-schema)
- [Setup and Installation](#setup-and-installation)
  - [Prerequisites](#prerequisites)
  - [Development Setup](#development-setup)
- [Contributing](#contributing)
- [License](#license)

---

## Description

ElektroHelper is designed to streamline the management of electricity data, allowing users to register locations, track electricity meters, monitor readings, and receive notifications. The application is fully containerized using Docker and orchestrated with Docker Compose, ensuring a smooth development and deployment experience.

---

## Features

- **User Management**: Create, manage, and authenticate users with secure password handling.
- **Location Management**: Associate users with specific locations.
- **Electricity Meter Tracking**: Register electricity meters and associate them with locations.
- **Readings Management**: Log and view electricity meter readings.
- **Notification System**: Send notifications to users regarding their electricity data.

---

## Tech Stack

- **Backend**: Go
- **Frontend**: Next.js
- **Database**: PostgreSQL
- **Containerization**: Docker, Docker Compose
- **ORM**: GORM for Go
- **Authentication**: JWT (JSON Web Tokens)
- **Validation**: Go Playground Validator

---

## Dependencies

### Backend (`elektrohelper/backend`)

#### Core
- `github.com/gin-gonic/gin`: Web framework for Go.
- `github.com/golang-jwt/jwt`: JWT implementation for authentication.
- `gorm.io/gorm`: ORM for database interaction.
- `gorm.io/driver/postgres`: PostgreSQL driver for GORM.

#### Utilities
- `github.com/go-playground/validator/v10`: Input validation library.
- `github.com/json-iterator/go`: JSON library for Go.
- `github.com/pelletier/go-toml/v2`: TOML parsing library.

#### Others
- For a full list, see the `go.mod` file.

---

## Database Schema

The application uses PostgreSQL with the following schema:

### Users
- `id SERIAL PRIMARY KEY`
- `name VARCHAR(255) NOT NULL`
- `surname VARCHAR(255) NOT NULL`
- `email VARCHAR(255) UNIQUE NOT NULL`
- `phone VARCHAR(15)`
- `password VARCHAR(255) NOT NULL`
- `creation_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP`
- `role VARCHAR(50) NOT NULL`

### Locations
- `id SERIAL PRIMARY KEY`
- `street VARCHAR(255) NOT NULL`
- `number VARCHAR(10) NOT NULL`
- `city VARCHAR(255) NOT NULL`
- `country VARCHAR(255) NOT NULL`
- `postal_code VARCHAR(10) NOT NULL`
- `user_id SERIAL NOT NULL`
  - Foreign key referencing `users(id)`

### Electricity Meters
- `id SERIAL PRIMARY KEY`
- `location_id INT NOT NULL`
  - Foreign key referencing `locations(id)`
- `date_of_registration TIMESTAMP DEFAULT CURRENT_TIMESTAMP`

### Electricity Readings
- `id SERIAL PRIMARY KEY`
- `electricity_meter_id INT NOT NULL`
  - Foreign key referencing `electricity_meters(id)`
- `reading_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP`
- `lower_reading VARCHAR(255) NOT NULL`
- `upper_reading VARCHAR(255) NOT NULL`

### Notifications
- `id SERIAL PRIMARY KEY`
- `user_id INT NOT NULL`
  - Foreign key referencing `users(id)`
- `notification_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP`
- `subject VARCHAR(255) NOT NULL`
- `message TEXT NOT NULL`

---

## Setup and Installation

### Prerequisites

- Docker and Docker Compose installed on your system.
- Go 1.23.4 or later installed.
- Node.js and npm (for Next.js development).

### Development Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/NenadGvozdenac/ElektroHelper.git
   cd elektrohelper
   ```

2. Build and start the containers:
   ```bash
   docker-compose up --build
   ```

3. The backend will be accessible at `http://localhost:8080`, and the frontend will be accessible at `http://localhost:3000`.

## Contributing

Contributions are welcome! Please fork the repository, make changes, and submit a pull request.

---

## License

This project is licensed under the [MIT License](LICENSE).