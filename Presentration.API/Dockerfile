FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Presentration.API/Presentration.API.csproj", "Presentration.API/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Apllication/Apllication.csproj", "Apllication/"]
COPY ["Domain/Domain.csproj", "Domain/"]
RUN dotnet restore "Presentration.API/Presentration.API.csproj"
COPY . .
WORKDIR "/src/Presentration.API"
RUN dotnet build "Presentration.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Presentration.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Presentration.API.dll"]