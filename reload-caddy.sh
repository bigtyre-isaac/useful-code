!/bin/bash

# Define the Caddy service name
SERVICE_NAME="caddy_server"

# Find the running Caddy container name based on the service name
CADDY_CONTAINER=$(docker ps --filter "name=${SERVICE_NAME}" --format "{{.Names}}" | head -n 1)

# Check if a Caddy container was found
if [ -z "$CADDY_CONTAINER" ]; then
    echo "No running Caddy container found for service name: ${SERVICE_NAME}"
    exit 1
fi

echo "Found Caddy container: ${CADDY_CONTAINER}"

# Run validate and reload commands inside the container
docker exec "$CADDY_CONTAINER" sh -c "cd /etc/caddy && caddy validate && caddy reload"

# Check the status of the last command
if [ $? -eq 0 ]; then
    echo "Caddy configuration validated and reloaded successfully."
else
    echo "Error: Validation or reload failed."
    exit 1
fi


