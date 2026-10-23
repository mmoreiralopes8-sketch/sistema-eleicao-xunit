using SistemaEleicao.Core;

namespace SistemaEleicao.Tests;

public class VotacaoTests
{
    private static EleicaoManager CriarEleicao()
    {
        var manager = new EleicaoManager();
        manager.CadastrarCandidato("Maria Souza", 13);
        manager.CadastrarCandidato("João Lima", 45);
        manager.CadastrarEleitor("123456789012");
        manager.CadastrarEleitor("210987654321");
        return manager;
    }

    [Fact]
    public void CadastrarEleitor_ComTituloDuplicado_LancaExcecao()
    {
        var manager = CriarEleicao();

        Assert.Throws<InvalidOperationException>(() => manager.CadastrarEleitor("123456789012"));
        Assert.Equal(2, manager.Eleitores.Count);
    }

    [Fact]
    public void CadastrarEleitor_ComTituloFormatado_EquivaleAoTituloSoComNumeros()
    {
        var manager = CriarEleicao();

        Assert.Throws<InvalidOperationException>(() => manager.CadastrarEleitor("1234.5678.9012"));
    }

    [Fact]
    public void CadastrarEleitor_ComTituloInvalido_LancaArgumentException()
    {
        var manager = new EleicaoManager();

        Assert.Throws<ArgumentException>(() => manager.CadastrarEleitor(""));
        Assert.Throws<ArgumentException>(() => manager.CadastrarEleitor("abc123"));
        Assert.Empty(manager.Eleitores);
    }

    [Fact]
    public void Votar_ComCandidatoExistente_ContabilizaVotoParaOCandidato()
    {
        var manager = CriarEleicao();

        var tipo = manager.Votar("123456789012", 13);

        Assert.Equal(TipoVoto.Candidato, tipo);
        Assert.Equal(1, manager.Candidatos[0].Votos);
        Assert.True(manager.Eleitores[0].JaVotou);
    }

    [Fact]
    public void Votar_ComEleitorQueJaVotou_LancaExcecaoENaoContabilizaNovoVoto()
    {
        var manager = CriarEleicao();
        manager.Votar("123456789012", 13);

        Assert.Throws<InvalidOperationException>(() => manager.Votar("123456789012", 13));
        Assert.Throws<InvalidOperationException>(() => manager.VotarEmBranco("123456789012"));

        Assert.Equal(1, manager.Candidatos[0].Votos);
        Assert.Equal(1, manager.Apurar().TotalVotos);
    }

    [Fact]
    public void Votar_ComEleitorNaoCadastrado_LancaExcecao()
    {
        var manager = CriarEleicao();

        Assert.Throws<InvalidOperationException>(() => manager.Votar("999999999999", 13));
        Assert.Equal(0, manager.Apurar().TotalVotos);
    }

    [Fact]
    public void Votar_ComCandidatoInexistente_ContabilizaComoNulo()
    {
        var manager = CriarEleicao();

        var tipo = manager.Votar("123456789012", 99);

        Assert.Equal(TipoVoto.Nulo, tipo);
        Assert.Equal(1, manager.Apurar().VotosNulos);
        Assert.Equal(0, manager.Candidatos[0].Votos);
        Assert.Equal(0, manager.Candidatos[1].Votos);
        Assert.True(manager.Eleitores[0].JaVotou);
    }

    [Fact]
    public void VotarEmBranco_ContabilizaVotoBranco()
    {
        var manager = CriarEleicao();

        var tipo = manager.VotarEmBranco("123456789012");

        Assert.Equal(TipoVoto.Branco, tipo);
        Assert.Equal(1, manager.Apurar().VotosBrancos);
        Assert.True(manager.Eleitores[0].JaVotou);
    }
}
