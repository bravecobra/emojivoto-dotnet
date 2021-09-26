#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["EmojiVoteBot/EmojiVoteBot.csproj", "EmojiVoteBot/"]
RUN dotnet restore "EmojiVoteBot/EmojiVoteBot.csproj"
COPY ./EmojiVoteBot ./EmojiVoteBot
COPY ./protos ./protos
WORKDIR "/src/EmojiVoteBot"
RUN dotnet build "EmojiVoteBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EmojiVoteBot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmojiVoteBot.dll"]