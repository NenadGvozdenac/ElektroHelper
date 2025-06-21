#!/bin/bash

docker-compose down -v

docker rmi elektrohelper-app || true

docker-compose up -d db neo4j elasticsearch mongo_db

echo "Waiting for PostgreSQL to start..."
until docker exec postgres_db pg_isready -U postgres >/dev/null 2>&1; do
  sleep 2
done
echo "PostgreSQL is ready!"

echo "Waiting for Neo4j to start..."
until docker exec neo4j_db cypher-shell -u neo4j -p password "RETURN 1;" >/dev/null 2>&1; do
  sleep 2
done
echo "Neo4j is ready!"

echo "Waiting for MongoDB to start..."
until docker exec mongo_db mongosh -u "root" -p "example" --authenticationDatabase admin --eval "db.adminCommand('ping')" >/dev/null 2>&1; do
  sleep 2
done
echo "MongoDB is ready!"

docker-compose up --build frontend-app app forums_app payment_app

echo "All services are turning off!"
