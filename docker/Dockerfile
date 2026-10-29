FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY global.json Directory.Build.props ./
COPY src/AssistBot.Domain/AssistBot.Domain.csproj                 src/AssistBot.Domain/
COPY src/AssistBot.Application/AssistBot.Application.csproj       src/AssistBot.Application/
COPY src/AssistBot.Infrastructure/AssistBot.Infrastructure.csproj src/AssistBot.Infrastructure/
COPY src/AssistBot.Bot/AssistBot.Bot.csproj                       src/AssistBot.Bot/
RUN dotnet restore src/AssistBot.Bot/AssistBot.Bot.csproj

COPY src/ src/
RUN dotnet publish src/AssistBot.Bot/AssistBot.Bot.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/runtime:10.0 AS final
WORKDIR /app
COPY --from=build /app .
USER $APP_UID
ENTRYPOINT ["dotnet", "AssistBot.Bot.dll"]
