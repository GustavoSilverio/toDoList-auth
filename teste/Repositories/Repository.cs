using Dapper;
using System.Data.SqlClient;
using teste.Models;

namespace teste.Repositories
{
    public class Repository : IRepositiory
    {

        private readonly string _connectionString;

        public Repository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async IAsyncEnumerable<Usuario> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var usuarios = await connection.QueryAsync<Usuario>("SELECT * FROM Usuario");

                foreach (var usuario in usuarios)
                {
                    yield return usuario;
                }
            }
        }

        //FALTA VERIFICAR SE O USUARIO QUE ESTÁ TENTANDO ADICIONAR A TASK EXISTE, ANTES DE EXECUTAR A QUERY
        public async IAsyncEnumerable<Tarefa> GetAllTasks(int idUsuario)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var tarefas = await connection.QueryAsync<Tarefa>(@"SELECT * FROM Tarefas WHERE IdUsuario = @idUsuario", new { idUSuario = idUsuario });

                foreach (var tarefa in tarefas)
                {
                    yield return tarefa;
                }
            }

        }

        public async Task<Usuario> AddUser(Usuario usuario)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                if (usuario == null)
                {
                    throw new ArgumentNullException(nameof(usuario));
                }

                string sqlQuery = "INSERT INTO Usuario (Nome, Email) VALUES (@Nome, @Email);";

                var id = await connection.QuerySingleOrDefaultAsync<int>(sqlQuery, usuario);

                usuario.Id = id;

                return usuario;
            }
        }

        public async Task<Tarefa> AddTask(Tarefa tarefa)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                if (tarefa == null)
                {
                    throw new ArgumentNullException(nameof(tarefa));
                }

                string sqlQuery = "INSERT INTO Tarefas (IdUsuario, Titulo, Descricao) VALUES (@IdUsuario, @Titulo, @Descricao)";

                var id = await connection.QuerySingleOrDefaultAsync<int>(sqlQuery, tarefa);

                tarefa.Id = id;

                return tarefa;
            }
        }

        public async Task<Usuario> Logar(Usuario usuario)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                try
                {
                    if (usuario == null)
                    {
                        throw new ArgumentNullException(nameof(usuario));
                    }

                    Usuario usuarioRetornado = await connection.QueryFirstOrDefaultAsync<Usuario>(@"SELECT * FROM Usuario WHERE Email = @Email", new { Email = usuario.Email });

                    if (usuarioRetornado != null)
                    {
                        if (usuarioRetornado.Nome != usuario.Nome)
                            throw new Exception("Nome de usuário errado!");

                        if (usuarioRetornado.Email != usuario.Email)
                            throw new Exception("Email do usuário errado!");

                        var tarefasUsuario = GetAllTasks(usuarioRetornado.Id);

                        if(tarefasUsuario == null)
                            throw new Exception("Erro em trazer tarefas do usuário");

                        usuarioRetornado.Tarefas = tarefasUsuario;

                        return usuarioRetornado;

                    }else
                    { throw new Exception("Usuario não existe!"); }

                } catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }
        }
    }
}
