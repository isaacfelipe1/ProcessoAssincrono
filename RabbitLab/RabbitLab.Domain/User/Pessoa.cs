namespace RabbitLab.Domain.User;
public class Pessoa
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int Idade { get; set; }
    public string ConteudoMensagem { get; set; } = string.Empty;
    public string Status { get; set; } = "PENDENTE";
}
