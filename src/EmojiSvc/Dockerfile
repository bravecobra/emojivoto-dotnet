#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
ARG SemVer=0.1.0
ENV SemVer=$SemVer
COPY ["src/EmojiShared/EmojiShared.csproj", "EmojiShared/"]
COPY ["src/EmojiSvc/EmojiSvc.csproj", "EmojiSvc/"]
RUN dotnet restore "EmojiSvc/EmojiSvc.csproj"
COPY ./src/EmojiShared ./EmojiShared
COPY ./src/EmojiSvc ./EmojiSvc
COPY ./src/protos ./protos
COPY ./.git ./.git
WORKDIR "/src/EmojiSvc"
RUN dotnet build "EmojiSvc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EmojiSvc.csproj" -c Release -o /app/publish

FROM base AS final
RUN apt-get update && apt-get install -y procps
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmojiSvc.dll"]