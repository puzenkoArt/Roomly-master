﻿FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Web/Roomly.Booking/Roomly.Booking.csproj", "Web/Roomly.Booking/"]
COPY ["Web/Roomly.Shared/Roomly.Shared.csproj", "Web/Roomly.Shared/"]
RUN dotnet restore "Web/Roomly.Booking/Roomly.Booking.csproj"
COPY . .
WORKDIR "/src/Web/Roomly.Booking"
RUN dotnet build "Roomly.Booking.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Roomly.Booking.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Roomly.Booking.dll"]
