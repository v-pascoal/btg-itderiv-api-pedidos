FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["btg-itderiv-api-pedidos.csproj", "./"]
RUN dotnet restore "btg-itderiv-api-pedidos.csproj"

COPY . .
RUN dotnet build "btg-itderiv-api-pedidos.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "btg-itderiv-api-pedidos.csproj" -c Release -o /app/publish /p:UseAppHost=true /p:PublishSingleFile=false

FROM base AS final
RUN apt-get update && apt-get install -y curl
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "btg-itderiv-api-pedidos.dll"]