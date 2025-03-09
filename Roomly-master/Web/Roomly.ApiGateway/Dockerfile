FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Web/Roomly.ApiGateway/Roomly.ApiGateway.csproj", "Web/Roomly.ApiGateway/"]
RUN dotnet restore "Web/Roomly.ApiGateway/Roomly.ApiGateway.csproj"
COPY . .
WORKDIR "/src/Web/Roomly.ApiGateway"
RUN dotnet build "Roomly.ApiGateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Roomly.ApiGateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Roomly.ApiGateway.dll"]
