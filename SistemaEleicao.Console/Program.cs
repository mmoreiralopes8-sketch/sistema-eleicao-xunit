using SistemaEleicao.Core;

var eleicao = new EleicaoManager();

while (true)
{
    ExibirMenu();
    var opcao = LerTexto("Escolha uma opção: ");

    // Fim da entrada (Ctrl+Z / Ctrl+D) encerra o programa como se fosse a opção "Sair".
    if (opcao is null || opcao == "0")
    {
        Console.WriteLine("Encerrando o sistema de eleição.");
        break;
    }

    try
    {
        switch (opcao)
        {
            case "1":
                CadastrarCandidato(eleicao);
                break;
            case "2":
                CadastrarEleitor(eleicao);
                break;
            case "3":
                Votar(eleicao);
                break;
            case "4":
                ApurarVotos(eleicao);
                break;
            default:
                Console.WriteLine("Opção inválida. Tente novamente.");
                break;
        }
    }
    catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
    {
        Console.WriteLine($"Não foi possível concluir a operação: {ex.Message}");
    }
}

static void ExibirMenu()
{
    Console.WriteLine();
    Console.WriteLine("===== SISTEMA DE ELEIÇÃO =====");
    Console.WriteLine("1 - Cadastrar candidato");
    Console.WriteLine("2 - Registrar eleitor");
    Console.WriteLine("3 - Votar");
    Console.WriteLine("4 - Apurar votos");
    Console.WriteLine("0 - Sair");
}

static string? LerTexto(string mensagem)
{
    Console.Write(mensagem);
    return Console.ReadLine()?.Trim();
}

static int LerNumero(string mensagem)
{
    var texto = LerTexto(mensagem);

    if (!int.TryParse(texto, out var numero))
    {
        throw new ArgumentException("Informe um número inteiro válido.");
    }

    return numero;
}

static void CadastrarCandidato(EleicaoManager eleicao)
{
    var nome = LerTexto("Nome do candidato: ") ?? string.Empty;
    var numero = LerNumero("Número do candidato: ");

    var candidato = eleicao.CadastrarCandidato(nome, numero);
    Console.WriteLine($"Candidato {candidato.Nome} ({candidato.Numero}) cadastrado com sucesso.");
}

static void CadastrarEleitor(EleicaoManager eleicao)
{
    var titulo = LerTexto("Título de eleitor/CPF: ") ?? string.Empty;

    var eleitor = eleicao.CadastrarEleitor(titulo);
    Console.WriteLine($"Eleitor {eleitor.Titulo} registrado com sucesso.");
}

static void Votar(EleicaoManager eleicao)
{
    var titulo = LerTexto("Título de eleitor/CPF: ") ?? string.Empty;
    var numero = LerNumero("Número do candidato (0 para voto em branco): ");

    var tipo = numero == 0
        ? eleicao.VotarEmBranco(titulo)
        : eleicao.Votar(titulo, numero);

    var descricao = tipo switch
    {
        TipoVoto.Candidato => "Voto registrado para o candidato.",
        TipoVoto.Nulo => "Candidato inexistente: voto registrado como nulo.",
        _ => "Voto em branco registrado."
    };

    Console.WriteLine(descricao);
}

static void ApurarVotos(EleicaoManager eleicao)
{
    var resultado = eleicao.Apurar();

    Console.WriteLine();
    Console.WriteLine("===== APURAÇÃO =====");

    foreach (var candidato in resultado.Candidatos)
    {
        Console.WriteLine($"{candidato.Numero} - {candidato.Nome}: {candidato.Votos} voto(s)");
    }

    Console.WriteLine($"Votos nulos: {resultado.VotosNulos}");
    Console.WriteLine($"Votos em branco: {resultado.VotosBrancos}");
    Console.WriteLine($"Total de votos: {resultado.TotalVotos}");

    if (resultado.Vencedor is not null)
    {
        Console.WriteLine($"Vencedor: {resultado.Vencedor.Nome} ({resultado.Vencedor.Numero}) com {resultado.Vencedor.Votos} voto(s).");
    }
    else if (resultado.HouveEmpate)
    {
        var nomes = string.Join(", ", resultado.Vencedores.Select(c => c.Nome));
        Console.WriteLine($"Empate entre: {nomes}.");
    }
    else
    {
        Console.WriteLine("Nenhum candidato recebeu votos válidos.");
    }
}
