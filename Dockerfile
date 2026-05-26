# 1. Stage-ul de build (folosim SDK-ul complet pentru compilare)
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

# Copiem fisierul de proiect si dam restore
COPY ["FitnessTrackerPAW.csproj", "./"]
RUN dotnet restore "FitnessTrackerPAW.csproj"

# Copiem restul codului si compilam (publish)
COPY . .
WORKDIR "/src/"
RUN dotnet publish "FitnessTrackerPAW.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 2. Stage-ul de runtime (mai usor, doar pentru rulare)
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app

# --- FIX OBLIGATORIU PENTRU SQL SERVER PE ALPINE (Conform PDF) ---
RUN apk add --no-cache icu-data-full icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
# ----------------------------------------------------------------

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# 3. Asamblarea finala
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FitnessTrackerPAW.dll"]