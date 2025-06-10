ARG BASE_TAG=lts-alpine
ARG BASE_CLI=20.0.1

FROM node:${BASE_TAG}
RUN npm install -g @angular/cli@${BASE_CLI}
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .

# Para desarrollo
RUN npm run build

EXPOSE 4300
CMD ["npm", "start"]
