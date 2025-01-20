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

# Test stage (for running tests)
FROM golang:1.23.4 AS tester

WORKDIR /app

# Copy the source and dependencies
COPY --from=builder /app .

# Run the tests
CMD ["go", "test", "-short", "elektrohelper/backend/internal/app/handlers/test"]

# Final production stage
FROM alpine:latest  

WORKDIR /root/

# Copy the Pre-built binary file from the builder stage
COPY --from=builder /app/main .

# Copy the static folder into the container
COPY --from=builder /app/static ./static

EXPOSE 8080

CMD ["./main"]
