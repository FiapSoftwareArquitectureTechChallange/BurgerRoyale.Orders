#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ENV TZ=America/Sao_Paulo
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/BurgerRoyale.Orders.API/BurgerRoyale.Orders.API.csproj", "src/BurgerRoyale.Orders.API/"]
COPY ["src/BurgerRoyale.Orders.Application/BurgerRoyale.Orders.Application.csproj", "src/BurgerRoyale.Orders.Application/"]
COPY ["src/BurgerRoyale.Orders.Domain/BurgerRoyale.Orders.Domain.csproj", "src/BurgerRoyale.Orders.Domain/"]
COPY ["src/BurgerRoyale.Orders.Infrastructure/BurgerRoyale.Orders.Infrastructure.csproj", "src/BurgerRoyale.Orders.Infrastructure/"]
COPY ["src/BurgerRoyale.Orders.IOC/BurgerRoyale.Orders.IOC.csproj", "src/BurgerRoyale.Orders.IOC/"]
RUN dotnet restore "src/BurgerRoyale.Orders.API/BurgerRoyale.Orders.API.csproj"
COPY . .
WORKDIR "/src/src/BurgerRoyale.Orders.API"
RUN dotnet build "BurgerRoyale.Orders.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BurgerRoyale.Orders.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BurgerRoyale.Orders.API.dll"]