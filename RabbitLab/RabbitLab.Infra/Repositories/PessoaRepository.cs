using Dapper;
using RabbitLab.Domain.Interface;
using RabbitLab.Domain.User;
using System.Data;

namespace RabbitLab.Infra.Repositories;

public class PessoaRepository : IPessoaRepository
{
    private readonly IDbConnection _dbConnection;

    public PessoaRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task AdicionarAsync(Pessoa pessoa)
    {
        var sql = @"INSERT INTO MensagensProcessamento (Nome, Idade, ConteudoMensagem, Status) 
                    VALUES (@Nome, @Idade, @ConteudoMensagem, @Status)";

        await _dbConnection.ExecuteAsync(sql, pessoa);
    }
    public async Task<IEnumerable<Pessoa>> ListarTodosAsync()
    {
        var sql = "SELECT Id, Nome, Idade, ConteudoMensagem, Status FROM MensagensProcessamento";
        return await _dbConnection.QueryAsync<Pessoa>(sql);
    }

    public async Task<IEnumerable<Pessoa>> ListarPendentesAsync()
    {
        var sql = "SELECT Id, Nome, Idade, ConteudoMensagem, Status FROM MensagensProcessamento WHERE Status = 'PENDENTE'";
        return await _dbConnection.QueryAsync<Pessoa>(sql);
    }
}