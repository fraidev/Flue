# Build da imagem
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0
WORKDIR /app
COPY out/ .
ENTRYPOINT ["dotnet", "FeedService.API.dll"]