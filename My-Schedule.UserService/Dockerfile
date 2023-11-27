# build: 
# dir: parent
# cmd: docker build -t my-schedule-userservice:1.0 -f My-Schedule.userservice/Dockerfile .
# run: docker run -p 8082:80 --network my-schedule-network --name my-schedule-userservice -e MessageQueueHostName="rabbitmq" my-schedule-userservice:1.0

# network: docker network create my-schedule-network
# rabbitmq: docker run -it --network my-schedule-network --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.12-management

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 8082

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy the projects folder into the image
COPY /My-Schedule.Shared ./My-Schedule.Shared
COPY /My-Schedule.UserService ./My-Schedule.UserService

WORKDIR /src/My-Schedule.UserService
RUN dotnet restore "My-Schedule.UserService.csproj"
RUN dotnet build "My-Schedule.UserService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "My-Schedule.UserService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "My-Schedule.UserService.dll"]
