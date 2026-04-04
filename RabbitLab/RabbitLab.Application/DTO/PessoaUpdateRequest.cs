namespace RabbitLab.Application.DTOs;

public record PessoaUpdateRequest(string Nome, int Idade, string Mensagem, string Status);
