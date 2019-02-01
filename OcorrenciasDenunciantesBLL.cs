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
    public class OcorrenciasDenunciantesBLL : IBLL
    {
        private DbServer dc;

        public OcorrenciasDenunciantesBLL(DbServer dc)
        {
            this.dc = dc;
        }

        public IEnumerable<OcorrenciasDenunciantes> Lista()
        {
            return dc.Lista<OcorrenciasDenunciantes>();
        }

        public IEnumerable<OcorrenciaDenunciante> Selecionar(int id)
        {
            var str = new StringBuilder()
                    .Append("select o.id,o.idDenunciante,o.idOcorrencia,Denunciantes.Nome as NomeDenunciante ")
                    .Append("from OcorrenciasDenunciantes as o ")
                    .Append("inner join Denunciantes on o.idDenunciante = Denunciantes.id ")
                    .Append("where o.excluido = 0 and o.idOcorrencia = @id");

            var parametros = new Dictionary<string, object>();
            parametros.Add("@id", id);

            var lista = dc.Consultar<OcorrenciaDenunciante>(str.ToString(), parametros).ToList();

            return lista;
        }

        public OcorrenciasDenunciantes Incluir(OcorrenciasDenunciantes OcorreniaDenunciante)
        {
            OcorreniaDenunciante.id = dc.Inserir(OcorreniaDenunciante);

            return OcorreniaDenunciante;
        }

        public OcorrenciasDenunciantes Atualizar(int id, OcorrenciasDenunciantes OcorreniaDenunciante)
        {
            dc.Atualizar(OcorreniaDenunciante, id);

            return OcorreniaDenunciante;
        }

        public void Excluir(int id)
        {
            dc.Excluir<OcorrenciasDenunciantes>(id);
        }
    }
}
