# SDK image para compilar e rodar
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copia todos os arquivos da solution (incluindo os compartilhados)
COPY . .

# Publica o projeto API direto na pasta final
RUN dotnet publish "BankMore.Account/BankMore.Account.API/BankMore.Account.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copia os arquivos publicados
COPY --from=build /app/publish .


ENTRYPOINT ["dotnet", "BankMore.Account.API.dll"]