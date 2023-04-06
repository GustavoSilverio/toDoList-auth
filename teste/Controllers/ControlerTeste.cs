using Microsoft.AspNetCore.Mvc;
using teste.Models;
using teste.Repositories;

namespace teste.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ControlerTeste : ControllerBase
    {

        private readonly IRepositiory _repository;

        public ControlerTeste(IRepositiory repository)
        {
            _repository = repository;
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetAll()
        {
            var usuarios = _repository.GetAll();
            return Ok(usuarios);
        }

        [HttpGet("get-all-tasks/{idUsuario}")]
        public async Task<ActionResult<IEnumerable<Tarefa>>> GetAllTasks(int idUsuario)
        {
            var tasks = _repository.GetAllTasks(idUsuario);
            return Ok(tasks);
        }

        [HttpPost("add-usuario")]
        public async Task<ActionResult<Usuario>> AddUsuario(Usuario usuario)
        {
            try
            {
                await _repository.AddUser(usuario);
                return Ok(usuario);
            }
            catch (Exception ex) { 

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add-task")]
        public async Task<ActionResult<Tarefa>> AddTarefa(Tarefa tarefa)
        {
            try
            {
                await _repository.AddTask(tarefa);
                return Ok(tarefa);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("logar")]
        public async Task<ActionResult<Usuario>> Logar(Usuario usuario)
        {
            Usuario usuarioLogado = await _repository.Logar(usuario);
            return Ok(usuarioLogado);
        }
    }

}
