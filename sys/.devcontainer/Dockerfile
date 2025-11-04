FROM node:22

# Install basic development tools
RUn apt-get update -y && apt-get upgrade -y && apt-get autoremove --purge -y
RUN apt install -y less man-db sudo
RUN npm install -g npm@latest && npm install -g @angular/cli

# Ensure default `node` user has access to `sudo`
ARG USERNAME=node
RUN echo $USERNAME ALL=\(root\) NOPASSWD:ALL > /etc/sudoers.d/$USERNAME \
    && chmod 0440 /etc/sudoers.d/$USERNAME

# Set `DEVCONTAINER` environment variable to help with orientation
ENV DEVCONTAINER=true
