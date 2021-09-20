# Create image based off of the official Node 16 image
FROM node:16-alpine as builder

# Copy dependency definitions
COPY package.json ./


## installing and Storing node modules on a separate layer will prevent unnecessary npm installs at each build
RUN npm i && mkdir /app && mv ./node_modules ./app

# Change directory so that our commands run inside this new directory
WORKDIR /app

# Get all the code needed to run the app
COPY . /app/

# Build server side bundles
RUN npm run build:ssr

FROM node:16-alpine
## From 'builder' copy published folder
COPY --from=builder /app /app/dist/frontend

WORKDIR /app
# Expose the port the app runs in
EXPOSE 4000

CMD ["node", "server/main.js"]
