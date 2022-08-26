FROM mcr.microsoft.com/dotnet/sdk:latest AS sdk
WORKDIR /build
EXPOSE 80

COPY . .

WORKDIR /build

ENV Kestrel__Endpoints__Http__Url=http://+:80

ENTRYPOINT ["dotnet", "run", "--project", "./src/WebApi/WebApi.csproj"]
