using Digiexpress.ADO;
using Sindiveg.API.Banco;
using Sindiveg.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindiveg.API.BLL
{
    public class ConfiguracoesBLL : IBLL
    {
        private DbServer dc;

        public ConfiguracoesBLL(DbServer dc)
        {
            this.dc = dc;
        }

        public IEnumerable<Configuracoes> Lista()
        {
            return dc.Lista<Configuracoes>();
        }

        public Configuracoes Selecionar(int id)
        {
            var config = dc.Selecionar<Configuracoes>(id);
            return config;
        }

        public Configuracoes Incluir(Configuracoes config, UserInfo userInfo)
        {
            config.idUsuario = userInfo.idUsuario.Value;
            config.DataModificacao = DateTime.Now;

            config.id = dc.Inserir(config);
            return config;
        }

        public Configuracoes Atualizar(Configuracoes config, UserInfo userInfo)
        {
            config.idUsuario = userInfo.idUsuario.Value;
            config.DataModificacao = DateTime.Now;
            dc.Atualizar(config, config.id);
            return config;
        }

        public void Excluir(int id)
        {
            dc.Excluir<Configuracoes>(id);
        }
    }
}
