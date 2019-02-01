using Digiexpress.ADO;
using Sindiveg.API.Banco;
using Sindiveg.API.Validacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sindiveg.API.BLL
{
    public class TiposDenunciantesBLL : IBLL
    {
        private DbServer dc;
        TiposDenunciantesValidacao Validacao = new TiposDenunciantesValidacao();

        public TiposDenunciantesBLL(DbServer dc)
        {
            this.dc = dc;
        }

        public IEnumerable<TiposDenunciantes> Lista(bool Sindiveg, List<int> Empresas)
        {

            if (Sindiveg)
                return dc.Lista<TiposDenunciantes>();
            else
                return dc.Lista<TiposDenunciantes>().Where(x => Empresas.Contains((int)x.idEmpresa));
        }

        public TiposDenunciantes Selecionar(int id, bool Sindiveg, List<int> Empresas)
        {
            var TipoDenunciante = dc.Selecionar<TiposDenunciantes>(id);
            if (Sindiveg || Empresas.Contains((int)TipoDenunciante.idEmpresa))
                return TipoDenunciante;
            else
                return null;
        }

        public TiposDenunciantes Incluir(TiposDenunciantes TipoDenunciante)
        {
            Validacao.Validacao(TipoDenunciante);
            TipoDenunciante.id = dc.Inserir(TipoDenunciante);

            return TipoDenunciante;
        }

        public TiposDenunciantes Atualizar(int id, TiposDenunciantes TipoDenunciante, bool Sindiveg, List<int> Empresas)
        {
            if (Sindiveg || Empresas.Contains((int)TipoDenunciante.idEmpresa))
            {
                Validacao.Validacao(TipoDenunciante);
                dc.Atualizar(TipoDenunciante, id);
            }
            return TipoDenunciante;
        }

        public void Excluir(int id, bool Sindiveg, List<int> Empresas)
        {
            var TipoDenunciante = dc.Selecionar<TiposDenunciantes>(id);
            if (Sindiveg || Empresas.Contains((int)TipoDenunciante.idEmpresa))
                dc.Excluir<TiposDenunciantes>(id);
        }
    }
}
