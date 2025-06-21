# Use .NET 8.0 SDK as base image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy project files and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the app
COPY . ./
RUN dotnet publish -c Release -o out

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy the published app
COPY --from=build /app/out ./

# Set environment variables
ENV ASPNETCORE_URLS=http://+:9091

# Expose port
EXPOSE 9091

# Run the app
CMD ["dotnet", "payment_backend.dll"]
