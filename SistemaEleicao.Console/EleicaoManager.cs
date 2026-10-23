namespace SistemaEleicao.Core;

/// <summary>
/// Gerencia o cadastro de candidatos da eleição.
/// </summary>
public class EleicaoManager
{
    private readonly List<Candidato> _candidatos = new();

    public IReadOnlyList<Candidato> Candidatos => _candidatos;

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
}
