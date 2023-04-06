using teste.Models;

namespace teste.Repositories
{
    public interface IRepositiory
    {
        public IAsyncEnumerable<Usuario> GetAll();
        public IAsyncEnumerable<Tarefa> GetAllTasks(int idUsuario);
        public Task<Usuario> AddUser (Usuario usuario);
        public Task<Tarefa> AddTask (Tarefa tarefa);
        public Task<Usuario> Logar (Usuario usuario);
    }
}
