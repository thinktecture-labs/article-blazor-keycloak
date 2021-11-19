# article-blazor-keycloak

## Requirements
- [Docker](https://www.docker.com/products/docker-desktop)
- [.NET 6] (https://dotnet.microsoft.com/download/dotnet/6.0)

## Run Project

- First Step: Start Keycloak
  - Change directory to /blazor-keycloak
  - Run Commandline: docker-compose up
- Second Step: Start API and Client
  - Open Project in for example [Visual Studio 2022](https://visualstudio.microsoft.com/de/launch/)
  - Setup multiple Startup Projects
    - Blazor.Keycloak.Api
    - Blazor.Keycloak.Client
  - Start Projects


