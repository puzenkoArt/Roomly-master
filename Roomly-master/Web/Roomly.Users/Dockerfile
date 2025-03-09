FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Web/Roomly.Users/Roomly.Users.csproj", "Web/Roomly.Users/"]
COPY ["Web/Roomly.Shared/Roomly.Shared.csproj", "Web/Roomly.Shared/"]
COPY ["Web/Roomly.Rooms/Roomly.Rooms.csproj", "Web/Roomly.Rooms/"]
RUN dotnet restore "Web/Roomly.Users/Roomly.Users.csproj"
COPY . .
WORKDIR "/src/Web/Roomly.Users"
RUN dotnet build "Roomly.Users.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Roomly.Users.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Roomly.Users.dll"]
