# Utiliser une version stable de Node.js pour la build
FROM --platform=$BUILDPLATFORM node:18-bullseye-slim AS build

WORKDIR /app

# Copier les fichiers package.json et package-lock.json et installer les dépendances
COPY package*.json ./

# RUN npm ci \
#     && npm install -g @angular/cli

# Installer Angular CLI globalement et les dépendances du projet
RUN npm install -g @angular/cli@18 \
    && npm ci
    
# Copier le reste du projet
COPY . .

# Construire l'application Angular en mode production
RUN npm run build --configuration=production

# Utiliser Nginx pour servir l'application Angular
FROM nginx:1.27.1-alpine

# Copier la configuration Nginx personnalisée
COPY ./nginx.conf /etc/nginx/conf.d/default.conf

# Copier les fichiers construits dans le dossier Nginx
COPY --from=build /app/dist/hexa-front/browser /usr/share/nginx/html

EXPOSE 4200