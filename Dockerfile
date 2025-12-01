# Imagem base para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar arquivo de projeto e restaurar dependências
COPY ["Padoka.csproj", "./"]
RUN dotnet restore "Padoka.csproj"

# Copiar todo o código fonte
COPY . .

# Build da aplicação
RUN dotnet build "Padoka.csproj" -c Release -o /app/build

# Publicar a aplicação
FROM build AS publish
RUN dotnet publish "Padoka.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Imagem final de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Expor porta
EXPOSE 9009

# Copiar arquivos publicados
COPY --from=publish /app/publish .

# Definir variáveis de ambiente
ENV ASPNETCORE_URLS=http://+:9009
ENV ASPNETCORE_ENVIRONMENT=Production

# Comando de inicialização
ENTRYPOINT ["dotnet", "Padoka.dll"]
