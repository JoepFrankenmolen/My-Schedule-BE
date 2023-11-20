# build: 
# dir: parent
# cmd: docker build -t my-schedule-apigateway:1.0 -f My-Schedule.APIGateway/Dockerfile .
# run: docker run -p 8080:80 --network my-schedule-network --name my-schedule-api-container my-schedule-apigateway:1.4

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY /My-Schedule.APIGateway .

RUN mv /src/Ocelot-docker.json /src/Ocelot.json # Rename Ocelot-docker.json to Ocelot.json
RUN dotnet restore "My-Schedule.APIGateway.csproj"
RUN dotnet build "My-Schedule.APIGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "My-Schedule.APIGateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
COPY --from=publish /app/publish .
COPY --from=build /src/Ocelot.json /app/Ocelot.json
ENTRYPOINT ["dotnet", "My-Schedule.APIGateway.dll"]

