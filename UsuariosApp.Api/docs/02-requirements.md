# Requisitos implementados

## Funcionalidades

| ID | Requisito | Comportamento e evidência |
| --- | --- | --- |
| RF01 | Cadastrar usuário | `UsuarioService.Criar` recebe nome, email e senha, valida e salva o usuário; a API retorna HTTP 201 com id, data/hora e mensagem. |
| RF02 | Impedir cadastro inválido | `UsuarioValidation` verifica nome, email e senha; o controller retorna HTTP 400 com as mensagens de validação. |
| RF03 | Rejeitar email duplicado | Consulta `IUsuarioRepository.EmailExistente` antes da gravação e rejeita email já cadastrado. |
| RF04 | Associar perfil Operador | Consulta o perfil pelo nome `Operador`; quando ausente, cria o perfil antes de salvar o usuário e atribui seu id a `PerfilId`. |
| RF05 | Autenticar usuário | Consulta email e senha com carregamento do perfil; retorna dados do usuário, perfil, token e expiração em HTTP 200. |
| RF06 | Rejeitar credenciais sem correspondência | `UsuarioService.Autenticar` lança `ApplicationException`; o controller retorna HTTP 401 com a mensagem de acesso negado. |
| RF07 | Emitir JWT | `TokenService.Gerar` usa assinatura HMAC SHA-256, emissor, audiência e duração configurados; inclui identificação, nome, email e perfil. |

## Regras do cadastro

| Campo | Regra executada |
| --- | --- |
| Nome | Obrigatório, com pelo menos 6 caracteres. |
| Email | Obrigatório, formato aceito por `EmailAddress()` do FluentValidation e ausência de correspondência no repositório. |
| Senha | Obrigatória, com pelo menos 8 caracteres, uma minúscula, uma maiúscula, um número e um símbolo; sem espaços em branco. |
| Id | Gerado com `Guid.NewGuid()` na entidade. A regra `NotNull()` não rejeita `Guid.Empty`. |
| Perfil | Definido pelo serviço como `Operador`; não é recebido no cadastro. |

O repositório compara email e senha com `string.Equals`, sem normalização de email ou transformação da senha. A senha é armazenada e comparada em texto simples.

`PerfilValidation` implementa nome obrigatório com mínimo de 6 caracteres e `Id.NotNull()`, mas não é chamado pelo fluxo de criação automática do perfil. Seus testes também registram a aceitação de `Guid.Empty`.

## Condições técnicas existentes

- Projetos com destino `net10.0`, API em ASP.NET Core e validações com FluentValidation.
- Banco em memória chamado `DbUsuarios`, sem persistência durável entre execuções da aplicação.
- Cadastro usa `DateTime.Now`; expiração do JWT usa `DateTime.UtcNow` acrescido de `Jwt:ExpirationInMinutes`.
- Swagger habilitado na inicialização; endpoint OpenAPI mapeado somente no ambiente Development.

## Limites do escopo

Não há endpoints para listar, editar ou excluir usuários, gerenciar perfis, recuperar senha ou renovar tokens. Não há configuração de autenticação JWT Bearer nem uso de `[Authorize]`. Não há validador específico para a autenticação. Limites máximos e índices declarados nos arquivos de mapeamento não estão aplicados ao contexto atual.
