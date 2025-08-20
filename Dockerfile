# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy everything and restore
COPY . ./
RUN dotnet restore

# Publish the app
RUN dotnet publish -c Release -o /app

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copy published app from build stage
COPY --from=build /app ./

# Expose port (adjust if your app uses a different one)
EXPOSE 5000

# Set entrypoint
ENTRYPOINT ["dotnet", "emailAPI.dll"]