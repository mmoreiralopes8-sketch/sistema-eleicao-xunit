namespace SistemaEleicao.Core;

/// <summary>
/// Representa um eleitor identificado pelo número do título (ou CPF simulado).
/// </summary>
public class Eleitor
{
    public Eleitor(string titulo)
    {
        Titulo = titulo;
    }

    public string Titulo { get; }

    public bool JaVotou { get; private set; }

    internal void MarcarComoVotou()
    {
        JaVotou = true;
    }
}
