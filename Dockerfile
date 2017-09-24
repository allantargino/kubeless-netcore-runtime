# Image usage only for build process
FROM microsoft/aspnetcore-build:2.0 AS build-env
WORKDIR /app

# Copy all library and Web API projects, restoring all dependencies
COPY ./Kubeless.Core/*.csproj ./Kubeless.Core/
COPY ./Kubeless.WebAPI/*.csproj ./Kubeless.WebAPI/
RUN dotnet restore ./Kubeless.WebAPI

# Copy everything else and build in release mode
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Kubeless.WebAPI.dll"]