FROM mcr.microsoft.com/dotnet/sdk:5.0

RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

WORKDIR /src
COPY ["WebApi/WebApi.csproj", "WebApi/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure.Shared/Infrastructure.Shared.csproj", "Infrastructure.Shared/"]
COPY ["Infrastructure.Identity/Infrastructure.Identity.csproj", "Infrastructure.Identity/"]
COPY ["Infrastructure.Persistence/Infrastructure.Persistence.csproj", "Infrastructure.Persistence/"]
RUN dotnet restore "WebApi/WebApi.csproj"

COPY . .

WORKDIR "/src/WebApi"

ADD migration_script.sh  /

RUN chmod +x /migration_script.sh

CMD ["/migration_script.sh"]


#CMD ["dotnet", "ef" ,"database", "update", "--startup-project", "WebApi.csproj", "-c", "IdentityContext"]
