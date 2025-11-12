# 1 Build stage 
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# copy csproj and restore first (leverages cache)
COPY ["ContactsApi/ContactsApi.csproj", "ContactsApi/"]

RUN dotnet restore "ContactsApi/ContactsApi.csproj"

# copy everything and publish
COPY ContactsApi/ ContactsApi/

WORKDIR /src/ContactsApi

RUN dotnet publish -c Release -o /app/publish /p:GenerateRuntimeConfigurationFiles=true

# 2 Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS runtime
WORKDIR /app

# Ensure CA certs are installed (some alpine images omit them)
RUN apk add --no-cache ca-certificates && update-ca-certificates

# Copy published files
COPY --from=build /app/publish ./

# Use non-root user for safety
ENV ASPNETCORE_URLS=http://+:5000

ENV DOTNET_RUNNING_IN_CONTAINER=true

EXPOSE 5000

ENTRYPOINT ["dotnet", "ContactsApi.dll"]
