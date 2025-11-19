FROM node:22

# 1. Paquetes del sistema (como root)
RUN apt-get update -y \
    && apt-get upgrade -y \
    && apt-get autoremove --purge -y \
    && apt-get install -y less man-db sudo \
    && rm -rf /var/lib/apt/lists/*

# 2. Instalar npm y Angular CLI GLOBAL (como root)
RUN npm install -g npm@latest @angular/cli

# 3. Configurar sudo para el usuario 'node'
ARG USERNAME=node
RUN echo "$USERNAME ALL=(ALL) NOPASSWD:ALL" > /etc/sudoers.d/$USERNAME \
    && chmod 0440 /etc/sudoers.d/$USERNAME

# 4. A partir de aquí, trabajar como `node`
USER $USERNAME

# 5. Variable para saber que estamos en devcontainer
ENV DEVCONTAINER=true
