# FiapCloudGames.Games

## 📌 Objetivos
Microsserviço de pagamentos do Monólito [FiapCloudGames](https://github.com/MarioGuilherme/FiapCloudGames) que trata todas as regras e lógicas pertinente ao escopo de jogos, compras, pedidos e recomendações de jogos, juntamente com o sua base de dados, com integração com ElasticSearch e recomendações e pesquisas inteligentes de jogos pelo usuário e seu histório de compra.

## 🚀 Instruções de uso
Faça o clone do projeto e já acesse a pasta do projeto clonado:
```
git clone https://github.com/MarioGuilherme/FiapCloudGames.Games && cd .\FiapCloudGames.Games
```

### ▶️ Iniciar Projeto
  1 - Navegue até o diretório da camada API da aplicação:
  ```
  cd .\FiapCloudGames.Games.API\
  ```
  2 - Insira o comando de execução do projeto:
  
  _(O BANCO DE DADOS É CRIADO AUTOMATICAMENTE QUANDO O PROJETO É INICIADO, SEM PRECISAR EXECUTAR O ```Database-Update```)_:
  ```
  docker-compose up
  ```
  3 - Acesse https://localhost:8082/swagger/index.html

### 🧪 Executar testes
  1 - Navegue até o diretório dos testes:
  ```
  cd .\FiapCloudGames.Games.Tests\
  ```
  2 - E insira o comando de execução de testes:
  ```
  dotnet test
  ```

## 🛠️ Tecnologias e Afins
- .NET 8 com C# 12;
- ASP.NET Core;
- Uso de Middlewares e IActionFilters;
- ElasticSearch;
- EntityFrameworkCore;
- Unit Of Work;
- SQL SERVER;
- FluentValidation;
- Swagger;
- xUnit junto com Moq;
- Autenticação JWT;
