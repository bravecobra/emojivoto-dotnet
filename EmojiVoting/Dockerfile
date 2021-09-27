#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["EmojiVoting/EmojiVoting.csproj", "EmojiVoting/"]
RUN dotnet restore "EmojiVoting/EmojiVoting.csproj"
COPY ./EmojiVoting ./EmojiVoting
COPY ./protos ./protos
WORKDIR "/src/EmojiVoting"
RUN dotnet build "EmojiVoting.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "EmojiVoting.csproj" -c Debug -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmojiVoting.dll"]