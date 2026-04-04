using RabbitLab.Domain.Interface;
using RabbitLab.Domain.User;

namespace RabbitLab.Application.Services;

public class PessoaService
{
    private readonly IPessoaRepository _repository;

    public PessoaService(IPessoaRepository repository)
    {
        _repository = repository;
    }

    public async Task CriarPessoa(string nome, int idade, string mensagem)
    {
        var novaPessoa = new Pessoa
        {
            Nome = nome,
            Idade = idade,
            ConteudoMensagem = mensagem,
            Status = "PENDENTE"
        };

        await _repository.AdicionarAsync(novaPessoa);
    }

    public async Task<IEnumerable<Pessoa>> ObterTodasAsPessoas()
    {
        return await _repository.ListarTodosAsync();
    }

    public async Task<IEnumerable<Pessoa>> ObterPessoasPendentes()
    {
        return await _repository.ListarPendentesAsync();
    }

    public async Task AtualizarPessoa(int id, string nome, int idade, string mensagem, string status)
    {
        var pessoaAtualizada = new Pessoa
        {
            Id = id,
            Nome = nome,
            Idade = idade,
            ConteudoMensagem = mensagem,
            Status = status
        };

        await _repository.AtualizarAsync(pessoaAtualizada);
    }

    public async Task RemoverPessoa(int id)
    {
        await _repository.RemoverAsync(id);
    }
}