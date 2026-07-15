using MinimalApi.Dominio.Entidades;

namespace Test.Domain.Entidades;

[TestClass]
public class VeiculoTest
{
    [TestMethod]
    public void TestarGetSetPropriedadesVeiculo()
    {
        // Arrange
        var veiculo = new Veiculo();

        // Act
        veiculo.Id = 1;
        veiculo.Nome = "Testar Carro";
        veiculo.Marca = "Marca Teste";
        veiculo.Ano = 2025;

        // Assert
        Assert.AreEqual(1, veiculo.Id);
        Assert.AreEqual("Testar Carro", veiculo.Nome);
        Assert.AreEqual("Marca Teste", veiculo.Marca);
        Assert.AreEqual(2025, veiculo.Ano);
    }
}