using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            var tarefaEncontrada = _context.Tarefas.Find(id);

            if (tarefaEncontrada == null)
                return NotFound($"Id '{id}' não enontrado!");

            return Ok(tarefaEncontrada);
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            var tarefas = _context.Tarefas.ToList();
            return Ok(tarefas);
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            var tarefasEncontradas = _context.Tarefas.Where(t => t.Titulo.Contains(titulo));
            return Ok(tarefasEncontradas);
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefasEncontradas = _context.Tarefas.Where(x => x.Data.Date == data.Date);
            return Ok(tarefasEncontradas);
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            var tarefasEncontradas = _context.Tarefas.Where(x => x.Status == status);
            return Ok(tarefasEncontradas);
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            _context.Add(tarefa);
            _context.SaveChanges();
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            var tarefaEncontrada = _context.Tarefas.Find(id);

            if (tarefaEncontrada == null)
                return NotFound($"Id '{id}' não enontrado!");

            // if (tarefa.Data == DateTime.MinValue)
            //     if (tarefaEncontrada.Data == DateTime.MinValue)
            //         return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            // TODO: Atualizar as informações da variável tarefaBanco com a tarefa recebida via parâmetro
            tarefaEncontrada.Titulo = !tarefa.Titulo.IsNullOrEmpty() ? tarefa.Titulo : tarefaEncontrada.Titulo;
            tarefaEncontrada.Descricao = !tarefa.Descricao.IsNullOrEmpty() ? tarefa.Descricao : tarefaEncontrada.Descricao;
            tarefaEncontrada.Data = !(tarefa.Data == DateTime.MinValue) ? tarefa.Data : tarefaEncontrada.Data;
            tarefaEncontrada.Status = tarefa.Status.HasValue ? tarefa.Status : tarefaEncontrada.Status;

            _context.Update(tarefaEncontrada);
            _context.SaveChanges();
            // TODO: Atualizar a variável tarefaBanco no EF e salvar as mudanças (save changes)
            return Ok(tarefaEncontrada);
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefaEncontrada = _context.Tarefas.Find(id);

            if (tarefaEncontrada == null)
                return NotFound($"Id '{id}' não enontrado!");

            // TODO: Remover a tarefa encontrada através do EF e salvar as mudanças (save changes)
            _context.Remove(tarefaEncontrada);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
