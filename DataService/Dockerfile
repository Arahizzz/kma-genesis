FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DataService/DataService.csproj", "DataService/"]
RUN dotnet restore "DataService/DataService.csproj"
COPY . .
WORKDIR "/src/DataService"
RUN dotnet build "DataService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DataService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY wait-for-it.sh wait-for-it.sh
RUN ["chmod", "+x", "wait-for-it.sh"]
COPY --from=publish /app/publish .
ENTRYPOINT ["./wait-for-it.sh", "rabbitmq:5672", "--", "./wait-for-it.sh", "postgres:5432", "--", "dotnet", "DataService.dll"]
