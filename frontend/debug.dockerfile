# Create image based off of the official Node 16 image
FROM node:16.9.1-alpine

# Copy dependency definitions
COPY package.json ./

## installing and Storing node modules on a separate layer will prevent unnecessary npm installs at each build
RUN npm i --force && mkdir /app && mv ./node_modules ./app

RUN npm i -g @angular/cli
# Change directory so that our commands run inside this new directory
WORKDIR /app

# Get all the code needed to run the app
COPY . /app/

# Expose the port the app runs in
EXPOSE 4200

CMD [ "npm", "run", "start:proxy" ]
