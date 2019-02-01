using Digiexpress.ADO;
using Sindiveg.API.Banco;
using Sindiveg.API.Models;
using Sindiveg.API.Validacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindiveg.API.BLL
{
    public class DenunciadosBLL : IBLL
    {
        private DbServer dc;
        DenunciadosValidacao Validacao = new DenunciadosValidacao();

        public DenunciadosBLL(DbServer dc)
        {
            this.dc = dc;
        }

        public IEnumerable<Denunciados> Lista()
        {
            return dc.Lista<Denunciados>();
        }

        public IEnumerable<Denunciado> ListaDenunciados(bool Sindiveg, List<int> Empresas)
        {
            var str = new StringBuilder()
                .Append("select d.id,d.idTipoDenunciado,idLocal, d.Nome,d.TipoDocumento,d.NumeroDocumento,td.Descricao as DescricaoLocal, ")
                .Append("l.Cidade,d.idEmpresa, l.Estado from Denunciados as d inner join TiposDenunciados as td ")
                .Append("on td.id = d.idTipoDenunciado inner join Locais as l on l.id = d.idLocal ")
                .Append("where d.Excluido = 0 ");

            var lista = dc.Consultar<Denunciado>(str.ToString()).ToList();

            var ListaRetorno = new List<Denunciado>();
            foreach (var item in lista)
            {
                var itemRetorno = new Denunciado
                {
                    id = item.id,
                    idTipoDenunciado = item.idTipoDenunciado,
                    idLocal = item.idLocal,
                    Nome = item.Nome,
                    NumeroDocumento = item.NumeroDocumento != null ? item.NumeroDocumento : string.Empty,
                    DescricaoLocal = item.DescricaoLocal,
                    Cidade = item.Cidade,
                    Estado = item.Estado,
                    idEmpresa = item.idEmpresa,
                    idDenunciado = item.idDenunciado,
                    Documento = item.TipoDocumento != null ? Enum.GetName(typeof(Sindiveg.API.Enumeradores.TipoDocumento), item.TipoDocumento) : string.Empty
                };
                ListaRetorno.Add(itemRetorno);
            };
            if (Sindiveg)
                return ListaRetorno;
            else
                return ListaRetorno.Where(x => Empresas.Contains(x.idEmpresa));
        }

        public Denunciados Selecionar(int id)
        {
            var Denunciado = dc.Selecionar<Denunciados>(id);
            return Denunciado;
        }

        public Denunciados Selecionar(int id, bool Sindiveg, List<int> Empresas)
        {
            var Denunciado = dc.Selecionar<Denunciados>(id);
            if (Sindiveg || Empresas.Contains((int)Denunciado.idEmpresa))
                return Denunciado;
            else
                return null;
        }

        public Denunciados Incluir(Denunciados Denunciado)
        {
            Validacao.Validacao(Denunciado);
            Denunciado.id = dc.Inserir(Denunciado);

            return Denunciado;
        }

        public Denunciados Atualizar(int id, Denunciados Denunciado, bool Sindiveg, List<int> Empresas)
        {
            if (Sindiveg || Empresas.Contains((int)Denunciado.idEmpresa))
            {
                Validacao.Validacao(Denunciado);
                dc.Atualizar(Denunciado, id);
            }

            return Denunciado;
        }

        public void Excluir(int id, bool Sindiveg, List<int> Empresas)
        {
            var Denunciado = dc.Selecionar<Denunciados>(id);
            if (Sindiveg || Empresas.Contains((int)Denunciado.idEmpresa))
                dc.Excluir<Denunciados>(id);
        }

        public IEnumerable<ComboDenunciados> ListaCombo()
        {
            var str = new StringBuilder()
                .Append("select id as idTipoDenunciado,Descricao as DescricaoTipoDenunciado ")
                .Append("from TiposDenunciados where Excluido = 0 ");

            var ListaDenunciados = dc.Consultar<ComboDenunciados>(str.ToString()).ToList().OrderBy(x => x.DescricaoTipoDenunciado);

            str.Clear();

            str = new StringBuilder()
               .Append("Select id as idLocal, Cidade, Estado from Locais where Excluido = 0");

            var ListaLocais = dc.Consultar<ComboDenunciados>(str.ToString()).ToList().OrderBy(x => x.Cidade);

            var ListaLocaisFormatados = new List<ComboDenunciados>();
            foreach (var item in ListaLocais)
            {
                ComboDenunciados ComboDenunciados = new ComboDenunciados();

                ComboDenunciados.idLocal = item.idLocal;
                ComboDenunciados.DescricaoLocal = item.Cidade + " - " + item.Estado;

                ListaLocaisFormatados.Add(ComboDenunciados);
            }

            var ListaRetorno = new List<ComboDenunciados>();

            ListaRetorno.AddRange(ListaDenunciados);
            ListaRetorno.AddRange(ListaLocaisFormatados);

            return ListaRetorno;

        }
    }
}
