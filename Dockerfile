#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

# Restore and build each lib
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["src/users-api/users-api.csproj", "src/users-api/"]
COPY ["src/users-logic/users-logic.csproj", "src/users-logic/"]
COPY ["src/users-data/users-data.csproj", "src/users-data/"]
COPY ["src/users-test/users-test.csproj", "src/users-test/"]
RUN dotnet restore "src/users-api/users-api.csproj"
RUN dotnet restore "src/users-logic/users-logic.csproj"
RUN dotnet restore "src/users-data/users-data.csproj"
RUN dotnet restore "src/users-test/users-test.csproj"
COPY . .
WORKDIR "/src/src/users-api"
RUN dotnet build "users-api.csproj" -c Release -o /app/build
WORKDIR "/src/src/users-logic"
RUN dotnet build "users-logic.csproj" -c Release -o /app/build
WORKDIR "/src/src/users-data"
RUN dotnet build "users-data.csproj" -c Release -o /app/build
WORKDIR "/src/src/users-test/"
RUN dotnet build "users-test.csproj" -c Release -o /app/build
RUN ls
RUN dotnet test "users-test.csproj"

FROM build AS publish
WORKDIR "/src/src/users-api"
RUN dotnet publish "users-api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "users-api.dll"]
