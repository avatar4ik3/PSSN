FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /PSSN.Api
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY PSSN.Api/PSSN.Api.csproj PSSN.Api/
RUN dotnet restore PSSN.Api/PSSN.Api.csproj
COPY . .
WORKDIR /src/PSSN.Api
RUN dotnet build PSSN.Api.csproj -c Release -o /app

COPY PSSN.Api/Properties/launchSettings.json /app
COPY PSSN.Api/appsettings.json /app

FROM build AS publish
RUN dotnet publish PSSN.Api.csproj -c Release -o /app



FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PSSN.Api.dll"]