# Testes existentes

O projeto `UsuariosApp.Tests` utiliza xUnit e FluentAssertions. Os testes de integração usam `WebApplicationFactory<Program>`, `HttpClient` e dados gerados com Bogus. O projeto referencia `coverlet.collector`, mas isso não estabelece uma meta de cobertura.

## Cobertura identificada no código

| Classe | Cenários existentes | Relação |
| --- | --- | --- |
| `UsuarioValidationTest` | Usuário válido; nome ausente e limite mínimo; email ausente, inválido e duplicado; senha ausente, fora da política e válida; aceitação atual de `Guid.Empty`. | RF02/RF03 |
| `PerfilValidationTest` | Nome válido, ausente e abaixo do mínimo; aceitação atual de `Guid.Empty`. | Validador de perfil isolado |
| `UsuarioServiceTest` | Cadastro com perfil existente ou novo; ordem de gravação; dados inválidos; email duplicado; falhas na consulta de email/perfil e na gravação de perfil/usuário. | RF01 a RF04 |
| `DataContextTest` | Provedor InMemory e inicialização dos DbSets de usuários e perfis. | Infraestrutura |
| `UsuariosControllerTest` | Cadastro válido retorna 201; dados inválidos e email duplicado retornam 400; autenticação válida retorna 200; usuário inexistente retorna 401. | RF01/RF02/RF03/RF05/RF06 |

Os doubles de repositório registram consultas e adições e permitem configurar falhas. O método `Obter(email, senha)` do double de usuário lança `NotSupportedException`; ele não é usado para testar autenticação em `UsuarioServiceTest`.

Os testes do serviço verificam o encapsulamento da falha de consulta de email em `AggregateException`, decorrente do uso de `.Result` na validação. Também verificam que erros de validação impedem a consulta do perfil e a gravação.

## Limites da evidência

- `UsuarioServiceTest` cobre cadastro, sem testes unitários do método `Autenticar`.
- Não há classe específica de testes de `TokenService`, nem assertions sobre assinatura, claims ou expiração do JWT.
- A integração de autenticação verifica status HTTP; não verifica o conteúdo do token ou do corpo retornado.
- Os testes de contexto não demonstram aplicação dos mapeamentos nem garantias relacionais.
- A integração utiliza a configuração InMemory da aplicação; não configura um banco exclusivo por teste. Os nomes de usuários são aleatórios, podendo afetar casos que exigem mínimo de 6 caracteres.

Esses pontos delimitam a cobertura existente e não constituem novos requisitos ou tarefas aprovadas.

## Execução

Na raiz da solução, com SDK .NET 10 e dependências restauradas:

```powershell
dotnet test UsuariosApp.Tests/UsuariosApp.Tests.csproj
```

Para executar apenas os testes do serviço:

```powershell
dotnet test UsuariosApp.Tests/UsuariosApp.Tests.csproj --filter FullyQualifiedName~UsuarioServiceTest
```

Última execução observada nesta sessão, antes da criação da documentação: **12 casos de `UsuarioServiceTest` aprovados, 0 falhas**. A suíte completa não foi executada nesta etapa documental. A presença dos demais testes foi verificada por leitura do código.
