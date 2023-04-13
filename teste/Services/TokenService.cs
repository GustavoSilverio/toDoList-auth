using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using teste.Models;

namespace teste.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _secretKey;

            //MUDAR ESSE SECRET, É LITERALMENTE UM TEXTO EM BASE64, COLOCAR UMA CADEIA DE
            //CARACTERES ALEATORIOS, E COLOCAR ISSO NUMA ENVIRONMENT_VARIABLE PARA AUMENTAR A SEGURANÇA
        public TokenService(string secretKey = "U09UWlpaX1NFQ1JFVA==")
        {
            _secretKey = secretKey;
        }

        public string GerarToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.Nome),
                    new Claim(ClaimTypes.Email, usuario.Email),
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
