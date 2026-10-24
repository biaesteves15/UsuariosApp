# Contratos da API

Base das rotas: `/api/v1/usuarios`. Requisições e respostas de negócio usam JSON. As duas operações não exigem token no código atual.

## POST `/api/v1/usuarios/criar`

Requisitos: RF01 a RF04.

```json
{
  "nome": "Sergio Silva",
  "email": "sergio@example.com",
  "senha": "Abcdef1!"
}
```

| Campo | Tipo | Regra |
| --- | --- | --- |
| `nome` | string | Obrigatório, mínimo de 6 caracteres. |
| `email` | string | Obrigatório, válido e não cadastrado. |
| `senha` | string | Obrigatória; política descrita em [02-requirements.md](02-requirements.md). |

**201 Created** — `CriarUsuarioResponse`. Exemplo ilustrativo; id e data/hora são gerados na execução:

```json
{
  "id": "fce58c5b-a915-4e27-9961-8e95b79b4149",
  "dataHora": "2026-10-24T14:00:00",
  "mensagem": "Usuário cadastrado com sucesso."
}
```

`id` é um UUID; `dataHora` é um `DateTime` obtido com `DateTime.Now`. O controller não define cabeçalho `Location`.

**400 Bad Request** — para `ValidationException`, retorna um array de mensagens. Exemplo de email duplicado:

```json
[
  "O email informado já está cadastrado para outro usuário."
]
```

Outras mensagens incluem `O nome do usuário é obrigatório.`, `O nome do usuário deve ter no mínimo 6 caracteres.`, `O email do usuário é obrigatório.`, `O email deve conter um endereço válido.`, `A senha do usuário é obrigatória.` e `A senha deve ter pelo menos 1 letra minúscula, 1 letra maiúscula, 1 número e 1 símbolo, e no mínimo 8 caracteres.` Várias mensagens podem ser retornadas na mesma requisição.

## POST `/api/v1/usuarios/autenticar`

Requisitos: RF05 a RF07.

```json
{
  "email": "sergio@example.com",
  "senha": "Abcdef1!"
}
```

`AutenticarUsuarioRequest` contém duas strings: `email` e `senha`. O serviço consulta a correspondência das credenciais, sem executar `UsuarioValidation`.

**200 OK** — `AutenticarUsuarioResponse`. Exemplo ilustrativo; o valor do token é apenas um marcador:

```json
{
  "id": "fce58c5b-a915-4e27-9961-8e95b79b4149",
  "nome": "Sergio Silva",
  "email": "sergio@example.com",
  "perfil": "Operador",
  "token": "<JWT gerado na execução>",
  "expiraEm": "2026-10-24T18:00:00Z"
}
```

`id` é um UUID; `nome`, `email`, `perfil` e `token` são strings. `expiraEm` é um `DateTime` em UTC, calculado com a duração configurada. O perfil retornado é o nome associado ao usuário consultado.

**401 Unauthorized** — credenciais sem correspondência:

```json
{
  "message": "Acesso negado. Usuário inválido."
}
```

## Erros e documentação automática

O atributo `[ApiController]` também pode produzir HTTP 400 por falhas de binding/validação do modelo, com o formato padrão do ASP.NET Core; esse caso é diferente do array retornado pelo cadastro. Exceções fora dos tratamentos explícitos do controller não possuem contrato de erro customizado.

Swagger UI: `/swagger`; documento Swagger: `/swagger/v1/swagger.json`. Em Development, `MapOpenApi()` também disponibiliza `/openapi/v1.json`. O código não declara todos os tipos/status de resposta com atributos `ProducesResponseType`; este documento registra os retornos explícitos do controller.
