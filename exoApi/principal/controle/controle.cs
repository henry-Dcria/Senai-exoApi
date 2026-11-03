using ExoApi.Models;
using ExoApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ExoApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjetosController : ControllerBase
    {
        private readonly ProjetoRepository _projetoRepository;

        public ProjetosController(ProjetoRepository projetoRepository)
        {
            _projetoRepository = projetoRepository;
        }

        // GET: api/projetos
        [HttpGet]
        public IActionResult Listar()
        {
            try
            {
                var projetos = _projetoRepository.Listar();
                return Ok(projetos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Erro ao listar os projetos.", detalhe = ex.Message });
            }
        }

        // GET: api/projetos/1
        [HttpGet("{id}")]
        public IActionResult BuscarPorId(int id)
        {
            try
            {
                Projeto projeto = _projetoRepository.BuscarPorId(id);
                if (projeto == null)
                {
                    return NotFound(new { mensagem = $"Projeto com ID {id} não foi encontrado." });
                }
                return Ok(projeto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Erro ao processar a consulta.", detalhe = ex.Message });
            }
        }

        // POST: api/projetos
        [HttpPost]
        public IActionResult Cadastrar(Projeto projeto)
        {
            try
            {
                if (projeto == null)
                {
                    return BadRequest(new { mensagem = "Dados do projeto inválidos." });
                }

                _projetoRepository.Cadastrar(projeto);

                // Retorna 201 Created com a URL do novo recurso e o objeto no corpo
                return CreatedAtAction(nameof(BuscarPorId), new { id = projeto.Id }, projeto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Erro ao cadastrar o projeto.", detalhe = ex.Message });
            }
        }

        // PUT: api/projetos/1
        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Projeto projeto)
        {
            try
            {
                Projeto projetoExistente = _projetoRepository.BuscarPorId(id);
                if (projetoExistente == null)
                {
                    return NotFound(new { mensagem = $"Projeto com ID {id} não foi encontrado para atualização." });
                }

                _projetoRepository.Atualizar(id, projeto);
                return NoContent(); // HTTP 204 No Content
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Erro ao atualizar o projeto.", detalhe = ex.Message });
            }
        }

        // DELETE: api/projetos/1
        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            try
            {
                Projeto projetoExistente = _projetoRepository.BuscarPorId(id);
                if (projetoExistente == null)
                {
                    return NotFound(new { mensagem = $"Projeto com ID {id} não foi encontrado para exclusão." });
                }

                _projetoRepository.Deletar(id);
                return Ok(new { mensagem = "Projeto removido com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Erro ao deletar o projeto.", detalhe = ex.Message });
            }
        }
    }
}