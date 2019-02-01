using Digiexpress.ADO;
using Sindiveg.API.Banco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sindiveg.API.BLL
{
    public class LogradourosBLL : IBLL
    {

        private DbServer dc;
        public LogradourosBLL(DbServer dc)
        {
            this.dc = dc;
        }
        public IEnumerable<Logradouros> Lista(bool Sindiveg, List<int> Empresas)
        {
            if (Sindiveg)
                return dc.Lista<Logradouros>();
            else
                return dc.Lista<Logradouros>().Where(x => Empresas.Contains((int)x.idEmpresa));

        }

        public Logradouros Selecionar(int id)
        {
            var Logradouro = dc.Selecionar<Logradouros>(id);
            return Logradouro;
        }

        public Logradouros Selecionar(int id, bool Sindiveg, List<int> Empresas)
        {
            var Logradouro = dc.Selecionar<Logradouros>(id);
            if (Sindiveg || Empresas.Contains((int)Logradouro.idEmpresa))
                return Logradouro;
            else
                return null;
        }

        public Logradouros Incluir(Logradouros Logradouro)
        {
            Logradouro.id = dc.Inserir(Logradouro);

            return Logradouro;
        }

        public Logradouros Atualizar(int id, Logradouros Logradouro, bool Sindiveg, List<int> Empresas)
        {
            if (Sindiveg || Empresas.Contains((int)Logradouro.idEmpresa))
                dc.Atualizar(Logradouro, id);

            return Logradouro;
        }
        public void Excluir(int id, bool Sindiveg, List<int> Empresas)
        {
            var Logradouro = dc.Selecionar<Logradouros>(id);
            if (Sindiveg || Empresas.Contains((int)Logradouro.idEmpresa))
                dc.Excluir<Logradouros>(id);
        }
    }
}
