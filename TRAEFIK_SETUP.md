# ElektroHelper with Traefik API Gateway

This document describes how to run ElektroHelper with Traefik as an API Gateway.

## Overview

Traefik serves as a reverse proxy and API Gateway, routing requests to the appropriate microservices:

- **Main API (Go)**: `http://localhost/api/*`
- **Forums API (.NET)**: `http://localhost/forums-api/*`
- **Payment API (.NET)**: `http://localhost/payment-api/*`
- **Frontend (Vue.js)**: `http://localhost/*` (default route)
- **Traefik Dashboard**: `http://localhost:8080`

## Architecture

```
Internet → Traefik (Port 80) → Backend Services
                ↓
         Dashboard (Port 8080)
```

### Routing Rules

1. **Priority Routing**:
   - `/api/*` → Main Go API (port 8080)
   - `/forums-api/*` → Forums .NET API (port 9090) - strips `/forums-api` prefix
   - `/payment-api/*` → Payment .NET API (port 9091) - strips `/payment-api` prefix
   - `/*` → Frontend Vue.js app (port 5173)

2. **Path Stripping**:
   - Forums API: `/forums-api/posts` becomes `/posts` when forwarded
   - Payment API: `/payment-api/payments` becomes `/payments` when forwarded

## Features

### 🔒 **Security**
- CORS headers management
- Rate limiting (50 req/s, burst 100)
- Security headers (X-Frame-Options, X-Content-Type-Options, etc.)
- Basic auth for Traefik dashboard

### 📊 **Monitoring**
- Health checks for all services (`/api/health`)
- Traefik dashboard with real-time metrics
- Access logging
- Load balancing

### 🚀 **Performance**
- Load balancing with health checks
- Request caching headers
- Compression support

## Setup Instructions

### 1. Create Traefik Configuration

The following files are created:

```
traefik/
├── traefik.yml      # Static configuration
├── dynamic.yml      # Dynamic routing rules
└── acme.json        # SSL certificates (auto-created)
```

### 2. Environment Variables

Copy `.env.traefik` to your main `.env` file or source it:

```bash
# Update your .env file with Traefik-specific URLs
VITE_API_URL=http://localhost/api
VITE_FORUM_URL=http://localhost/forums-api
VITE_PAYMENT_URL=http://localhost/payment-api
```

### 3. Run with Docker Compose

```bash
# Start all services with Traefik
docker-compose up --build

# Or run in background
docker-compose up -d --build
```

### 4. Access Points

- **Application**: http://localhost
- **Traefik Dashboard**: http://localhost:8080
- **Main API Health**: http://localhost/api/health
- **Forums API Health**: http://localhost/forums-api/api/health
- **Payment API Health**: http://localhost/payment-api/api/health

## Health Checks

Each service now includes health check endpoints:

### Go Backend
```bash
curl http://localhost/api/health
```

### Forums API
```bash
curl http://localhost/forums-api/api/health
```

### Payment API
```bash
curl http://localhost/payment-api/api/health
```

## Traefik Dashboard

Access the dashboard at `http://localhost:8080` with:
- **Username**: admin
- **Password**: secure123

The dashboard shows:
- Real-time traffic
- Service health status
- Route mappings
- Error rates

## Frontend Configuration

Update your frontend service calls to use the new gateway URLs:

```typescript
// Old direct service calls
const API_URL = 'http://localhost:8080/api'
const FORUM_URL = 'http://localhost:9090/api'
const PAYMENT_URL = 'http://localhost:9091/api'

// New gateway calls
const API_URL = 'http://localhost/api'
const FORUM_URL = 'http://localhost/forums-api/api'
const PAYMENT_URL = 'http://localhost/payment-api/api'
```

## Benefits

### 🎯 **Centralized Entry Point**
- Single port (80) for all services
- Simplified client configuration
- Unified SSL termination

### 🔧 **Service Discovery**
- Automatic service registration via Docker labels
- Dynamic routing updates
- Zero-downtime deployments

### 📈 **Observability**
- Centralized access logs
- Metrics and monitoring
- Request tracing

### 🛡️ **Security & Resilience**
- Rate limiting per service
- Circuit breaker patterns
- Request/response transformations

## Troubleshooting

### Service Not Reachable
1. Check if service is running: `docker-compose ps`
2. Verify Traefik dashboard for route status
3. Check service health endpoints

### CORS Issues
- Verify CORS headers in `traefik/dynamic.yml`
- Check browser developer tools for specific errors

### Rate Limiting
- Adjust rate limits in `traefik/dynamic.yml`
- Monitor dashboard for rate limit hits

## Development

For development, you can run individual services directly:

```bash
# Run only specific services
docker-compose up traefik app frontend-app

# Scale services
docker-compose up --scale app=2
```

This setup provides a production-ready API Gateway that can be easily extended with additional features like authentication, caching, and advanced monitoring.
