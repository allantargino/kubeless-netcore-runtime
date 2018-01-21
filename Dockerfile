# Image usage only for build process.
FROM microsoft/aspnetcore-build:2.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers.
COPY ./src ./
RUN dotnet restore ./Kubeless.WebAPI/

# Copy everything else and build in release mode.
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image.
FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY --from=build-env /app/src/Kubeless.WebAPI/out ./

# TODO: Calling dotnet restore is not possible, because the image does not contains SDK functions.
# Restore the functions dependencies.
#WORKDIR /kubeless
#RUN dotnet restore --packages packages

# Run the web api application.
WORKDIR /app
#ENTRYPOINT ["dotnet", "Kubeless.WebAPI.dll"]