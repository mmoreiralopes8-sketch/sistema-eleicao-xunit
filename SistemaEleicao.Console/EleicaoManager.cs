namespace SistemaEleicao.Core;

/// <summary>
/// Gerencia candidatos, eleitores, votação e apuração.
/// </summary>
public class EleicaoManager
{
    private readonly List<Candidato> _candidatos = new();
    private readonly List<Eleitor> _eleitores = new();
    private int _votosNulos;
    private int _votosBrancos;

    public IReadOnlyList<Candidato> Candidatos => _candidatos;

    public IReadOnlyList<Eleitor> Eleitores => _eleitores;

    public Candidato CadastrarCandidato(string nome, int numero)
    {
        var candidato = new Candidato(nome, numero);

        if (_candidatos.Any(c => c.Numero == candidato.Numero))
        {
            throw new InvalidOperationException($"Já existe um candidato cadastrado com o número {numero}.");
        }

        _candidatos.Add(candidato);
        return candidato;
    }

    public Eleitor CadastrarEleitor(string titulo)
    {
        var tituloNormalizado = NormalizarTitulo(titulo);

        if (_eleitores.Any(e => e.Titulo == tituloNormalizado))
        {
            throw new InvalidOperationException($"O eleitor {tituloNormalizado} já está cadastrado.");
        }

        var eleitor = new Eleitor(tituloNormalizado);
        _eleitores.Add(eleitor);
        return eleitor;
    }

    public TipoVoto Votar(string titulo, int numeroCandidato)
    {
        var eleitor = ObterEleitorApto(titulo);
        var candidato = _candidatos.FirstOrDefault(c => c.Numero == numeroCandidato);

        eleitor.MarcarComoVotou();

        if (candidato is null)
        {
            _votosNulos++;
            return TipoVoto.Nulo;
        }

        candidato.ReceberVoto();
        return TipoVoto.Candidato;
    }

    public TipoVoto VotarEmBranco(string titulo)
    {
        var eleitor = ObterEleitorApto(titulo);

        eleitor.MarcarComoVotou();
        _votosBrancos++;
        return TipoVoto.Branco;
    }

    public ResultadoApuracao Apurar()
    {
        var maiorVotacao = _candidatos.Count == 0 ? 0 : _candidatos.Max(c => c.Votos);
        var vencedores = maiorVotacao == 0
            ? new List<Candidato>()
            : _candidatos.Where(c => c.Votos == maiorVotacao).ToList();

        var votosValidos = _candidatos.Sum(c => c.Votos);

        return new ResultadoApuracao(
            _candidatos.OrderByDescending(c => c.Votos).ThenBy(c => c.Numero).ToList(),
            _votosNulos,
            _votosBrancos,
            votosValidos + _votosNulos + _votosBrancos,
            vencedores);
    }

    private Eleitor ObterEleitorApto(string titulo)
    {
        var tituloNormalizado = NormalizarTitulo(titulo);
        var eleitor = _eleitores.FirstOrDefault(e => e.Titulo == tituloNormalizado)
            ?? throw new InvalidOperationException($"O eleitor {tituloNormalizado} não está cadastrado.");

        if (eleitor.JaVotou)
        {
            throw new InvalidOperationException($"O eleitor {tituloNormalizado} já votou.");
        }

        return eleitor;
    }

    private static string NormalizarTitulo(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
        {
            throw new ArgumentException("O título/CPF do eleitor é obrigatório.", nameof(titulo));
        }

        var normalizado = new string(titulo.Where(c => c is not ('.' or '-' or ' ')).ToArray());

        if (normalizado.Length == 0 || !normalizado.All(char.IsDigit))
        {
            throw new ArgumentException("O título/CPF do eleitor deve conter apenas números.", nameof(titulo));
        }

        return normalizado;
    }
}

/// <summary>
/// Resultado final da apuração dos votos.
/// </summary>
public class ResultadoApuracao
{
    public ResultadoApuracao(
        IReadOnlyList<Candidato> candidatos,
        int votosNulos,
        int votosBrancos,
        int totalVotos,
        IReadOnlyList<Candidato> vencedores)
    {
        Candidatos = candidatos;
        VotosNulos = votosNulos;
        VotosBrancos = votosBrancos;
        TotalVotos = totalVotos;
        Vencedores = vencedores;
    }

    public IReadOnlyList<Candidato> Candidatos { get; }

    public int VotosNulos { get; }

    public int VotosBrancos { get; }

    public int TotalVotos { get; }

    public IReadOnlyList<Candidato> Vencedores { get; }

    public bool HouveEmpate => Vencedores.Count > 1;

    public Candidato? Vencedor => Vencedores.Count == 1 ? Vencedores[0] : null;
}
