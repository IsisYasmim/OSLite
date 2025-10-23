# OSLite - Mini Domínio de Ordem de Serviço

Este projeto implementa um mini-domínio para gerenciar Ordens de Serviço (OS) em uma assistência técnica. A modelagem segue os princípios de Domain-Driven Design (DDD) e Test-Driven Development (TDD), com foco em tipos adequados, encapsulamento e proteção rigorosa de invariantes.

## Justificativa das Decisões de Modelagem: Tipos de C#
A escolha do tipo de dado em C# (class, record struct, ou enum) foi determinada pela semântica do domínio de cada conceito, priorizando a distinção entre Identidade (Entidades) e Valor (Value Objects).

### 1. Classes: Entidades e Agregados (Semântica de Identidade)
As classes são utilizadas para objetos que possuem uma identidade única e um ciclo de vida longo. Sua igualdade é baseada na referência, não nos valores de seus atributos.

| Tipo | Onde Usado | Porque |
|------|------------|--------|
| class | Cliente | É uma Entidade central com um Id único. Sua igualdade é verificada pela identidade, e não pelo nome ou e-mail.|
| class | OrdemDeServico | É o Aggregate Root (Raiz de Agregado). Possui um Id único e gerencia a consistência de seus componentes (ItemDeServico). Seu estado (StatusOS) é mutável por meio de métodos controlados (IniciarExecucao, Concluir), protegendo as regras de transição |

### 2. Record Struct: Value Objects (Semântica de Valor)
Os record struct são a escolha ideal para Value Objects (VOs): objetos que são definidos unicamente pelos seus valores e são imutáveis.
| Tipo | Onde Usado | Porque |
|------|------------|--------|
| record struct | Money | Representa uma quantia monetária. Foi escolhido por ser pequeno, imutável e, mais importante, garantir igualdade baseada em valor (dois objetos Money com o valor $10,00$ são idênticos, independentemente da instância). O struct ajuda a evitar alocações de heap desnecessárias.|
| record struct | ItemDeServico | Embora seja parte da composição (e tecnicamente um VO), é um record imutável. Sua igualdade é baseada nos valores (Descrição, Quantidade, PrecoUnitario), o que simplifica a lógica e a testabilidade do subtotal derivado. |

### 3. Enums: Estados (Tornando o Domínio Explícito)
Enums são usados para representar um conjunto fixo e limitado de estados ou categorias.
| Tipo | Onde Usado | Porque |
|------|------------|--------|
| enum | StatusOS | Define o fluxo de vida da Ordem de Serviço (Aberta, EmExecucao, Concluida, Cancelada). O uso de enum torna as regras de negócio sobre transições de estado explícitas e fortemente tipadas, eliminando o risco de "strings mágicas" e melhorando drasticamente a legibilidade e a manutenção do código.|
