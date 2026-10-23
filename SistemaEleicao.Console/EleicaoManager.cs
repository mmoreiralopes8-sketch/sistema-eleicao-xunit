namespace SistemaEleicao.Core;

/// <summary>
/// Regras de negócio da eleição: candidatos, eleitores, votação e apuração.
/// </summary>
public class EleicaoManager
{
    private readonly List<Candidato> _candidatos = new();
    private readonly List<Eleitor> _eleitores = new();
    private static readonly char[] _separadoresDeTitulo = { '.', '-', ' ' };

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

    /// <summary>
    /// Consolida os votos computados e identifica o(s) candidato(s) mais votado(s).
    /// </summary>
    public ResultadoApuracao Apurar()
    {
        var votosValidos = _candidatos.Sum(c => c.Votos);
        var totalVotos = votosValidos + _votosNulos + _votosBrancos;
        var candidatosOrdenados = _candidatos
            .OrderByDescending(c => c.Votos)
            .ThenBy(c => c.Numero)
            .ToList();

        return new ResultadoApuracao(
            candidatosOrdenados,
            _votosNulos,
            _votosBrancos,
            totalVotos,
            ObterMaisVotados());
    }

    private List<Candidato> ObterMaisVotados()
    {
        var maiorVotacao = _candidatos.Count == 0 ? 0 : _candidatos.Max(c => c.Votos);

        return maiorVotacao == 0
            ? new List<Candidato>()
            : _candidatos.Where(c => c.Votos == maiorVotacao).ToList();
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

        var normalizado = new string(titulo.Where(c => !_separadoresDeTitulo.Contains(c)).ToArray());

        if (normalizado.Length == 0 || !normalizado.All(char.IsDigit))
        {
            throw new ArgumentException("O título/CPF do eleitor deve conter apenas números.", nameof(titulo));
        }

        return normalizado;
    }
}
