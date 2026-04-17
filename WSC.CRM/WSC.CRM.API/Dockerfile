# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WSC.CRM/WSC.CRM.API/WSC.CRM.API.csproj", "WSC.CRM/WSC.CRM.API/"]
COPY ["WSC.Shared/WSC.Shared.Infrastructure/WSC.Shared.Infrastructure.csproj", "WSC.Shared/WSC.Shared.Infrastructure/"]
COPY ["WSC.Shared/WSC.Shared.Contracts/WSC.Shared.Contracts.csproj", "WSC.Shared/WSC.Shared.Contracts/"]
COPY ["WSC.Delivery/WSC.Delivery.Domain/WSC.Delivery.Domain.csproj", "WSC.Delivery/WSC.Delivery.Domain/"]
COPY ["WSC.CRM/WSC.CRM.Application/WSC.CRM.Application.csproj", "WSC.CRM/WSC.CRM.Application/"]
COPY ["WSC.CRM/WSC.CRM.Domain/WSC.CRM.Domain.csproj", "WSC.CRM/WSC.CRM.Domain/"]
COPY ["WSC.CRM/WSC.CRM.Infrastructure/WSC.CRM.Infrastructure.csproj", "WSC.CRM/WSC.CRM.Infrastructure/"]
RUN dotnet restore "./WSC.CRM/WSC.CRM.API/WSC.CRM.API.csproj"
COPY . .
WORKDIR "/src/WSC.CRM/WSC.CRM.API"
RUN dotnet build "./WSC.CRM.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WSC.CRM.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WSC.CRM.API.dll"]