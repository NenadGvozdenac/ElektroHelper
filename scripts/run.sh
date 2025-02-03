#!/bin/bash

# # Stop and remove containers and volumes, but leave the images
# docker-compose down -v

# # Delete the previous app image
docker rmi elektrohelper-app

# # Start the databases first to ensure they are ready
docker-compose up -d db neo4j

# Wait for the PostgreSQL database to initialize
echo "Waiting for PostgreSQL to start..."
until docker exec -it $(docker ps -q -f "name=db") pg_isready -U postgres; do
  sleep 2
done
echo "PostgreSQL is ready!"

# Wait for the Neo4j database to initialize
echo "Waiting for Neo4j to start..."
until docker exec -it $(docker ps -q -f "name=neo4j") cypher-shell -u neo4j -p password "RETURN 1;" >/dev/null 2>&1; do
  sleep 2
done
echo "Neo4j is ready!"

# Start the application services
docker-compose up --build frontend-app app forums_app

echo "All services are up and running!"
