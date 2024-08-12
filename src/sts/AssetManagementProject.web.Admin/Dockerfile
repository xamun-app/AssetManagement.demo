#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["sts/AssetManagementProject.web.Admin/AssetManagementProject.web.Admin.csproj", "AssetManagementProject.web.Admin/"]
COPY ["sts/AssetManagementProject.web.Shared/AssetManagementProject.web.Shared.csproj", "AssetManagementProject.web.Shared/"]
COPY ["sts/AssetManagementProject.web.Admin.EntityFramework.MySql/AssetManagementProject.web.Admin.EntityFramework.MySql.csproj", "AssetManagementProject.web.Admin.EntityFramework.MySql/"]
COPY ["sts/AssetManagementProject.web.Admin.EntityFramework.Shared/AssetManagementProject.web.Admin.EntityFramework.Shared.csproj", "AssetManagementProject.web.Admin.EntityFramework.Shared/"]
COPY ["sts/AssetManagementProject.web.Admin.EntityFramework.PostgreSQL/AssetManagementProject.web.Admin.EntityFramework.PostgreSQL.csproj", "AssetManagementProject.web.Admin.EntityFramework.PostgreSQL/"]
COPY ["sts/AssetManagementProject.web.Admin.EntityFramework.SqlServer/AssetManagementProject.web.Admin.EntityFramework.SqlServer.csproj", "AssetManagementProject.web.Admin.EntityFramework.SqlServer/"]
RUN dotnet restore "AssetManagementProject.web.Admin/AssetManagementProject.web.Admin.csproj"
COPY ./sts .
WORKDIR "/src/AssetManagementProject.web.Admin"
RUN dotnet build "AssetManagementProject.web.Admin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AssetManagementProject.web.Admin.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AssetManagementProject.web.Admin.dll"]