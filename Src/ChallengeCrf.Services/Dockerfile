#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ChallengeB3.Services/ChallengeB3.Services.csproj", "ChallengeB3.Services/"]
RUN dotnet restore "ChallengeB3.Services/ChallengeB3.Services.csproj"
COPY . .
WORKDIR "/src/ChallengeB3.Services"
RUN dotnet build "ChallengeB3.Services.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChallengeB3.Services.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChallengeB3.Services.dll"]