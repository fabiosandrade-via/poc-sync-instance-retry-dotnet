#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/poc_consumer_spot_instance_dlq/poc_consumer_spot_instance_dlq/poc_consumer_spot_instance_dlq.csproj", "src/poc_consumer_spot_instance_dlq/poc_consumer_spot_instance_dlq/"]
RUN dotnet restore "src/poc_consumer_spot_instance_dlq/poc_consumer_spot_instance_dlq/poc_consumer_spot_instance_dlq.csproj"
COPY . .
WORKDIR "src/poc_consumer_spot_instance_dlq/poc_consumer_spot_instance_dlq/"
RUN dotnet build "poc_consumer_spot_instance_dlq.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "poc_consumer_spot_instance_dlq.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "poc_consumer_spot_instance_dlq.dll"]