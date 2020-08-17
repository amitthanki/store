#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["./BackendGateway/BackendGateway.csproj", "./BackendGateway/"]
COPY ["./Middleware/Middleware.csproj", "./Middleware/"]
RUN dotnet restore "Middleware/Middleware.csproj"
RUN dotnet restore "BackendGateway/BackendGateway.csproj"
COPY ["./Middleware/", "Middleware/"]
COPY ["./BackendGateway/", "BackendGateway/"]
WORKDIR "/src/BackendGateway"
RUN dotnet build "BackendGateway.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "BackendGateway.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BackendGateway.dll"]
