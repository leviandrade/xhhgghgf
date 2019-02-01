using Digiexpress.ADO;
using Sindiveg.API.Banco;
using Sindiveg.API.Validacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sindiveg.API.BLL
{
    public class TiposDenunciadosBLL : IBLL
    {
        private DbServer dc;
        TiposDenunciadosValidacao validacao = new TiposDenunciadosValidacao();

        public TiposDenunciadosBLL(DbServer dc)
        {
            this.dc = dc;
        }

        public IEnumerable<TiposDenunciados> Lista(bool Sindiveg, List<int> Empresas)
        {
            if (Sindiveg)
                return dc.Lista<TiposDenunciados>();
            else
                return dc.Lista<TiposDenunciados>().Where(x => Empresas.Contains((int)x.idEmpresa));
        }

        public TiposDenunciados Selecionar(int id, bool Sindiveg, List<int> Empresas)
        {
            var TipoDenunciado = dc.Selecionar<TiposDenunciados>(id);
            if (Sindiveg || Empresas.Contains((int)TipoDenunciado.idEmpresa))
                return TipoDenunciado;
            else
                return null;
        }

        public TiposDenunciados Incluir(TiposDenunciados TipoDenunciado)
        {
            validacao.Validacao(TipoDenunciado);
            TipoDenunciado.id = dc.Inserir(TipoDenunciado);

            return TipoDenunciado;
        }

        public TiposDenunciados Atualizar(int id, TiposDenunciados TipoDenunciado, bool Sindiveg, List<int> Empresas)
        {
            if (Sindiveg || Empresas.Contains((int)TipoDenunciado.idEmpresa))
            {
                validacao.Validacao(TipoDenunciado);
                dc.Atualizar(TipoDenunciado, id);
            }

            return TipoDenunciado;
        }

        public void Excluir(int id, bool Sindiveg, List<int> Empresas)
        {
            var TipoDenunciado = dc.Selecionar<TiposDenunciados>(id);
            if (Sindiveg || Empresas.Contains((int)TipoDenunciado.idEmpresa))
                dc.Excluir<TiposDenunciados>(id);
        }
    }
}
