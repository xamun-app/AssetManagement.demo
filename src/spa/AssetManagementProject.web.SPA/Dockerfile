#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
#RUN curl -sL https://deb.nodesource.com/setup_10.x |  bash -
#RUN apt-get install -y nodejs

ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 80
EXPOSE 443


FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
ENV ASPNETCORE_ENVIRONMENT=Production
WORKDIR /src

RUN curl -sL https://deb.nodesource.com/setup_10.x |  bash -
RUN apt-get install -y nodejs

COPY ["nuget.config", "."]
COPY ["spa/AssetManagementProject.web.SPA/AssetManagementProject.web.SPA.csproj", "AssetManagementProject.web.SPA/"]
RUN dotnet restore "AssetManagementProject.web.SPA/AssetManagementProject.web.SPA.csproj"
COPY ./spa .
COPY ./.npmrc ./ClientApp
WORKDIR "/src/AssetManagementProject.web.SPA"
RUN dotnet build "AssetManagementProject.web.SPA.csproj" -c Release -o /app/build


#RUN apt-get update && \
#    apt-get install -y wget && \
#    apt-get install -y gnupg2 && \
#    wget -qO- https://deb.nodesource.com/setup_10.x | bash - && \
#    apt-get install -y build-essential nodejs
     
FROM build AS publish
RUN dotnet publish "AssetManagementProject.web.SPA.csproj" -c Release -o /app/publish


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY spa/AssetManagementProject.web.SPA/ClientApp/dist ./ClientApp/dist
ENTRYPOINT ["dotnet", "AssetManagementProject.web.SPA.dll"]