FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["AuthenticationWebApi/AuthenticationWebApi.csproj", "AuthenticationWebApi/"]
RUN dotnet restore "AuthenticationWebApi/AuthenticationWebApi.csproj"
COPY . .
WORKDIR "/src/AuthenticationWebApi"
RUN dotnet build "AuthenticationWebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuthenticationWebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthenticationWebApi.dll"]
