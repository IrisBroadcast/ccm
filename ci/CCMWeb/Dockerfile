# Get base image (user is root)
FROM mcr.microsoft.com/dotnet/sdk:3.1 as builder

# Create work directory folder
WORKDIR /source

# Copy all csproj file inorder to fetch all nuget packages
COPY ./CCM.Core/*.csproj ./CCM.Core/
COPY ./CCM.Data/*.csproj ./CCM.Data/
COPY ./CCM.Web/*.csproj ./CCM.Web/

# Get all nuget dependencies
RUN dotnet restore ./CCM.Web/CCM.Web.csproj

# Copy source code
COPY CCM.Core CCM.Core
COPY CCM.Data CCM.Data
COPY CCM.Web CCM.Web

# Build application
RUN dotnet publish --configuration Release --output ./out ./CCM.Web/CCM.Web.csproj

# Switch to runtime image
FROM mcr.microsoft.com/dotnet/aspnet:3.1

# Copy buildfolder to runtime image
WORKDIR /app
COPY --from=builder /source/out .

ENTRYPOINT [ "/app/CCM.Web" ]
