# Checklist do estado atual

Conferência documental baseada no código existente em 24/10/2026. Cada marca indica presença ou verificação descrita, sem certificar execução de toda a suíte ou prontidão para produção.

## Implementação identificada

- [x] 01. Entidades `Usuario` e `Perfil` e associação por `PerfilId` identificadas.
- [x] 02. Regras de cadastro e consulta de email duplicado documentadas — RF01/RF02/RF03.
- [x] 03. Busca e criação automática do perfil `Operador` identificadas — RF04.
- [x] 04. Endpoint de cadastro e retornos 201/400 conferidos no controller.
- [x] 05. Consulta por email e senha com inclusão do perfil identificada — RF05/RF06.
- [x] 06. Endpoint de autenticação e retornos 200/401 conferidos no controller.
- [x] 07. Emissão de JWT, claims, assinatura HS256 e configuração de expiração documentadas — RF07.
- [x] 08. Registros scoped de serviços, contexto e repositórios conferidos em `Program.cs`.
- [x] 09. EF Core InMemory, Swagger e OpenAPI documentados conforme a configuração atual.

## Testes e limites conferidos

- [x] 10. Testes existentes dos validadores, serviço de cadastro, contexto e integração inventariados.
- [x] 11. Resultado observado dos 12 testes de `UsuarioServiceTest` registrado; execução da suíte completa não presumida.
- [x] 12. Ausência de testes específicos de assinatura, claims e expiração do JWT registrada.
- [x] 13. Ausência de configuração de validação JWT e de rotas com `[Authorize]` registrada.
- [x] 14. Armazenamento/comparação de senha em texto simples e banco sem persistência durável registrados.
- [x] 15. Mapeamentos declarados sem registro no contexto e limites da unicidade por consulta registrados.
- [x] 16. `PerfilValidation` isolado do serviço e aceitação atual de `Guid.Empty` documentados.

## Documentação entregue

- [x] 17. Constituição e escopo registrados em `01-constitution.md`.
- [x] 18. Requisitos implementados registrados em `02-requirements.md`.
- [x] 19. Arquitetura e modelo de dados representados em `03-architecture.md`, com Mermaid.
- [x] 20. Contratos de cadastro e autenticação registrados em `04-apicontracts.md`.
- [x] 21. Tarefas implementadas enumeradas e marcadas em `05-tasks.md`.
- [x] 22. Cobertura existente e comandos registrados em `06-tests.md`.
- [x] 23. Checklist enumerado e marcado neste arquivo.

As limitações registradas descrevem o projeto atual; não foram incluídas como funcionalidades implementadas ou tarefas adicionais a executar.
