# Build stage using the corporate devcontainer image
FROM docker-registry-002.zeuslearning.com/zeuslearning/vscode/devcontainers/dotnet AS build
WORKDIR /src

# 1. Copy the entire repository first
COPY . .

# 2. Restore ONLY the targeted API project explicitly
# This generates the assets file directly in TraineeManagement.api/obj/
RUN --mount=type=secret,id=aws_token \
    export CODEARTIFACT_TOKEN=$(cat /run/secrets/aws_token) && \
    dotnet restore TraineeManagementGateway.api.csproj --configfile NuGet.config

# 3. Publish ONLY the API project file explicitly
RUN dotnet publish TraineeManagementGateway.api.csproj \
    -c Release \
    -o /App/out \
    --no-restore

# Build runtime image
FROM docker-registry-002.zeuslearning.com/zeuslearning/vscode/devcontainers/dotnet
WORKDIR /App
COPY --from=build /App/out .
ENTRYPOINT ["dotnet", "TraineeManagementGateway.api.dll"]