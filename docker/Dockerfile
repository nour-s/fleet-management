## a stage container to build the app.
## this helps avoid rebuilding if no code has changed.
FROM mcr.microsoft.com/dotnet/sdk:latest AS sdk
WORKDIR /build

COPY . .

WORKDIR /build
RUN dotnet restore  && dotnet build
RUN dotnet publish "./src/WebApi/WebApi.csproj" -c Release -o /publish --no-restore

## the main container that runs tha app.
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS runtime
WORKDIR /app
EXPOSE 80

ENV Kestrel__Endpoints__Http__Url=http://+:80

COPY --from=sdk /publish .

ENTRYPOINT ["dotnet", "WebApi.dll"]
