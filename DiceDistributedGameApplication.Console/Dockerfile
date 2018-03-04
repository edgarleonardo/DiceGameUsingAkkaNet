FROM microsoft/dotnet:2.0-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.0-sdk AS build
WORKDIR /src
COPY *.sln ./
COPY DiceDistributedGameApplication.Console/DiceDistributedGameApplication.Console.csproj DiceDistributedGameApplication.Console/
RUN dotnet restore
COPY . .
WORKDIR /src/DiceDistributedGameApplication.Console
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DiceDistributedGameApplication.Console.dll"]
