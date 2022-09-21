FROM mcr.microsoft.com/dotnet/sdk:6.0.401

ENV PATH="${PATH}:/root/.dotnet/tools"

RUN dotnet tool install --global dotnet-ef  --version 6.0.9

WORKDIR /src
COPY ["WebApi/WebApi.csproj", "WebApi/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure.Shared/Infrastructure.Shared.csproj", "Infrastructure.Shared/"]
COPY ["Infrastructure.Identity/Infrastructure.Identity.csproj", "Infrastructure.Identity/"]
COPY ["Infrastructure.Persistence/Infrastructure.Persistence.csproj", "Infrastructure.Persistence/"]

RUN dotnet restore "WebApi/WebApi.csproj"  --disable-parallel

COPY . .

WORKDIR "/src/WebApi/."

ADD migration_script.sh  .

RUN chmod +x ./migration_script.sh

CMD /bin/bash ./migration_script.sh
