# Folosim imaginea oficiala de .NET 8 SDK pentru a compila proiectul
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiem fisierul de proiect si restauram dependintele
COPY ["FitnessTrackerPAW.csproj", "./"]
RUN dotnet restore "FitnessTrackerPAW.csproj"

# Copiem restul codului si compilam aplicatia
COPY . .
WORKDIR "/src/"
RUN dotnet build "FitnessTrackerPAW.csproj" -c Release -o /app/build
RUN dotnet publish "FitnessTrackerPAW.csproj" -c Release -o /app/publish

# Construim imaginea finala ultra-usoara de ASP.NET
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Setam portul default 8080 pentru .NET 8
EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080

ENTRYPOINT ["dotnet", "FitnessTrackerPAW.dll"]