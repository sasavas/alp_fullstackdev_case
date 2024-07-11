# Utilizăm imaginea oficială .NET 6 SDK pentru a construi aplicația noastră
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Setăm directorul de lucru la folderul radăcină al aplicației
WORKDIR /app

# Copiem fișierele de proiect și csproj în directorul curent
COPY src/ForceGetCase.API/*.csproj ./src/ForceGetCase.API/
COPY src/ForceGetCase.Application/*.csproj ./src/ForceGetCase.Application/
COPY src/ForceGetCase.Core/*.csproj ./src/ForceGetCase.Core/
COPY src/ForceGetCase.DataAccess/*.csproj ./src/ForceGetCase.DataAccess/
COPY src/ForceGetCase.Shared/*.csproj ./src/ForceGetCase.Shared/

# Restaurăm pachetele NuGet în directorul corespunzător
WORKDIR /app/src/ForceGetCase.API
RUN dotnet restore

# Copiem întregul conținut al soluției în container
WORKDIR /app
COPY . .

# Setăm directorul de lucru la proiectul API
WORKDIR /app/src/ForceGetCase.API

# Construim aplicația
RUN dotnet build -c Release -o /app/build

# Stage final pentru a crea imaginea de producție
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Creăm imaginea finală, folosind imaginea oficială .NET 6 pentru aplicații ASP.NET
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ForceGetCase.API.dll"]
