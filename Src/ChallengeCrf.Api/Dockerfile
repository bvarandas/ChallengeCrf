#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5200

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Src/ChallengeCrf.Api/ChallengeCrf.Api.csproj", "Src/ChallengeCrf.Api/"]
COPY ["Src/ChallengeCrf.Application/ChallengeCrf.Application.csproj", "Src/ChallengeCrf.Application/"]
COPY ["Src/ChallengeCrf.Domain/ChallengeCrf.Domain.csproj", "Src/ChallengeCrf.Domain/"]
COPY ["Src/ChallengeCrf.Infra.Data/ChallengeCrf.Infra.Data.csproj", "Src/ChallengeCrf.Infra.Data/"]
COPY ["Src/ChallengeCrf.Infra.CrossCutting.Ioc/ChallengeCrf.Infra.CrossCutting.Ioc.csproj", "Src/ChallengeCrf.Infra.CrossCutting.Ioc/"]
COPY ["Src/ChallengeCrf.Infra.CrossCutting.Bus/ChallengeCrf.Infra.CrossCutting.Bus.csproj", "Src/ChallengeCrf.Infra.CrossCutting.Bus/"]
RUN dotnet restore "Src/ChallengeCrf.Api/ChallengeCrf.Api.csproj"
COPY . .
WORKDIR "/src/Src/ChallengeCrf.Api"
RUN dotnet build "ChallengeCrf.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChallengeCrf.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChallengeCrf.Api.dll"]