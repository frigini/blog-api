FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY src/blog-api/BlogApi.csproj ./src/blog-api/
WORKDIR /app/src/blog-api
RUN dotnet restore

COPY src/blog-api/ ./
RUN dotnet publish -c Release -o /app/out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "BlogApi.dll"]