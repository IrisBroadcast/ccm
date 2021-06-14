# Get base image
FROM mcr.microsoft.com/dotnet/sdk:3.1 as builder

# Create work directory folder
WORKDIR /source

# Copy all csproj file inorder to fetch all nuget packages
COPY ./CCMCore/*.csproj ./CCMCore/
COPY ./CCMDiscoveryApi/*.csproj ./CCMDiscoveryApi/

# Get all nuget dependencies
RUN dotnet restore ./CCMDiscoveryApi/CCM.DiscoveryApi.csproj

# Copy source code
COPY CCMCore CCMCore
COPY CCMDiscoveryApi CCMDiscoveryApi

# Build application
RUN dotnet publish --configuration Release --output ./out ./CCMDiscoveryApi/CCM.DiscoveryApi.csproj

# Switch to runtime image
FROM mcr.microsoft.com/dotnet/aspnet:3.1

# Copy buildfolder to runtime image
WORKDIR /app
COPY --from=builder /source/out .

ENTRYPOINT [ "/app/CCM.DiscoveryApi" ]
