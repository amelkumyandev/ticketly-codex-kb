FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY Ticketly.slnx ./
COPY src/Ticketly.Api/Ticketly.Api.csproj src/Ticketly.Api/
COPY src/Ticketly.Application/Ticketly.Application.csproj src/Ticketly.Application/
COPY src/Ticketly.Domain/Ticketly.Domain.csproj src/Ticketly.Domain/
COPY src/Ticketly.Infrastructure/Ticketly.Infrastructure.csproj src/Ticketly.Infrastructure/
COPY tests/Ticketly.Tests/Ticketly.Tests.csproj tests/Ticketly.Tests/
RUN dotnet restore Ticketly.slnx

COPY . .
RUN dotnet publish src/Ticketly.Api/Ticketly.Api.csproj --configuration Release --output /app/publish --no-restore

FROM runtime AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "Ticketly.Api.dll"]
