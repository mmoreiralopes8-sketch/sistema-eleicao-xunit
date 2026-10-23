namespace SistemaEleicao.Core;

/// <summary>
/// Representa um candidato da eleição, identificado pelo número de urna.
/// </summary>
public class Candidato
{
    public Candidato(string nome, int numero)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            throw new ArgumentException("O nome do candidato é obrigatório.", nameof(nome));
        }

        if (numero <= 0)
        {
            throw new ArgumentException("O número do candidato deve ser maior que zero.", nameof(numero));
        }

        Nome = nome.Trim();
        Numero = numero;
    }

    public string Nome { get; }

    public int Numero { get; }

    public int Votos { get; private set; }

    internal void ReceberVoto()
    {
        Votos++;
    }
}
