# Arquitetura do projeto

Este projeto deve evoluir para uma arquitetura modular com Vertical Slice Architecture.
Nome de classes, objetos, variaveis devem ser em ingles, com excecao de nomes de dominio e regras de negocio.
NUNCA mudar as portas usadas nas aplicações.
## Estrutura

- Novas features devem ficar no projeto mais proximo do caso de uso: `src/{Modulo}/Features/{FeatureName}`.
- Cada feature deve ser autocontida e conter, quando aplicavel:
  - `Request.cs`
  - `Response.cs`
  - `Validator.cs`
  - `Handler.cs`
- Cada feature deve ter seus testes unitarios e de integracao em `tests/{Modulo}/Features/{FeatureName}`.
- Controllers devem ser finos: receber HTTP, chamar validator/handler e traduzir o resultado para `IActionResult`.
- Nao adicionar novas regras de negocio em controllers.
- NUNCA USAR banco de dados difernte de SQL SERVER, NUNCA MUDAR AS configurações de infra sem SOLICITACAO.

## Regras de implementacao

- Usar FluentValidation para novas validacoes de entrada.
- Nao adicionar novas validacoes com DataAnnotations.
- Todo fluxo async deve receber e propagar `CancellationToken`.
- Queries de leitura com Entity Framework devem usar `AsNoTracking()`.
- Listagens devem ser paginadas e retornar envelope com `items`, `page`, `pageSize`, `totalItems` e `totalPages`.
- Paginacao deve usar `page=1` e `pageSize=10` como padrao, com `pageSize` maximo de 50.
- Evitar `Include()` sem necessidade.
- Priorizar `Select()` para projetar DTOs/Responses, evitando carregar entidades completas.
- Nao criar services genericos compartilhados sem justificativa explicita.
- Repositorios e services legados podem permanecer durante migracao, mas novas features devem preferir handlers especificos e projecoes diretas quando isso for mais simples.
