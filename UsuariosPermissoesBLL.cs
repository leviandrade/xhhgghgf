using Digiexpress.ADO;
using Sindiveg.API.Banco;
using Sindiveg.API.Enumeradores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindiveg.API.BLL
{
    public class UsuariosPermissoesBLL : IBLL
    {
        private DbServer dc;

        public UsuariosPermissoesBLL(DbServer dc)
        {
            this.dc = dc;
        }

        public IEnumerable<UsuariosPermissoes> Lista()
        {
            return dc.Lista<UsuariosPermissoes>();
        }

        public UsuariosPermissoes Selecionar(int id)
        {
            var permissao = dc.Selecionar<UsuariosPermissoes>(id);

            return permissao;
        }

        public UsuariosPermissoes SelecionarIdUsuario(int id)
        {
            var consulta = new StringBuilder();
            var parametros = new Dictionary<string, object>();
            consulta.Append("select * from usuariosPermissoes where idUsuario = @idUsuario");
            parametros.Add("idUsuario", id);
            return dc.Consultar<UsuariosPermissoes>(consulta.ToString(), parametros).FirstOrDefault();
        }

        public UsuariosPermissoes Incluir(UsuariosPermissoes permissoes)
        {
            permissoes.id = dc.Inserir(permissoes);

            return permissoes;
        }



        public UsuariosPermissoes Atualizar(int id, UsuariosPermissoes permissoes)
        {
            dc.Atualizar(permissoes, id);

            return permissoes;
        }

        public void Atualizar(UsuariosPermissoes permissoes)
        {
            var permissoesBD = SelecionarIdUsuario(permissoes.idUsuario);

            if (permissoesBD != null)
            {
                permissoesBD.Empresas = permissoes.Empresas;
                permissoesBD.TipoOcorrencia = permissoes.TipoOcorrencia;
                permissoesBD.Menus = permissoes.Menus;
                Atualizar(permissoesBD.id, permissoesBD);
            }
        }

        public void Excluir(int id)
        {
            dc.Excluir<UsuariosPermissoes>(id);
        }

        public object ListaPermissoesMenu(int idUsuario)
        {
            var str = new StringBuilder()
               .Append("select *from UsuariosPermissoes where idUsuario = @idUsuario");
            var parametros = new Dictionary<string, object>();
            parametros.Add("idUsuario", idUsuario);
            var Usuario = dc.Consultar<UsuariosPermissoes>(str.ToString(), parametros).FirstOrDefault();

            var permissoesUsuarios = Usuario.Menus.Split(",");
            var permissoes = Enum.GetValues(typeof(Menus)).Cast<int>().ToList();
            var ListaRetorno = new List<string>();
            foreach (var item in permissoesUsuarios)
            {
                if (permissoes.Contains(Convert.ToInt32(item)))
                {
                    var NomeMenu = Enum.GetName(typeof(Menus), Convert.ToInt32(item));
                    ListaRetorno.Add(NomeMenu);
                }
            }

            return new object[] {ListaRetorno };
        }
    }
}
