#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/MShop.Product.Api/MShop.Product.Api.csproj", "Services/MShop.Product.Api/"]
COPY ["Commons/MShop.Infrastructure/MShop.Infrastructure.csproj", "Commons/MShop.Infrastructure/"]
RUN dotnet restore "Services/MShop.Product.Api/MShop.Product.Api.csproj"
COPY . .
WORKDIR "/src/Services/MShop.Product.Api"
RUN dotnet build "MShop.Product.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MShop.Product.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MShop.Product.Api.dll"]