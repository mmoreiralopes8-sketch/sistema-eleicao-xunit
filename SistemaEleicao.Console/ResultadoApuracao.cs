namespace SistemaEleicao.Core;

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
