#!/bin/bash

# Stop and remove containers and volumes, but leave the images
docker-compose down -v

# Delete the previous app image
docker rmi elektrohelper-app

# Start the database first to ensure it's ready
docker-compose up -d db

# Wait for the database to initialize
echo "Waiting for the database to start..."
until docker exec -it $(docker ps -q -f "name=db") pg_isready -U your_user; do
  sleep 2
done

# Start the app service
docker-compose up --build app

# Stop and remove containers and volumes when finished
# docker-compose down -v
