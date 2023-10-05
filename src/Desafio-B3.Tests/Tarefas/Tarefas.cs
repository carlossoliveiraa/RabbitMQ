using ImplementandoRabiitMQ.Application.Domain;

public class TarefaTests
{
    [Fact]
    public void NovaTarefa_ComDescricaoValidaEDataFutura_DeveCriarTarefa()
    {
        // Arrange
        string descricao = "Tarefa de teste";
        DateTime data = DateTime.Now.AddDays(1);

        // Act
        var tarefa = new Tarefa(descricao, data);

        // Assert
        Assert.Equal(descricao, tarefa.Descricao);
        Assert.Equal(data, tarefa.Data);      
    }

    [Fact]
    public void NovaTarefa_ComDescricaoVazia_DeveGerarExcecao()
    {
        // Arrange
        string descricao = "";
        DateTime data = DateTime.Now.AddDays(1);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Tarefa(descricao, data));
    }

    [Fact]
    public void NovaTarefa_ComDataPassada_DeveGerarExcecao()
    {
        // Arrange
        string descricao = "Tarefa de teste";
        DateTime data = DateTime.Now.AddDays(-1);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Tarefa(descricao, data));
    }         
}
