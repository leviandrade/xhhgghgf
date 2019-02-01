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
    public class OcorrenciasProdutoBLL :IBLL
    {
        private DbServer dc;

        public OcorrenciasProdutoBLL(DbServer dc)
        {
            this.dc = dc;
        }

        public IEnumerable<OcorrenciaProduto> Lista()
        {
            var str = new StringBuilder()
               .Append("select o.id,o.idProduto,o.Litros,o.Damage,o.idOcorrencia, ")
               .Append("Produtos.Nome as NomeProduto From OcorrenciasProduto as o ")
               .Append("inner join Produtos on Produtos.id = o.idProduto ")
               .Append("where o.Excluido = 0 ");

            return dc.Consultar<OcorrenciaProduto>(str.ToString()).ToList();
        }

        public IEnumerable<OcorrenciaProduto> Selecionar(int id)
        {
            var str = new StringBuilder()
              .Append("select o.id,o.idProduto,o.Litros,o.Damage,o.idOcorrencia, ")
              .Append("Produtos.Nome as NomeProduto From OcorrenciasProduto as o ")
              .Append("inner join Produtos on Produtos.id = o.idProduto ")
              .Append("where o.Excluido = 0 and o.idOcorrencia = @id");

            var parametros = new Dictionary<string, object>();
                parametros.Add("@id", id);

            return dc.Consultar<OcorrenciaProduto>(str.ToString(), parametros);
        }

        public OcorrenciasProduto Incluir(OcorrenciasProduto OcorrenciaProduto)
        {
            OcorrenciaProduto.id = dc.Inserir(OcorrenciaProduto);

            return OcorrenciaProduto;
        }

        public OcorrenciasProduto Atualizar(int id, OcorrenciasProduto OcorrenciaProduto)
        {
            dc.Atualizar(OcorrenciaProduto, id);

            return OcorrenciaProduto;
        }

        public void Excluir(int id)
        {
            dc.Excluir<OcorrenciasProduto>(id);
        }
    }
}
