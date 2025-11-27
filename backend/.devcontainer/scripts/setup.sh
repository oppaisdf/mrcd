#!/usr/bin/env bash
set -eu

# Activa pipefail solo si el shell lo soporta
if (set -o pipefail 2>/dev/null); then
  set -o pipefail
fi


if command -v sudo >/dev/null 2>&1; then
  sudo apt-get update -y && sudo apt-get upgrade -y && sudo apt-get autoremove --purge -y
else
  apt-get update -y && apt-get upgrade -y && apt-get autoremove --purge -y
fi

if command -v pwsh >/dev/null 2>&1; then
  pwsh -NoLogo -NoProfile -File .devcontainer/scripts/setup.ps1
fi

echo "🔥 Contenedor listo, papu 🔥"
