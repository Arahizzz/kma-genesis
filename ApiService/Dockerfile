FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ApiService/ApiService.csproj", "ApiService/"]
RUN dotnet restore "ApiService/ApiService.csproj"
COPY . .
WORKDIR "/src/ApiService"
RUN dotnet build "ApiService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApiService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY wait-for-it.sh wait-for-it.sh
RUN ["chmod", "+x", "wait-for-it.sh"]
COPY --from=publish /app/publish .
ENTRYPOINT ["./wait-for-it.sh", "rabbitmq:5672", "--", "./wait-for-it.sh", "redis:6379", "--", "dotnet", "ApiService.dll"]
