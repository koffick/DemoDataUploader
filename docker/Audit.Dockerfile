
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/Audit/DataUploader.Audit/DataUploader.Audit.csproj", "Audit/DataUploader.Audit/"]
RUN dotnet restore "./Audit/DataUploader.Audit/DataUploader.Audit.csproj"
COPY . .
WORKDIR "src/Audit/DataUploader.Audit"
RUN dotnet build "./DataUploader.Audit.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DataUploader.Audit.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DataUploader.Audit.dll"]
