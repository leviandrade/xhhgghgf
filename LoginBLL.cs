using Digiexpress;
using Digiexpress.ADO;
using Sindiveg.API.Exceptions;
using Sindiveg.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindiveg.API.BLL
{
    public class LoginBLL : IBLL
    {
        private DbServer dc;

        public LoginBLL(DbServer dc)
        {
            this.dc = dc;
        }

        public Usuario Autenticar(string Login, string Senha)
        {
            if (Login == null && Senha == null)
                throw new LoginInvalido();

            Senha = Serialization.Criptografar(Base64ToString(Senha));

            var usuario = SelecionarUsuario(Login);

            if (usuario == null || usuario.Senha != Senha)
                throw new LoginInvalido();

            return usuario;
        }

        public Usuario SelecionarUsuario(string Login)
        {

            var parametros = new Dictionary<string, object>();

            var str = new StringBuilder()
               .Append("select * from UsuariosPermissoes inner join Usuarios ")
               .Append("on UsuariosPermissoes.idUsuario = Usuarios.id ")
               .Append("where Excluido = 0 and Login = @Login");

            parametros.Add("Login", Login);

            var Usuario = dc.Consultar<Usuario>(str.ToString(), parametros).FirstOrDefault();

            return Usuario;
        }


        private string Base64ToString(string base64)
        {
            byte[] data = Convert.FromBase64String(base64);
            return Encoding.UTF8.GetString(data);
        }
    }
}
