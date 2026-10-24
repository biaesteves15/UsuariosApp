# Tarefas implementadas

Registro retrospectivo. Os itens marcados indicam código ou documentação existente, sem presumir execução de todos os testes ou funcionalidades adicionais.

- [x] 01. Estruturar a solução em API, Application, Domain, Infra.Data, Infra.Security e Tests — projetos da solução `UsuariosApp.slnx`.
- [x] 02. Modelar `Usuario` e `Perfil`, com identificadores GUID e associação por `PerfilId` — entidades do Domain; RF01/RF04.
- [x] 03. Definir contratos de repositório para adição, consulta de perfil, verificação de email e busca de credenciais — `IUsuarioRepository` e `IPerfilRepository`.
- [x] 04. Implementar validações de nome, email, senha e duplicidade — `UsuarioValidation`; RF02/RF03.
- [x] 05. Implementar `PerfilValidation` — classe existente com testes próprios; não integrada ao fluxo do serviço.
- [x] 06. Configurar armazenamento em memória e implementar repositórios — `DataContext`, `UsuarioRepository` e `PerfilRepository`.
- [x] 07. Declarar os mapeamentos `UsuarioMap` e `PerfilMap` — arquivos existentes; sem aplicação no `DataContext` atual.
- [x] 08. Implementar cadastro com busca/criação do perfil `Operador` — `UsuarioService.Criar`; RF01 a RF04.
- [x] 09. Expor cadastro com respostas 201 e 400 — `UsuariosController.CriarAsync`.
- [x] 10. Implementar consulta de credenciais com inclusão do perfil — `UsuarioRepository.Obter`; RF05/RF06.
- [x] 11. Gerar JWT com claims, assinatura e expiração configuráveis — `TokenService`; RF07.
- [x] 12. Implementar autenticação e respostas 200 e 401 — `UsuarioService.Autenticar` e `UsuariosController.AutenticarAsync`; RF05/RF06.
- [x] 13. Registrar serviços e repositórios no contêiner de DI — `Program.cs`.
- [x] 14. Habilitar Swagger e OpenAPI — `Program.cs`.
- [x] 15. Implementar testes unitários dos validadores, cadastro no serviço e contexto — classes descritas em [06-tests.md](06-tests.md).
- [x] 16. Implementar testes de integração para cadastro e autenticação — `UsuariosControllerTest`.
- [x] 17. Fornecer `TokenService` nas instanciações de `UsuarioService` dos testes de cadastro — `UsuarioServiceTest`.
- [x] 18. Documentar o estado implementado nos sete arquivos SDD desta pasta.
