#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/poc_spot_instance_service_api/poc_spot_service_api/poc_spot_service_api.csproj", "src/poc_spot_instance_service_api/poc_spot_service_api/"]
RUN dotnet restore "src/poc_spot_instance_service_api/poc_spot_service_api/poc_spot_service_api.csproj"
COPY . .
WORKDIR "src/poc_spot_instance_service_api/poc_spot_service_api/"
RUN dotnet build "poc_spot_service_api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "poc_spot_service_api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "poc_spot_service_api.dll"]