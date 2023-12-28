# build: 
# dir: parent
# cmd: docker build -t my-schedule-notificationservice:1.0 -f My-Schedule.NotificationService/Dockerfile .
# run: docker run -p 8081:80 --network my-schedule-network --name my-schedule-notificationservice -e MessageQueueHostName="rabbitmq" my-schedule-notificationservice:1.0

# network: docker network create my-schedule-network
# rabbitmq: docker run -it --network my-schedule-network --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.12-management

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy the projects folder into the image
COPY /My-Schedule.Shared ./My-Schedule.Shared
COPY /My-Schedule.NotificationService ./My-Schedule.NotificationService

WORKDIR /src/My-Schedule.NotificationService
RUN dotnet restore "My-Schedule.NotificationService.csproj"
RUN dotnet build "My-Schedule.NotificationService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "My-Schedule.NotificationService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "My-Schedule.NotificationService.dll"]
