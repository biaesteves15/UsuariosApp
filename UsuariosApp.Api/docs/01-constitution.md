# Constituição do projeto

## Finalidade

Registrar a especificação do UsuariosApp a partir da implementação existente em 24/10/2026. Esta é uma documentação retrospectiva para adoção de Spec Driven Development (SDD); não representa funcionalidades futuras.

## Princípios

1. **Fidelidade:** descrever somente comportamentos identificados no código e distinguir implementação, cobertura de testes e limitações.
2. **Responsabilidades:** manter a divisão existente entre API, Application, Domain, Infra.Data, Infra.Security e Tests.
3. **Contratos:** preservar os DTOs, rotas e respostas HTTP documentados em [04-apicontracts.md](04-apicontracts.md).
4. **Regras de negócio:** usar como referência os validadores e o `UsuarioService`, sem atribuir ao fluxo regras que não são executadas.
5. **Rastreabilidade:** relacionar requisitos, tarefas e testes pelos identificadores desta documentação.
6. **Evidência:** uma tarefa marcada como concluída indica implementação identificada; não significa que todos os seus cenários foram testados.

## Escopo atual

- Cadastro de usuário com associação ao perfil `Operador`.
- Autenticação por email e senha, com emissão de JWT.
- Armazenamento com Entity Framework Core InMemory.
- Documentação HTTP por Swagger e OpenAPI.
- Testes unitários e de integração existentes.

## Uso dos documentos

| Documento | Papel |
| --- | --- |
| [02-requirements.md](02-requirements.md) | Requisitos e regras implementados |
| [03-architecture.md](03-architecture.md) | Componentes, dependências e dados |
| [04-apicontracts.md](04-apicontracts.md) | Entradas, saídas e erros da API |
| [05-tasks.md](05-tasks.md) | Tarefas já implementadas e suas evidências |
| [06-tests.md](06-tests.md) | Cobertura existente e comandos de execução |
| [07-checklist.md](07-checklist.md) | Conferência do estado documentado |

Em futuras alterações, atualizar primeiro os requisitos e contratos afetados e, depois, manter arquitetura, tarefas, testes e checklist coerentes com a entrega. Essa orientação organiza o uso da especificação; não descreve uma automação existente.
