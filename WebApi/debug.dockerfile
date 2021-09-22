


FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

WORKDIR /vsdbg
RUN apt-get update \
&& apt-get install -y --no-install-recommends \
unzip \
&& rm -rf /var/lib/apt/lists/* \
&& curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /vsdbg

ENV ASPNETCORE_URLS=http://+:8000
EXPOSE 8000
WORKDIR /src
COPY ["WebApi.csproj", ""]
RUN dotnet restore "WebApi.csproj"
COPY . .
WORKDIR "/src"

ENTRYPOINT ["dotnet","watch", "run", "--urls", "http://0.0.0.0:8000","--no-launch-profile"]
#ENTRYPOINT ["ls", "."]