#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["api/AssetManagementProject.web.Api/AssetManagementProject.web.Api.csproj", "AssetManagementProject.web.Api/"]
COPY ["api/AssetManagementProject.web.Domain/AssetManagementProject.web.Domain.csproj", "AssetManagementProject.web.Domain/"]
COPY ["api/AssetManagementProject.web.Entity/AssetManagementProject.web.Entity.csproj", "AssetManagementProject.web.Entity/"]
RUN dotnet restore "AssetManagementProject.web.Api/AssetManagementProject.web.Api.csproj"
COPY ./api .
WORKDIR "/src/AssetManagementProject.web.Api"
RUN dotnet build "AssetManagementProject.web.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AssetManagementProject.web.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AssetManagementProject.web.Api.dll"]