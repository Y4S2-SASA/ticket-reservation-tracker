# Stage 1 - Build React App
FROM node:20-alpine AS build

WORKDIR /app

# Copy package files
COPY package*.json ./

RUN echo "=== Files copied ===" && ls -la && \
    echo "=== Checking for lock file ===" && \
    ls package-lock.json || echo "LOCK FILE MISSING!"

# Install dependencies
RUN npm ci

# Copy source code
COPY . .

# Build the application
RUN npm run build

# Stage 2 - Serve with Nginx
FROM nginx:alpine

# Copy React build files
COPY --from=build /app/build /usr/share/nginx/html

# Expose port
EXPOSE 80

# Start Nginx
CMD ["nginx", "-g", "daemon off;"]