using Digiexpress;
using Digiexpress.ADO;
using Sindiveg.API.Banco;
using Sindiveg.API.Enumeradores;
using Sindiveg.API.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindiveg.API.BLL
{
    public class UsuariosBLL : IBLL
    {
        private DbServer dc;


        public UsuariosBLL(DbServer dc)
        {
            this.dc = dc;
        }

        public IEnumerable<Usuarios> Lista()
        {
            return dc.Lista<Usuarios>();
        }

        public Usuarios Selecionar(int id)
        {
            var usuario = dc.Selecionar<Usuarios>(id);
            var permissoes = new UsuariosPermissoesBLL(dc).SelecionarIdUsuario(id);

            if (permissoes != null) { 
                usuario.PermissaoEmpresas = permissoes.Empresas;
                usuario.PermissaoTiposOcorrencias = permissoes.TipoOcorrencia;
            }

            return usuario;
        }

        public Usuarios Incluir(Usuarios usuario)
        {
            ValidaNovoUsuario(usuario);

            usuario.Senha = Serialization.Criptografar(Base64ToString(usuario.Senha));
            usuario.id = dc.Inserir(usuario);

            var permissoes = new UsuariosPermissoes();

            if (usuario.Sindiveg)
                permissoes.Menus = string.Join(',', (int)Menus.Usuarios,(int) Menus.Configuracoes, usuario.PermissaoMenus);
            else
                permissoes.Menus = usuario.PermissaoMenus;

            permissoes.idUsuario = usuario.id;
            permissoes.Empresas = usuario.PermissaoEmpresas;
            permissoes.TipoOcorrencia = usuario.PermissaoTiposOcorrencias;

            new UsuariosPermissoesBLL(dc).Incluir(permissoes);

            return usuario;
        }

        public Usuarios Atualizar(int id, Usuarios usuario)
        {
            var usuariosBD = Selecionar(id);

            if (String.IsNullOrWhiteSpace(usuario.Senha))
            {
                usuario.Senha = usuariosBD.Senha;
            }
            else
            {
                usuario.Senha = Serialization.Criptografar(Base64ToString(usuario.Senha));
            }

            dc.Atualizar(usuario, id);

            var permissoes = new UsuariosPermissoes();

            if (usuario.Sindiveg)
                permissoes.Menus = string.Join(',', (int)Menus.Usuarios, (int)Menus.Configuracoes, usuario.PermissaoMenus);
            else
                permissoes.Menus = usuario.PermissaoMenus;

            //else if (usuariosBD.Sindiveg && !usuario.Sindiveg)
            //{
            //    var permissoesUsuarios = usuario.PermissaoMenus.Split(",").ToArray();
            //    var retornoMenus = permissoesUsuarios.Skip((int)Menus.Usuarios);
            //    permissoes.Menus = string.Join(",", retornoMenus);
            //}



            permissoes.idUsuario = usuario.id;
            permissoes.Empresas = usuario.PermissaoEmpresas;
            permissoes.TipoOcorrencia = usuario.PermissaoTiposOcorrencias;

            new UsuariosPermissoesBLL(dc).Atualizar(permissoes);

            return usuario;
        }

        public void Excluir(int id)
        {
            dc.Excluir<Usuarios>(id);
        }

        private void ValidaNovoUsuario(Usuarios usuario)
        {
            var erros = new List<String>();
            var usuarioExistente = Lista().Where(x => x.Login.Equals(usuario.Login) && !x.Excluido).FirstOrDefault();

            if (usuarioExistente != null)
                erros.Add(String.Format("Já existe um usuáro com este nome de Login: {0}", usuario.Login));

            if (erros.Count() > 0)
                throw new ErroValidacao(erros);
        }

        private string Base64ToString(string base64)
        {
            byte[] data = Convert.FromBase64String(base64);
            return Encoding.UTF8.GetString(data);
        }
    }
}
