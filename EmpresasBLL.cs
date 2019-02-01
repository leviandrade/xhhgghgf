using Digiexpress.ADO;
using Sindiveg.API.Banco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sindiveg.API.BLL
{
    public class EmpresasBLL : IBLL
    {
        private DbServer dc;

        public EmpresasBLL(DbServer dc)
        {
            this.dc = dc;
        }

        public IEnumerable<Empresas> Lista(bool Sindiveg, List<int> Empresas)
        {
            if (Sindiveg)
                return dc.Lista<Empresas>();
            else
                return dc.Lista<Empresas>().Where(x => Empresas.Contains((int)x.idEmpresa) || Empresas.Contains(x.id));

        }

        public Empresas Selecionar(int id)
        {
            var empresa = dc.Selecionar<Empresas>(id);
            return empresa;
        }

        public Empresas Selecionar(int id, bool Sindiveg, List<int> Empresas)
        {
            var empresa = dc.Selecionar<Empresas>(id);
            if (Sindiveg || Empresas.Contains((int)empresa.idEmpresa))
                return empresa;
            else
                return null;
        }

        public Empresas Incluir(Empresas empresa)
        {
            empresa.id = dc.Inserir(empresa);

            return empresa;
        }

        public Empresas Atualizar(int id, Empresas empresa, bool Sindiveg, List<int> Empresas)
        {
            if (Sindiveg || Empresas.Contains((int)empresa.idEmpresa))
                dc.Atualizar(empresa, id);

            return empresa;
        }

        public void Excluir(int id, bool Sindiveg, List<int> Empresas)
        {
            var empresa = dc.Selecionar<Empresas>(id);
            if (Sindiveg || Empresas.Contains((int)empresa.idEmpresa))
                dc.Excluir<Empresas>(id);
        }
    }
}
