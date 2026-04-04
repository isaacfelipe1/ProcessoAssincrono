using Microsoft.AspNetCore.Mvc;
using RabbitLab.Application.Services;
using RabbitLab.Application.DTOs;

namespace RabbitLab.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PessoaController : ControllerBase
{
    private readonly PessoaService _pessoaService;

    public PessoaController(PessoaService pessoaService)
    {
        _pessoaService = pessoaService;
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] PessoaRequest request)
    {
        await _pessoaService.CriarPessoa(request.Nome, request.Idade, request.Mensagem);
        return Ok(new { msg = "Salvo com sucesso!", status = "PENDENTE" });
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var lista = await _pessoaService.ObterTodasAsPessoas();
        return Ok(lista);
    }

    [HttpGet("pendentes")]
    public async Task<IActionResult> GetPendentes()
    {
        var lista = await _pessoaService.ObterPessoasPendentes();
        return Ok(lista);
    }
}