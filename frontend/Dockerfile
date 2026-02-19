ARG BASE_TAG=lts-alpine

# Construcciones
FROM node:${BASE_TAG} AS builder

# Sistema
WORKDIR /system
COPY package*.json ./
RUN npm install
COPY ./ ./
RUN npx ng build --configuration production

# Configuración del servidor
FROM nginx:alpine as server
RUN apk update && apk upgrade
RUN mkdir -p /sites/sys
COPY default.conf /etc/nginx/conf.d/default.conf

# Script de inicio
RUN \
	echo "#!/bin/sh" > /usr/local/bin/entrypoint.sh && \
	echo 'sed -i "s|_domain_|${DOMAIN}|g" /etc/nginx/conf.d/default.conf' >> /usr/local/bin/entrypoint.sh && \
	echo 'sed -i "s|_apiuri_|${API_URI}|g" /etc/nginx/conf.d/default.conf' >> /usr/local/bin/entrypoint.sh && \
	echo 'sed -i "s|_apihost_|${API_HOST}|g" /etc/nginx/conf.d/default.conf' >> /usr/local/bin/entrypoint.sh && \
	echo "nginx -g 'daemon off;'" >> /usr/local/bin/entrypoint.sh && \
	chmod +x /usr/local/bin/entrypoint.sh

# Configuración de sistema en servidor
COPY --from=builder /system/dist/app /sites/sys

EXPOSE 80
ENTRYPOINT ["/usr/local/bin/entrypoint.sh"]