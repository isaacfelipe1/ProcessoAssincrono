using FluentAssertions;
using Moq;
using RabbitLab.Application.Services;
using RabbitLab.Domain.Interface;
using RabbitLab.Domain.User;

namespace RabbitLab.Tests.Services;

public class PessoaServiceTests
{
    private readonly Mock<IPessoaRepository> _repositoryMock;
    private readonly PessoaService _service;

    public PessoaServiceTests()
    {

        _repositoryMock = new Mock<IPessoaRepository>();
        _service = new PessoaService(_repositoryMock.Object);
    }

    [Fact]
    public async Task CriarPessoa_DeveChamarAdicionarAsyncComStatusPendente()
    {
        // Arrange
        var nome = "Isaac";
        var idade = 28;
        var mensagem = "Solicito Reajuste";

        // Act
        await _service.CriarPessoa(nome, idade, mensagem);

        _repositoryMock.Verify(r => r.AdicionarAsync(It.Is<Pessoa>(p =>
            p.Nome == nome &&
            p.Status == "PENDENTE"
        )), Times.Once);
    }

    [Fact]
    public async Task ObterTodasAsPessoas_DeveRetornarListaCorreta_QuandoExistemDados()
    {
        // Arrange
        var pessoasFake = new List<Pessoa>
        {
            new Pessoa { Id = 1, Nome = "Joao", Status = "PENDENTE" },
            new Pessoa { Id = 2, Nome = "Maria", Status = "PROCESSADO" }
        };

        _repositoryMock.Setup(r => r.ListarTodosAsync()).ReturnsAsync(pessoasFake);

        // Act
        var resultado = await _service.ObterTodasAsPessoas();

        resultado.Should().NotBeNull()
                 .And.HaveCount(2)
                 .And.Contain(p => p.Nome == "Maria");
    }

    [Fact]
    public async Task AtualizarPessoa_DeveChamarAtualizarAsyncComDadosCorretos()
    {
        var id = 1;
        var nome = "Ana";
        var idade = 30;
        var mensagem = "Mensagem atualizada";
        var status = "PROCESSADO";

        await _service.AtualizarPessoa(id, nome, idade, mensagem, status);

        _repositoryMock.Verify(r => r.AtualizarAsync(It.Is<Pessoa>(p =>
            p.Id == id &&
            p.Nome == nome &&
            p.Idade == idade &&
            p.ConteudoMensagem == mensagem &&
            p.Status == status
        )), Times.Once);
    }

    [Fact]
    public async Task RemoverPessoa_DeveChamarRemoverAsyncComId()
    {
        var id = 2;

        await _service.RemoverPessoa(id);

        _repositoryMock.Verify(r => r.RemoverAsync(id), Times.Once);
    }
}