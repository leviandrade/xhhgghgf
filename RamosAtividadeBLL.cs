using Digiexpress.ADO;
using Sindiveg.API.Banco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sindiveg.API.BLL
{
    public class RamosAtividadeBLL : IBLL
    {
        private DbServer dc;

        public RamosAtividadeBLL(DbServer dc)
        {
            this.dc = dc;
        }

        public IEnumerable<RamosAtividade> Lista(bool Sindiveg, List<int> Empresas)
        {
            if (Sindiveg)
                return dc.Lista<RamosAtividade>();
            else
                return dc.Lista<RamosAtividade>().Where(x => Empresas.Contains((int)x.idEmpresa));
        }

        public RamosAtividade Selecionar(int id, bool Sindiveg, List<int> Empresas)
        {
            var RamosAtividade = dc.Selecionar<RamosAtividade>(id);
            if (Sindiveg || Empresas.Contains((int)RamosAtividade.idEmpresa))
                return RamosAtividade;
            else
                return null;
        }

        public RamosAtividade Incluir(RamosAtividade RamoAtividade)
        {
            RamoAtividade.id = dc.Inserir(RamoAtividade);

            return RamoAtividade;
        }

        public RamosAtividade Atualizar(int id, RamosAtividade RamoAtividade, bool Sindiveg, List<int> Empresas)
        {
            if (Sindiveg || Empresas.Contains((int)RamoAtividade.idEmpresa))
                dc.Atualizar(RamoAtividade, id);

            return RamoAtividade;
        }

        public void Excluir(int id, bool Sindiveg, List<int> Empresas)
        {
            var RamosAtividade = dc.Selecionar<RamosAtividade>(id);
            if (Sindiveg || Empresas.Contains((int)RamosAtividade.idEmpresa))
                dc.Excluir<RamosAtividade>(id);
        }
    }
}
