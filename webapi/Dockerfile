#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["webapi/host.csproj", "webapi/"]
COPY ["shared/shared.csproj", "shared/"]
RUN dotnet restore "webapi/host.csproj"
COPY . .
WORKDIR "/src/webapi"
RUN dotnet build "host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "host.csproj" -c Release -o /app/publish
RUN ls /app/publish 

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "host.dll"]