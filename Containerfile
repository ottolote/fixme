FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore FixMe.slnx
RUN dotnet publish Host/Api/FixMe.Api.csproj --configuration Release --no-restore --output /app/publish

FROM mcr.microsoft.com/dotnet/runtime:10.0 AS runtime
WORKDIR /app

ENV DOTNET_ENVIRONMENT=Production \
    FIXME_API_URL=http://*:8080

EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FixMe.Api.dll"]
