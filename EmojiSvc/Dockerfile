#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["EmojiSvc/EmojiSvc.csproj", "EmojiSvc/"]
RUN dotnet restore "EmojiSvc/EmojiSvc.csproj"
COPY ./EmojiSvc ./EmojiSvc
COPY ./protos ./protos
WORKDIR "/src/EmojiSvc"
RUN dotnet build "EmojiSvc.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "EmojiSvc.csproj" -c Debug -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmojiSvc.dll"]