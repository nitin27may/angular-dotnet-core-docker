#!/bin/sh
    
dotnet ef database update --startup-project ../WebApi/WebApi.csproj -c "ApplicationDbContext"

dotnet ef database update --startup-project ../WebApi/WebApi.csproj -c "IdentityContext"
