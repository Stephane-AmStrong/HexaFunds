# syntax=docker/dockerfile:1

# Etape de build
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copie des fichiers de projet et restauration des dépendances avec utilisation de cache
COPY ["Directory.Packages.props", "."]
COPY ["src/Presentation/HexaFunds.WebApi/HexaFunds.WebApi.csproj", "Presentation/HexaFunds.WebApi/"]
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages dotnet restore "Presentation/HexaFunds.WebApi/HexaFunds.WebApi.csproj"

# Copie du reste des fichiers et compilation
COPY src/ .
WORKDIR "/src/Presentation/HexaFunds.WebApi"
RUN dotnet build "HexaFunds.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Etape de publication
FROM build AS publish
RUN dotnet publish "HexaFunds.WebApi.csproj" -c $BUILD_CONFIGURATION --use-current-runtime --self-contained false -o /app/publish

# Etape finale avec l'image ASP.NET
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS final

# Installer curl pour les healthchecks
RUN apk add --no-cache curl

WORKDIR /app

# Sécurité : définir un utilisateur non-root
ARG APP_UID=1001
USER $APP_UID

# Copie des fichiers publiés dans l'image finale
COPY --chown=$APP_UID:$APP_UID --from=publish /app/publish .

# Copie du certificat PFX avec les bonnes permissions
COPY --chown=$APP_UID:$APP_UID /src/Presentation/HexaFunds.WebApi/https/aspnetapp.pfx /app/https/aspnetapp.pfx

# Exposition des ports
EXPOSE 80
EXPOSE 443

# Commande d'entrée
ENTRYPOINT ["dotnet", "HexaFunds.WebApi.dll"]
