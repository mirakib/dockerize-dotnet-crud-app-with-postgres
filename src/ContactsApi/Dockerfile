# 1. Build stage 
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# copy csproj and restore first (leverages cache)
COPY ["ContactsApi.csproj", "./"]
RUN dotnet restore "ContactsApi.csproj"

# copy everything and publish
COPY . ./
RUN dotnet publish -c Release -o /app/publish /p:GenerateRuntimeConfigurationFiles=true

# 2. Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS runtime
WORKDIR /app

RUN apk add --no-cache ca-certificates && update-ca-certificates

COPY --from=build /app/publish ./

ENV ASPNETCORE_URLS=http://+:5000
ENV DOTNET_RUNNING_IN_CONTAINER=true
EXPOSE 5000

ENTRYPOINT ["dotnet", "ContactsApi.dll"]
