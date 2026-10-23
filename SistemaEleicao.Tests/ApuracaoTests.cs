using SistemaEleicao.Core;

namespace SistemaEleicao.Tests;

public class ApuracaoTests
{
    private static EleicaoManager CriarEleicaoComEleitores(int quantidadeDeEleitores)
    {
        var manager = new EleicaoManager();
        manager.CadastrarCandidato("Maria Souza", 13);
        manager.CadastrarCandidato("João Lima", 45);

        for (var i = 1; i <= quantidadeDeEleitores; i++)
        {
            manager.CadastrarEleitor((1000 + i).ToString());
        }

        return manager;
    }

    [Fact]
    public void Apurar_SemVotos_NaoHaVencedor()
    {
        var manager = CriarEleicaoComEleitores(0);

        var resultado = manager.Apurar();

        Assert.Equal(0, resultado.TotalVotos);
        Assert.Null(resultado.Vencedor);
        Assert.False(resultado.HouveEmpate);
    }

    [Fact]
    public void Apurar_ComVotos_RetornaTotaisEVencedor()
    {
        var manager = CriarEleicaoComEleitores(5);
        manager.Votar("1001", 13);
        manager.Votar("1002", 13);
        manager.Votar("1003", 45);
        manager.Votar("1004", 77);
        manager.VotarEmBranco("1005");

        var resultado = manager.Apurar();

        Assert.Equal(5, resultado.TotalVotos);
        Assert.Equal(1, resultado.VotosNulos);
        Assert.Equal(1, resultado.VotosBrancos);
        Assert.NotNull(resultado.Vencedor);
        Assert.Equal(13, resultado.Vencedor!.Numero);
        Assert.Equal(2, resultado.Vencedor.Votos);
        Assert.Equal(13, resultado.Candidatos[0].Numero);
    }

    [Fact]
    public void Apurar_ComEmpate_NaoDefineVencedorUnico()
    {
        var manager = CriarEleicaoComEleitores(2);
        manager.Votar("1001", 13);
        manager.Votar("1002", 45);

        var resultado = manager.Apurar();

        Assert.True(resultado.HouveEmpate);
        Assert.Null(resultado.Vencedor);
        Assert.Equal(2, resultado.Vencedores.Count);
    }
}
