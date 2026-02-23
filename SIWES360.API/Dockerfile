FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5229

ENV ASPNETCORE_URLS=http://+:5229

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["SIWES360.API/SIWES360.API.csproj", "SIWES360.API/"]
RUN dotnet restore "SIWES360.API/SIWES360.API.csproj"
COPY . .
WORKDIR "/src/SIWES360.API"
RUN dotnet build "SIWES360.API.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "SIWES360.API.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SIWES360.API.dll"]
