# Get base image
FROM mcr.microsoft.com/dotnet/sdk:3.1 as builder

# Create work directory folder
WORKDIR /source

# Copy all csproj file inorder to fetch all nuget packages
COPY ./CCM.Core/*.csproj ./CCM.Core/
COPY ./CCM.DiscoveryApi/*.csproj ./CCM.DiscoveryApi/

# Get all nuget dependencies
RUN dotnet restore ./CCM.DiscoveryApi/CCM.DiscoveryApi.csproj

# Copy source code
COPY CCM.Core CCM.Core
COPY CCM.DiscoveryApi CCM.DiscoveryApi

# Build application
RUN dotnet publish --configuration Release --output ./out ./CCM.DiscoveryApi/CCM.DiscoveryApi.csproj

# Switch to runtime image
FROM mcr.microsoft.com/dotnet/aspnet:3.1
# RUN apt-get update
# RUN apt-get install tcpdump -y
# RUN apt-get install net-tools -y
# RUN apt-get install iputils-ping -y
# RUN apt-get install nano -y

# Copy buildfolder to runtime image
WORKDIR /app
COPY --from=builder /source/out .

ENTRYPOINT [ "/app/CCM.DiscoveryApi" ]
