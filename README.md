# ElektroHelper

ElektroHelper is a comprehensive microservices-based application designed to manage electricity data, community forums, and payment processing. The platform combines utility management with social features, creating a complete ecosystem for property management and community engagement.

---

## Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Technology Stack](#technology-stack)
- [Microservices](#microservices)
- [Infrastructure](#infrastructure)
- [Features](#features)
- [Setup and Installation](#setup-and-installation)
- [API Documentation](#api-documentation)
- [Contributing](#contributing)
- [License](#license)

---

## Overview

ElektroHelper is a modern, cloud-ready application built with microservices architecture and containerized deployment. It provides:

- **Electricity Management**: Track meter readings, locations, and consumption data
- **Community Forums**: Social platform with posts, comments, voting, and user profiles
- **Payment Processing**: Integrated payment system with history tracking
- **Real-time Search**: Elasticsearch-powered search functionality
- **API Gateway**: Traefik-based routing and load balancing
- **Cloud Deployment**: AWS-ready with ECS, CloudFormation, and Terraform support

---

## Architecture

```
┌──────────────────────────────────────────────────────────────────┐
│                        Traefik API Gateway                       │
│                         (Port 80/443)                            │
└──────────────────────┬───────────────────────────────────────────┘
                       │
      ┌────────────────┼────────────────┬──────────────────────────┐
      │                │                │                          │
┌─────▼─────┐   ┌──────▼──────┐   ┌─────▼─────┐             ┌──────▼──────┐
│ Main API  │   │  Forums API │   │  Payment  │             │  Frontend   │
│    (Go)   │   │   (.NET)    │   │ API(.NET) │             │   (Vue.js)  │
│ Port 8080 │   │  Port 9090  │   │ Port 9091 │             │  Port 5173  │
└─────┬─────┘   └──────┬──────┘   └─────┬─────┘             └─────────────┘
      │                │                │
┌─────▼─────┐   ┌──────▼──────┐   ┌─────▼─────┐
│PostgreSQL │   │    Neo4j    │   │  MongoDB  │
│           │   │Elasticsearch│   │           │
└───────────┘   └─────────────┘   └───────────┘
```

### Routing Rules (Traefik)
- `/api/*` → Main Go API (electricity management)
- `/forums-api/*` → Forums .NET API (community features)
- `/payment-api/*` → Payment .NET API (payment processing)
- `/*` → Frontend Vue.js application

---

## Technology Stack

### Frontend
- **Framework**: Vue.js 3 with TypeScript
- **Build Tool**: Vite 6
- **Styling**: Tailwind CSS 3
- **UI Components**: Headless UI, Lucide Icons
- **HTTP Client**: Axios
- **Routing**: Vue Router 4
- **State Management**: Composables pattern

### Backend Services

#### Main API (Go)
- **Runtime**: Go 1.23.4
- **Framework**: Gin web framework
- **Database**: PostgreSQL with GORM ORM
- **Authentication**: JWT tokens
- **Validation**: Go Playground Validator
- **CORS**: Gin-contrib/cors
- **Testing**: Testify, SQL Mock

#### Forums API (.NET)
- **Runtime**: .NET 8.0
- **Architecture**: Clean Architecture + CQRS
- **Database**: Neo4j (Graph) + Elasticsearch (Search)
- **Patterns**: MediatR, Repository Pattern
- **Authentication**: JWT Bearer tokens
- **Documentation**: Swagger/OpenAPI
- **Background Services**: Post synchronization

#### Payment API (.NET)
- **Runtime**: .NET 8.0
- **Architecture**: Clean Architecture + CQRS
- **Database**: MongoDB (Document store)
- **Patterns**: MediatR, Repository Pattern
- **Authentication**: JWT Bearer tokens
- **Documentation**: Swagger/OpenAPI

### Infrastructure & DevOps
- **API Gateway**: Traefik v3.0
- **Containerization**: Docker & Docker Compose
- **Orchestration**: Docker Swarm / Kubernetes ready
- **Cloud Platform**: AWS (ECS, RDS, DocumentDB, Neptune)
- **Infrastructure as Code**: CloudFormation, Terraform
- **Monitoring**: Traefik Dashboard, Health Check endpoints
- **Email**: MailHog (development), AWS SES (production)

### Databases
- **PostgreSQL**: User data, locations, electricity readings
- **Neo4j**: Social graph (users, posts, comments, relationships)
- **MongoDB**: Payment transactions and history
- **Elasticsearch**: Full-text search for forum posts

---

## Microservices

### 1. Main API Service (Go)
**Purpose**: Core electricity management and user authentication
**Port**: 8080
**Database**: PostgreSQL

**Key Features**:
- User registration and authentication
- Location management
- Electricity meter tracking
- Reading data collection
- Email notifications
- Health check endpoint

**Key Endpoints**:
- `POST /api/register` - User registration
- `POST /api/login` - User authentication
- `GET /api/locations` - Get user locations
- `POST /api/electricity_meters` - Register meter
- `POST /api/electricity_readings` - Submit readings

### 2. Forums API Service (.NET)
**Purpose**: Community forums and social features
**Port**: 9090
**Database**: Neo4j + Elasticsearch

**Key Features**:
- Forum management
- Post creation and management
- Comment system
- Voting system (upvote/downvote)
- User profiles and following
- Real-time search
- RSS feed generation
- Admin moderation tools

**Key Endpoints**:
- `GET /api/forums` - List forums
- `POST /api/posts` - Create post
- `GET /api/posts/{id}` - Get post details
- `POST /api/comments` - Add comment
- `POST /api/voting/upvote` - Vote on posts
- `GET /api/search` - Search posts

### 3. Payment API Service (.NET)
**Purpose**: Payment processing and transaction management
**Port**: 9091
**Database**: MongoDB

**Key Features**:
- Payment processing
- Transaction history
- Payment method management
- Billing integration
- Payment status tracking

**Key Endpoints**:
- `POST /api/payments` - Process payment
- `GET /api/payments` - Get payment history
- `GET /api/payments/{id}` - Get payment details

---

## Infrastructure

### Development Environment
```yaml
services:
  traefik:           # API Gateway (Port 80, 443, 8080)
  app:               # Main API (Go)
  forums_app:        # Forums API (.NET)
  payment_app:       # Payment API (.NET)
  frontend-app:      # Vue.js Frontend
  db:                # PostgreSQL
  mongo_db:          # MongoDB
  neo4j:             # Neo4j Graph Database
  elasticsearch:     # Elasticsearch
  mail:              # MailHog (Development mail server)
```

### Production Deployment (AWS)
- **ECS Fargate**: Container orchestration
- **Application Load Balancer**: Traffic distribution
- **RDS PostgreSQL**: Managed relational database
- **DocumentDB**: MongoDB-compatible document database
- **Neptune**: Managed graph database
- **Elasticsearch Service**: Managed search
- **SES**: Email service
- **CloudWatch**: Logging and monitoring
- **Route 53**: DNS management
- **Certificate Manager**: SSL/TLS certificates

### Security Features
- JWT-based authentication across all services
- CORS configuration for secure cross-origin requests
- HTTPS/TLS encryption in production
- Environment-based configuration
- Secrets management
- API rate limiting via Traefik

---

## Features

### Core Electricity Management
- **User Registration**: Secure account creation with email verification
- **Location Management**: Register and manage multiple properties
- **Meter Registration**: Track electricity meters per location
- **Reading Collection**: Record and monitor consumption data
- **Historical Analytics**: View consumption trends and patterns
- **Notifications**: Email alerts for readings and updates

### Community Forums
- **Multi-Forum Support**: Organized discussion spaces
- **Rich Post Editor**: Create detailed posts with formatting
- **Threaded Comments**: Nested comment discussions
- **Voting System**: Community-driven content ranking
- **User Profiles**: Personal profiles with activity history
- **Social Following**: Follow other community members
- **Advanced Search**: Find posts and discussions instantly
- **RSS Feeds**: Stay updated with latest content
- **Moderation Tools**: Admin controls for content management

### Payment Processing
- **Secure Payments**: PCI-compliant payment processing
- **Multiple Payment Methods**: Credit cards, PayPal integration
- **Transaction History**: Complete payment audit trail
- **Billing Integration**: Automated billing for utility services
- **Payment Reminders**: Automated notification system

### Technical Features
- **API Gateway**: Centralized routing and load balancing
- **Health Monitoring**: Real-time service health checks
- **Auto-scaling**: Container-based horizontal scaling
- **Microservices**: Independent, scalable service architecture
- **Cloud-Ready**: AWS deployment with infrastructure as code
- **Monitoring**: Comprehensive logging and metrics
- **Development Tools**: Hot reload, debugging, testing suites

---

## Setup and Installation

### Prerequisites

- **Docker** & **Docker Compose**: For containerized development
- **Node.js** 18+ (for local frontend development)
- **Go** 1.23+ (for local backend development)
- **.NET** 8.0 SDK (for local .NET service development)
- **Git**: For version control

### Development Setup

1. **Clone the Repository**
   ```bash
   git clone https://github.com/NenadGvozdenac/ElektroHelper/tree/feat/traefik
   cd ElektroHelper
   ```

2. **Environment Configuration**
   ```bash
   # Copy environment template
   cp .env.example .env
   
   # Configure your environment variables
   # Edit .env file with your specific settings
   ```

3. **Start Development Environment**
   ```bash
   # Start all services with Traefik API Gateway
   docker-compose up --build
   
   # Or start specific services
   docker-compose up traefik app frontend-app
   ```

4. **Access the Application**
   - **Frontend**: http://localhost (routed through Traefik)
   - **Main API**: http://localhost/api (Go service)
   - **Forums API**: http://localhost/forums-api (.NET service)
   - **Payment API**: http://localhost/payment-api (.NET service)
   - **Traefik Dashboard**: http://localhost:8080
   - **MailHog UI**: http://localhost:8025

5. **Database Initialization**
   ```bash
   # Databases are automatically initialized with starter data
   # PostgreSQL: ./database/starter.sql
   # Neo4j: Automatically configured
   # MongoDB: Automatically configured
   # Elasticsearch: Automatically configured
   ```

### Local Development (Without Docker)

1. **Backend Services**
   ```bash
   # Main API (Go)
   cd backend
   go mod download
   go run main.go
   
   # Forums API (.NET)
   cd forums_backend
   dotnet restore
   dotnet run
   
   # Payment API (.NET)
   cd payment_backend
   dotnet restore
   dotnet run
   ```

2. **Frontend**
   ```bash
   cd frontend
   npm install
   npm run dev
   ```

3. **Database Setup**
   - PostgreSQL: Create database and run `./database/starter.sql`
   - Neo4j: Install and configure with default credentials
   - MongoDB: Install and configure
   - Elasticsearch: Install and configure

### Testing

```bash
# Run all tests
docker-compose --profile testing up --build

# Run specific service tests
cd backend && go test ./...
cd forums_backend && dotnet test
cd payment_backend && dotnet test

# Integration tests
docker-compose up db_test
docker-compose up tester
```

---

## API Documentation

### Interactive Documentation
- **Main API**: http://localhost:8080/swagger (when running locally)
- **Forums API**: http://localhost:9090/swagger
- **Payment API**: http://localhost:9091/swagger

### Key API Endpoints

#### Authentication
```http
POST /api/register        # User registration
POST /api/login          # User login
POST /api/refresh_token  # Token refresh
POST /api/logout         # User logout
```

#### Electricity Management
```http
GET    /api/locations              # Get user locations
POST   /api/locations              # Create location
GET    /api/electricity_meters     # Get meters
POST   /api/electricity_meters     # Register meter
GET    /api/electricity_readings   # Get readings
POST   /api/electricity_readings   # Submit reading
```

#### Forums
```http
GET    /forums-api/api/forums      # List forums
POST   /forums-api/api/forums      # Create forum
GET    /forums-api/api/posts       # List posts
POST   /forums-api/api/posts       # Create post
GET    /forums-api/api/search      # Search posts
POST   /forums-api/api/voting      # Vote on content
```

#### Payments
```http
GET    /payment-api/api/payments   # Payment history
POST   /payment-api/api/payments   # Process payment
GET    /payment-api/api/payments/:id  # Payment details
```

#### Health Checks
```http
GET /api/health              # Main API health
GET /forums-api/api/health   # Forums API health
GET /payment-api/api/health  # Payment API health
```

---

## Contributing

1. **Fork the Repository**
2. **Create Feature Branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```

3. **Make Changes**
   - Follow the existing code style
   - Add tests for new functionality
   - Update documentation as needed

4. **Test Your Changes**
   ```bash
   # Run all tests
   docker-compose --profile testing up --build
   
   # Test specific services
   cd backend && go test ./...
   cd frontend && npm test
   ```

5. **Submit Pull Request**
   - Provide clear description of changes
   - Include screenshots for UI changes
   - Ensure all tests pass

### Development Guidelines

- **Go Code**: Follow Go best practices and gofmt standards
- **TypeScript**: Use strict TypeScript with proper typing
- **C# Code**: Follow .NET conventions and clean architecture
- **Vue.js**: Use Composition API and TypeScript
- **Database**: Include migrations for schema changes
- **Documentation**: Update README and API docs for changes

---

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---

## Support

For questions, issues, or contributions:

1. **GitHub Issues**: Report bugs and request features
2. **Discussions**: General questions and community support
3. **Documentation**: Check `TRAEFIK_SETUP.md` for deployment help
4. **AWS Deployment**: See `aws/README.md` for cloud deployment

---

*ElektroHelper - Modern Electricity Management with Community Features*