# Sistema de Eleição (Lista 31)

Aplicação de console em **.NET 8** que simula uma eleição, com testes unitários em **xUnit** e histórico de commits no padrão **Conventional Commits**.

Atividade da disciplina **Garantia da Qualidade de Software** (Gestão e Qualidade de Software), com foco em versionamento semântico com Git.

## Funcionalidades

1. **Cadastrar candidatos** – nome e número, sem permitir números duplicados.
2. **Registrar eleitores** – número do título/CPF (simulado), usado para controlar quem já votou.
3. **Votar** – o eleitor digita o número do candidato (ou `0` para voto em branco). Cada eleitor vota uma única vez e números inexistentes são contabilizados como votos nulos.
4. **Apurar votos** – relatório com votos por candidato, nulos, brancos, total e o candidato vencedor (ou empate).

## Estrutura

```
SistemaEleicao.sln
├── SistemaEleicao.Console/   # aplicação de console e regras de negócio (namespace SistemaEleicao.Core)
│   ├── Program.cs            # menu interativo
│   ├── Candidato.cs
│   ├── Eleitor.cs
│   ├── EleicaoManager.cs     # cadastro, votação e apuração
│   ├── ResultadoApuracao.cs
│   └── TipoVoto.cs
└── SistemaEleicao.Tests/     # testes unitários (xUnit)
    ├── CandidatoTests.cs
    ├── VotacaoTests.cs
    └── ApuracaoTests.cs
```

## Pré-requisitos

- [.NET SDK 8.0](https://dotnet.microsoft.com/download) ou superior
- Git

## Como executar

```bash
git clone https://github.com/mmoreiralopes8-sketch/sistema-eleicao-xunit.git
cd sistema-eleicao-xunit

# Executar a aplicação
dotnet run --project SistemaEleicao.Console/SistemaEleicao.Console.csproj
```

Menu da aplicação:

```
===== SISTEMA DE ELEIÇÃO =====
1 - Cadastrar candidato
2 - Registrar eleitor
3 - Votar
4 - Apurar votos
0 - Sair
```

Fluxo sugerido: cadastre ao menos dois candidatos (opção 1), registre os eleitores (opção 2), faça os votos (opção 3) e, por fim, apure o resultado (opção 4).

## Como executar os testes

```bash
dotnet test
```

Os testes cobrem, entre outros cenários:

- impossibilidade de cadastrar dois candidatos com o mesmo número;
- validação de nome e número do candidato;
- impossibilidade de um eleitor votar duas vezes;
- votos em candidatos inexistentes contabilizados como nulos;
- voto em branco, totais da apuração, vencedor e empate.

## Padrão de commits

O projeto segue o [Conventional Commits](https://www.conventionalcommits.org/pt-br/v1.0.0/) (`feat`, `fix`, `test`, `refactor`, `docs`, `chore`). Para conferir o histórico:

```bash
git log --oneline
```
