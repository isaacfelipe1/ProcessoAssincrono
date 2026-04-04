using RabbitLab.Domain.User;

namespace RabbitLab.Domain.Interface;
public interface IPessoaRepository
{
    Task AdicionarAsync(Pessoa pessoa);
    Task<IEnumerable<Pessoa>> ListarTodosAsync();
    Task<IEnumerable<Pessoa>> ListarPendentesAsync();

}
