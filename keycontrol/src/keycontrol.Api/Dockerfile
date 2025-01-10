FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/keycontrol.Api/keycontrol.Api.csproj", "src/keycontrol.Api/"]
COPY ["src/keycontrol.Application/keycontrol.Application.csproj", "src/keycontrol.Application/"]
COPY ["src/keycontrol.Domain/keycontrol.Domain.csproj", "src/keycontrol.Domain/"]
COPY ["src/keycontrol.Infrastructure/keycontrol.Infrastructure.csproj", "src/keycontrol.Infrastructure/"]
COPY ["src/keycontrol.Tests/keycontrol.Tests.csproj", "src/keycontrol.Tests/"]
RUN dotnet restore "src/keycontrol.Api/keycontrol.Api.csproj"
COPY . .
WORKDIR "/src/src/keycontrol.Api"
RUN dotnet build "keycontrol.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "keycontrol.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "keycontrol.Api.dll"]
