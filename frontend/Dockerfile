# Use an official Node.js image
FROM node:18-alpine

# Set working directory
WORKDIR /app

# Copy package.json and package-lock.json
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the application source code
COPY . .

# Expose the port for the development server
EXPOSE 5173

# Start the development server with watch mode enabled
CMD ["npm", "run", "dev", "--", "--host"]
