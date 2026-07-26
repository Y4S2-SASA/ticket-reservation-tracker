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

# Server URL is baked in at build time by CRA; pass it via --build-arg
ARG REACT_APP_SERVER_URL
ENV REACT_APP_SERVER_URL=$REACT_APP_SERVER_URL

# Build the application
RUN npm run build

# Stage 2 - Serve with Nginx
FROM nginx:alpine

# Copy React build files
COPY --from=build /app/build /usr/share/nginx/html

# Prevent browsers from caching index.html (so REACT_APP_SERVER_URL changes take effect immediately on rebuild)
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Expose port
EXPOSE 80

# Start Nginx
CMD ["nginx", "-g", "daemon off;"]