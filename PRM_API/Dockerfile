# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
USER app
WORKDIR /app
EXPOSE 80

# SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy all source code (Dockerfile nằm cùng thư mục => không cần đi vòng)
COPY . .
RUN dotnet restore "./PRM_API.csproj"
RUN dotnet publish "./PRM_API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PRM_API.dll"]
