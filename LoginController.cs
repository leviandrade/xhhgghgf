using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Sindiveg.API.BLL;
using Sindiveg.API.Models;

namespace Sindiveg.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Login")]
    public class LoginController : Controller
    {
        private TokenConfigurations tokenConfigurations;
        private SigningConfigurations signingConfigurations;
        private LoginBLL uBLL;
        private Handler handler;

        public LoginController(TokenConfigurations tokenConfigurations, SigningConfigurations signingConfigurations, Handler handler, LoginBLL uBLL)
        {
            this.tokenConfigurations = tokenConfigurations;
            this.signingConfigurations = signingConfigurations;
            this.uBLL = uBLL;
            this.handler = handler;
        }

        [ProducesResponseType(typeof(Token), 200)]
        [ProducesResponseType(401)]
        [HttpPost]
        public IActionResult Post([FromBody]LoginInfo loginInfo)
        {
            return handler.Handle(this, () =>
            {
                Usuario usuario = uBLL.Autenticar(loginInfo.login, loginInfo.senha);

                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")));

                claims.Add(new Claim("Empresas", usuario.Empresas));
                claims.Add(new Claim("TipoOcorrencia", usuario.TipoOcorrencia));
                claims.Add(new Claim("idUsuario", usuario.id.ToString()));
                claims.Add(new Claim("idEmpresa", usuario.idEmpresa.ToString()));
                claims.Add(new Claim("Sindiveg", usuario.Sindiveg.ToString()));

                ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity(usuario.Login),
                    claims
                );

                DateTime dataCriacao = DateTime.Now;
                DateTime dataExpiracao = dataCriacao.AddMinutes(tokenConfigurations.Minutes);

                var handler = new JwtSecurityTokenHandler();
                var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = tokenConfigurations.Issuer,
                    Audience = tokenConfigurations.Audience,
                    SigningCredentials = signingConfigurations.SigningCredentials,
                    Subject = identity,
                    NotBefore = dataCriacao,
                    Expires = dataExpiracao
                });
                var token = handler.WriteToken(securityToken);

                return new Token
                {
                    authenticated = true,
                    created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                    expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                    accessToken = token,
                    message = "OK"
                };
            });

        }
    }
}