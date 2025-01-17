FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5005 5006  # Expose both HTTP and HTTPS ports1


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["IConduct/IConduct.csproj", "IConduct/"]
RUN dotnet restore "./IConduct/IConduct.csproj"
COPY . .
WORKDIR "/src/IConduct"
RUN dotnet build "./IConduct.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./IConduct.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IConduct.dll"]