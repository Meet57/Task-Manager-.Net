# Use the official .NET 6 SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY TaskManagerAPI.csproj ./
RUN dotnet restore

# Copy the rest of the source code
COPY . ./

# Build the app
RUN dotnet publish -c Release -o out

# Final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0

WORKDIR /app
COPY --from=build /app/out ./

EXPOSE 80
ENTRYPOINT ["dotnet", "TaskManagerAPI.dll"]