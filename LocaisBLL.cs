using Digiexpress.ADO;
using Sindiveg.API.Banco;
using Sindiveg.API.Validacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sindiveg.API.BLL
{
    public class LocaisBLL : IBLL
    {
        private DbServer dc;
        LocaisValidacao Validacao = new LocaisValidacao();

        public LocaisBLL(DbServer dc)
        {
            this.dc = dc;
        }

        public IEnumerable<Locais> Lista(bool Sindiveg, List<int> Empresas)
        {
            if (Sindiveg)
                return dc.Lista<Locais>();
            else
                return dc.Lista<Locais>().Where(x => Empresas.Contains((int)x.idEmpresa));
        }

        public Locais Selecionar(int id)
        {
            var Local = dc.Selecionar<Locais>(id);
            return Local;
        }

        public Locais Selecionar(int id, bool Sindiveg, List<int> Empresas)
        {
            var Local = dc.Selecionar<Locais>(id);
            if (Sindiveg || Empresas.Contains((int)Local.idEmpresa))
                return Local;
            else
                return null;
        }

        public Locais Incluir(Locais Local)
        {
            Validacao.Validacao(Local);
            Local.id = dc.Inserir(Local);

            return Local;
        }

        public Locais Atualizar(int id, Locais Local, bool Sindiveg, List<int> Empresas)
        {
            if (Sindiveg || Empresas.Contains((int)Local.idEmpresa))
            {
                Validacao.Validacao(Local);
                dc.Atualizar(Local, id);
            }

            return Local;
        }

        public void Excluir(int id, bool Sindiveg, List<int> Empresas)
        {
            var Local = dc.Selecionar<Locais>(id);
            if (Sindiveg || Empresas.Contains((int)Local.idEmpresa))
                dc.Excluir<Locais>(id);
        }
    }
}
