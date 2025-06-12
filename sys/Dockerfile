ARG BASE_TAG=lts-alpine
ARG BASE_CLI=20.0.3

FROM node:${BASE_TAG}
RUN npm install -g npm@latest
RUN npm install -g @angular/cli@${BASE_CLI}
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .

# Para desarrollo
RUN npm run build

EXPOSE 4300
CMD ["npm", "start"]
