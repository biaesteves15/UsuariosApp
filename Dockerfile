# Etapa 1: compilação e publicação com o SDK do .NET 10
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

# Define o diretório de trabalho
WORKDIR /src

# Copia a solução, incluindo todos os projetos referenciados
COPY . .

# Restaura os pacotes da API e dos projetos referenciados
RUN dotnet restore "UsuariosApp.Api/UsuariosApp.Api.csproj"

# Compila e publica a aplicação em modo Release
RUN dotnet publish "UsuariosApp.Api/UsuariosApp.Api.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false

# Etapa 2: execução com o runtime do ASP.NET Core 10
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

# Define o diretório da aplicação
WORKDIR /app

# Configura o ambiente de execução
ENV ASPNETCORE_ENVIRONMENT=Production

# Configura a porta HTTP utilizada pela API
ENV ASPNETCORE_HTTP_PORTS=8080

# Documenta a porta utilizada pelo container
EXPOSE 8080

# Copia os arquivos publicados na etapa anterior
COPY --from=build /app/publish .

# Executa a aplicação com o usuário sem privilégios da imagem
USER $APP_UID

# Inicia a API
ENTRYPOINT ["dotnet", "UsuariosApp.Api.dll"]