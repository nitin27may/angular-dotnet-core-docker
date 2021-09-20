# Base to run final published code
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443


# Build step to create artifact
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["WebApi.csproj", ""]
RUN dotnet restore "./WebApi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "WebApi.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "WebApi.csproj" -c Release -o /app/publish

#  run publishe code
FROM base AS final
# RUN dotnet tool install --global dotnet-ef
# RUN export PATH="$PATH:$HOME/.dotnet/tools/"
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]