FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Order/Order.Api/Order.Api.csproj", "src/Order/Order.Api/"]
COPY ["src/Order/Order.Application/Order.Application.csproj", "src/Order/Order.Application/"]
COPY ["src/Order/Order.Core/Order.Core.csproj", "src/Order/Order.Core/"]
COPY ["src/Order/Order.Infrastructure/Order.Infrastructure.csproj", "src/Order/Order.Infrastructure/"]
COPY ["src/Common/EventBusRabbitMQ/EventBusRabbitMQ.csproj", "src/Common/EventBusRabbitMQ/"]

RUN dotnet restore "src/Order/Order.Api/Order.Api.csproj"
COPY . . 
WORKDIR "/src/src/Order/Order.Api"
RUN dotnet build "Order.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Order.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Order.Api.dll"]