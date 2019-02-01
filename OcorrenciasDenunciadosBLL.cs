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
    public class OcorrenciasDenunciadosBLL : IBLL
    {
        private DbServer dc;

        public OcorrenciasDenunciadosBLL(DbServer dc)
        {
            this.dc = dc;
        }

        public IEnumerable<OcorrenciasDenunciados> Lista()
        {
            return dc.Lista<OcorrenciasDenunciados>();
        }

        public IEnumerable<OcorrenciaDenunciado> Selecionar(int id)
        {
            var str = new StringBuilder()
                .Append("select o.id,o.idDenunciado,o.idOcorrencia,Denunciados.Nome as NomeDenunciado ")
                .Append("from OcorrenciasDenunciados as o ")
                .Append("inner join Denunciados on o.idDenunciado = Denunciados.id ")
                .Append("where o.excluido = 0 and o.idOcorrencia = @id");

            var parametros = new Dictionary<string, object>();
            parametros.Add("@id", id);

            var lista = dc.Consultar<OcorrenciaDenunciado>(str.ToString(), parametros).ToList();

            return lista;
        }

        public OcorrenciasDenunciados Incluir(OcorrenciasDenunciados OcorrenciaDenunciado)
        {
            OcorrenciaDenunciado.id = dc.Inserir(OcorrenciaDenunciado);

            return OcorrenciaDenunciado;
        }

        public OcorrenciasDenunciados Atualizar(int id, OcorrenciasDenunciados OcorrenciaDenunciado)
        {
            dc.Atualizar(OcorrenciaDenunciado, id);

            return OcorrenciaDenunciado;
        }

        public void Excluir(int id)
        {
            dc.Excluir<OcorrenciasDenunciados>(id);
        }
    }
}
