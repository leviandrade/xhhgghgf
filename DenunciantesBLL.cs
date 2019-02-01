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
    public class DenunciantesBLL : IBLL
    {
        private DbServer dc;
        DenunciantesValidacao Validacao = new DenunciantesValidacao();

        public DenunciantesBLL(DbServer dc)
        {
            this.dc = dc;
        }

        public IEnumerable<Denunciantes> Lista()
        {
            return dc.Lista<Denunciantes>();
        }

        public IEnumerable<Denunciante> ListaDenunciantes(bool Sindiveg, List<int> Empresas)
        {
            var str = new StringBuilder()
                .Append("select d.id,d.idTipoDenunciante,d.idLocal,d.Nome,d.TipoDocumento,d.NumeroDocumento, td.Descricao as DescricaoLocal, ")
                .Append("l.Cidade ,d.idEmpresa,l.Estado from Denunciantes as d inner join TiposDenunciantes as td ")
                .Append("on td.id = d.idTipoDenunciante inner join Locais as l on l.id = d.idLocal ")
                .Append("where d.Excluido = 0 ");

            var lista = dc.Consultar<Denunciante>(str.ToString()).ToList();

            var ListaRetorno = new List<Denunciante>();
            foreach (var item in lista)
            {
                var itemRetorno = new Denunciante
                {
                    id = item.id,
                    idTipoDenunciante = item.idTipoDenunciante,
                    idLocal = item.idLocal,
                    Nome = item.Nome,
                    NumeroDocumento = item.NumeroDocumento != null ? item.NumeroDocumento : string.Empty,
                    DescricaoLocal = item.DescricaoLocal,
                    Cidade = item.Cidade,
                    Estado = item.Estado,
                    idEmpresa = item.idEmpresa,
                    Documento = item.TipoDocumento != null ? Enum.GetName(typeof(Sindiveg.API.Enumeradores.TipoDocumento), item.TipoDocumento) : string.Empty
                };
                ListaRetorno.Add(itemRetorno);
            };
            if (Sindiveg)
                return ListaRetorno;
            else
                return ListaRetorno.Where(x => Empresas.Contains(x.idEmpresa));
        }

        public Denunciantes Selecionar(int id)
        {
            var denunciante = dc.Selecionar<Denunciantes>(id);
            return denunciante;
        }

        public Denunciantes Selecionar(int id, bool Sindiveg, List<int> Empresas)
        {
            var Denunciante = dc.Selecionar<Denunciantes>(id);
            if (Sindiveg || Empresas.Contains((int)Denunciante.idEmpresa))
                return Denunciante;
            else
                return null;
        }

        public Denunciantes Incluir(Denunciantes Denunciante)
        {
            Validacao.Validacao(Denunciante);
            Denunciante.id = dc.Inserir(Denunciante);

            return Denunciante;
        }

        public Denunciantes Atualizar(int id, Denunciantes Denunciante, bool Sindiveg, List<int> Empresas)
        {
            if (Sindiveg || Empresas.Contains((int)Denunciante.idEmpresa))
            {
                Validacao.Validacao(Denunciante);
                dc.Atualizar(Denunciante, id);
            }

            return Denunciante;
        }

        public void Excluir(int id, bool Sindiveg, List<int> Empresas)
        {
            var Denunciante = dc.Selecionar<Denunciantes>(id);
            if (Sindiveg || Empresas.Contains((int)Denunciante.idEmpresa))
                dc.Excluir<Denunciantes>(id);
        }

        public IEnumerable<ComboDenunciantes> ListaCombo()
        {
            var str = new StringBuilder()
                .Append("select id as idTipoDenunciante,Descricao as DescricaoTipoDenunciante ")
                .Append("from TiposDenunciantes where Excluido = 0 ");

            var ListaDenunciados = dc.Consultar<ComboDenunciantes>(str.ToString()).ToList().OrderBy(x => x.DescricaoTipoDenunciante);

            str.Clear();

            str = new StringBuilder()
               .Append("Select id as idLocal, Cidade, Estado from Locais where Excluido = 0");

            var ListaLocais = dc.Consultar<ComboDenunciantes>(str.ToString()).ToList().OrderBy(x => x.Cidade);

            var ListaLocaisFormatados = new List<ComboDenunciantes>();
            foreach (var item in ListaLocais)
            {
                ComboDenunciantes ComboDenunciados = new ComboDenunciantes();

                ComboDenunciados.idLocal = item.idLocal;
                ComboDenunciados.DescricaoLocal = item.Cidade + " - " + item.Estado;

                ListaLocaisFormatados.Add(ComboDenunciados);
            }

            var ListaRetorno = new List<ComboDenunciantes>();

            ListaRetorno.AddRange(ListaDenunciados);
            ListaRetorno.AddRange(ListaLocaisFormatados);

            return ListaRetorno;

        }

    }
}
