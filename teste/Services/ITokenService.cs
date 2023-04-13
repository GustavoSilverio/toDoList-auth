using teste.Models;

namespace teste.Services
{
    public interface ITokenService
    {
        public string GerarToken(Usuario usuario);
    }
}
