#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/EmojiShared/EmojiShared.csproj", "EmojiShared/"]
COPY ["src/EmojiUI/EmojiUI.csproj", "EmojiUI/"]
RUN dotnet restore "EmojiUI/EmojiUI.csproj"
COPY ./src/EmojiShared ./EmojiShared
COPY ./src/EmojiUI ./EmojiUI
COPY ./src/protos ./protos
WORKDIR "/src/EmojiUI"
RUN dotnet build "EmojiUI.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "EmojiUI.csproj" -c Debug -o /app/publish

FROM base AS final
RUN apt-get update && apt-get install -y procps
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmojiUI.dll"]