using System.Runtime.Serialization;

namespace teste.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string  Email { get; set; }
        public object? Tarefas { get; set; }
    }
}
