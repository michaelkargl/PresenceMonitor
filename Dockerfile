FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish --configuration Release --output out

## ---

# Build runtime image
FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /app

COPY --from=build-env /app/out .

# https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-environment-variables
# By disabling the diagnostic pipeline, the docker container runs in read-only mode.
ENV DOTNET_EnableDiagnostics=0
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1

ENTRYPOINT [ "dotnet", "PresenceMonitor.dll" ]
