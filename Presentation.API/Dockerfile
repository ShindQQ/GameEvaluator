FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Presentation.API/Presentation.API.csproj", "Presentation.API/"]
RUN dotnet restore "Presentation.API/Presentation.API.csproj"
COPY . .
WORKDIR "/src/Presentation.API"
RUN dotnet build "Presentation.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Presentation.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM node:18.7.0 AS node-builder
WORKDIR /node
COPY ./GameEvaluator.Web /node
RUN npm install
RUN npm run build

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=node-builder /node/build ./wwwroot
ENTRYPOINT ["dotnet", "Presentation.API.dll"]