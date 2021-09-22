


FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
ENV ASPNETCORE_URLS=http://+:8000
EXPOSE 8000
WORKDIR /src
COPY ["WebApi.csproj", ""]
RUN dotnet restore "WebApi.csproj"
COPY . .
WORKDIR "/src"

ENTRYPOINT ["dotnet","watch", "run", "--urls", "http://0.0.0.0:8000","--no-launch-profile"]
#ENTRYPOINT ["ls", "."]