#!/bin/bash

# Stop and remove containers and volumes, but leave the images
docker-compose down -v

# Delete the previous app and tester images
docker rmi elektrohelper-app elektrohelper-tester

# Build the app image
docker compose build app

# Start the tester container
docker-compose up tester

# Stop and remove containers, volumes, and remove app and tester images
docker-compose down -v

# Delete the previous app and tester images
docker rmi elektrohelper-app elektrohelper-tester