# Build stage
FROM golang:1.23.4 AS builder

WORKDIR /app

# Copy go mod and sum files
COPY go.mod go.sum ./

# Download all dependencies
RUN go mod download

# Copy the source code
COPY . .

# Build the Go app as a static binary
RUN CGO_ENABLED=0 GOOS=linux go build -a -installsuffix cgo -o main ./main.go

# Test stage
FROM golang:1.23.4 AS tester

WORKDIR /app

# Copy go mod and sum files first for better caching
COPY go.mod go.sum ./
RUN go mod download

# Copy the source code
COPY . .

# Set environment variable to indicate test environment
ENV GO_ENV=test

# Command to run tests
CMD ["go", "test", "-v", "elektrohelper/backend/tests/integration"]

# Production stage
FROM alpine:latest  

WORKDIR /root/

# Add necessary SSL certificates for HTTPS
RUN apk --no-cache add ca-certificates

# Copy the Pre-built binary file from the builder stage
COPY --from=builder /app/main .

# Copy the static folder into the container
COPY --from=builder /app/static ./static

# Create a non-root user
RUN adduser -D appuser
USER appuser

EXPOSE 8080

CMD ["./main"]