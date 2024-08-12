#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["sts/AssetManagementProject.web.STS.Identity/AssetManagementProject.web.STS.Identity.csproj", "AssetManagementProject.web.STS.Identity/"]
COPY ["sts/AssetManagementProject.web.Shared/AssetManagementProject.web.Shared.csproj", "AssetManagementProject.web.Shared/"]
COPY ["sts/AssetManagementProject.web.Admin.EntityFramework.Shared/AssetManagementProject.web.Admin.EntityFramework.Shared.csproj", "AssetManagementProject.web.Admin.EntityFramework.Shared/"]
RUN dotnet restore "AssetManagementProject.web.STS.Identity/AssetManagementProject.web.STS.Identity.csproj"
COPY ./sts .
WORKDIR "/src/AssetManagementProject.web.STS.Identity"
RUN dotnet build "AssetManagementProject.web.STS.Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AssetManagementProject.web.STS.Identity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AssetManagementProject.web.STS.Identity.dll"]