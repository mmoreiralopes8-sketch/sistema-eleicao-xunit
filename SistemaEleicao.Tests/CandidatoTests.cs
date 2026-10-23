using SistemaEleicao.Core;

namespace SistemaEleicao.Tests;

public class CandidatoTests
{
    [Fact]
    public void CadastrarCandidato_ComDadosValidos_AdicionaNaLista()
    {
        var manager = new EleicaoManager();

        var candidato = manager.CadastrarCandidato("Maria Souza", 13);

        Assert.Equal("Maria Souza", candidato.Nome);
        Assert.Equal(13, candidato.Numero);
        Assert.Equal(0, candidato.Votos);
        Assert.Single(manager.Candidatos);
    }

    [Fact]
    public void CadastrarCandidato_ComNumerosDiferentes_PermiteAmbos()
    {
        var manager = new EleicaoManager();

        manager.CadastrarCandidato("Maria Souza", 13);
        manager.CadastrarCandidato("João Lima", 45);

        Assert.Equal(2, manager.Candidatos.Count);
    }

    [Fact]
    public void CadastrarCandidato_ComNumeroDuplicado_LancaExcecao()
    {
        var manager = new EleicaoManager();
        manager.CadastrarCandidato("Maria Souza", 13);

        var erro = Assert.Throws<InvalidOperationException>(() => manager.CadastrarCandidato("João Lima", 13));

        Assert.Contains("13", erro.Message);
        Assert.Single(manager.Candidatos);
    }

    [Fact]
    public void CadastrarCandidato_ComNomeVazio_LancaArgumentException()
    {
        var manager = new EleicaoManager();

        Assert.Throws<ArgumentException>(() => manager.CadastrarCandidato("   ", 13));
        Assert.Empty(manager.Candidatos);
    }

    [Fact]
    public void CadastrarCandidato_ComNumeroInvalido_LancaArgumentException()
    {
        var manager = new EleicaoManager();

        Assert.Throws<ArgumentException>(() => manager.CadastrarCandidato("Maria Souza", 0));
        Assert.Throws<ArgumentException>(() => manager.CadastrarCandidato("Maria Souza", -5));
        Assert.Empty(manager.Candidatos);
    }
}
