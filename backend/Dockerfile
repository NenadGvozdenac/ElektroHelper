# Build stage
FROM golang:1.23.4 AS builder

WORKDIR /app

# Copy go mod and sum files
COPY go.mod go.sum ./
RUN go mod download

# Copy the source code and static files
COPY . .

# Build the Go app as a static binary
RUN CGO_ENABLED=0 GOOS=linux go build -a -installsuffix cgo -o main ./main.go

# Production stage
FROM alpine:latest  

# Create app directory with proper permissions
RUN mkdir -p /app/static && \
    adduser -D appuser && \
    chown -R appuser:appuser /app && \
    chmod -R 755 /app

WORKDIR /app

# Add necessary SSL certificates
RUN apk --no-cache add ca-certificates

# Copy the binary and static directory with explicit permissions
COPY --from=builder --chown=appuser:appuser /app/main .
COPY --from=builder --chown=appuser:appuser /app/static ./static

# Switch to non-root user
USER appuser

EXPOSE 8080

CMD ["./main"]