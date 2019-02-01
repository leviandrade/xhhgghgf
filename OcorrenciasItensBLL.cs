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
    public class OcorrenciasItensBLL : IBLL
    {
        private DbServer dc;
        OcorrenciasItensValidacao Validacao = new OcorrenciasItensValidacao();

        public OcorrenciasItensBLL(DbServer dc)
        {
            this.dc = dc;
        }

        public int TipoOcorrenciaItem(int id)
        {
            var str = new StringBuilder()
            .Append("select TipoOcorrencia from Ocorrencias ")
            .Append("inner join OcorrenciasItens on OcorrenciasItens.idOcorrencia = Ocorrencias.id ")
            .Append("where Ocorrencias.Excluido = 0 and OcorrenciasItens.Excluido = 0 and OcorrenciasItens.id = @id ");

            var Parametros = new Dictionary<string, object>();
            Parametros.Add("id", id);

            var TipoOcorrencia = dc.Consultar<Tipo>(str.ToString(), Parametros);

            return TipoOcorrencia.FirstOrDefault().TipoOcorrencia;
        }

        public IEnumerable<OcorrenciaItens> Listar(Ocorrencias ocorrencia)
        {
            var parametros = new Dictionary<string, object>();
            var str = new StringBuilder()
           .Append("select o.id,o.idProduto,o.idDenunciado,o.idOcorrencia,o.idEmpresa,Ocorrencias.TipoOcorrencia, ")
           .Append("o.ComplementoLogradouro, o.ModoOperacao, o.data, o.idLogradouro, o.idLocalRoubo, o.Descricao, o.idCategoria, o.Lote, ")
           .Append("Produtos.Nome as Produto,Denunciados.nome as Denunciado,o.Damage,o.Litros ")
           .Append("from OcorrenciasItens as o left join Produtos on Produtos.id = o.idProduto ")
           .Append("left join Denunciados on Denunciados.id = o.idDenunciado ")
           .Append("left join Empresas on Produtos.idEmpresaProduto = Empresas.id ")
           .Append("inner join Ocorrencias on o.idOcorrencia = Ocorrencias.id ")
           .Append("where o.Excluido = 0 ")
           .Append("and o.idOcorrencia = @idOcorrencia");

            parametros.Add("idOcorrencia", ocorrencia.id);

            return dc.Consultar<OcorrenciaItens>(str.ToString(), parametros).ToList();
        }

        public IEnumerable<OcorrenciaItens> Lista(bool Sindiveg, List<int> Empresas, List<int> TipoOcorrencia)
        {
            var str = new StringBuilder()
            .Append("select o.id,o.idProduto,o.idDenunciado,o.idOcorrencia,o.idEmpresa,Ocorrencias.TipoOcorrencia, ")
            .Append("Produtos.Nome as Produto,Denunciados.nome as Denunciado,o.Damage,o.Litros ")
            .Append("from OcorrenciasItens as o left join Produtos on Produtos.id = o.idProduto ")
            .Append("left join Denunciados on Denunciados.id = o.idDenunciado ")
            .Append("left join Empresas on Produtos.idEmpresaProduto = Empresas.id ")
            .Append("left join Ocorrencias on o.idOcorrencia = Ocorrencias.id ")
            .Append("where o.Excluido = 0 ");

            var lista = dc.Consultar<OcorrenciaItens>(str.ToString()).ToList().OrderByDescending(x=>x.id);

            if (Sindiveg)
                return lista;
            else
                return lista.Where(x => Empresas.Contains(x.idEmpresa)).Where(x => TipoOcorrencia.Contains(x.TipoOcorrencia));
        }

        public OcorrenciasItens Selecionar(int id, bool Sindiveg, List<int> Empresas, List<int> TipoOcorrencia, string path)
        {
            var anexos = new AnexosBLL(String.Format("{0}{1}{2}", path, "\\wwwroot\\Anexos\\OcorrenciasItens\\", id)).ListarArquivos(id);

            int Tipo = TipoOcorrenciaItem(id);
            var ocorrenciaItem = dc.Selecionar<OcorrenciasItens>(id);
            ocorrenciaItem.anexos = anexos;

            if (Sindiveg || (Empresas.Contains((int)ocorrenciaItem.idEmpresa) && TipoOcorrencia.Contains(Tipo)))
                return ocorrenciaItem;
            else
                return null;
        }

        public OcorrenciasItens Incluir(OcorrenciasItens OcorrenciaItem)
        {
            Validacao.Validacao(OcorrenciaItem);
            OcorrenciaItem.id = dc.Inserir(OcorrenciaItem);

            foreach (var item in OcorrenciaItem.anexos)
            {
                item.idOcorrenciaItem = OcorrenciaItem.id;
                item.data = DateTime.Today.ToString();
            }

            var anexosBLL = new AnexosBLL(String.Format("{0}{1}{2}", OcorrenciaItem.path, "\\wwwroot\\Anexos\\OcorrenciasItens\\", OcorrenciaItem.id));

            anexosBLL.Salvar(OcorrenciaItem.anexos);
            return OcorrenciaItem;
        }

        public OcorrenciasItens Atualizar(OcorrenciasItens OcorrenciaItem, bool Sindiveg, List<int> Empresas, List<int> TipoOcorrencia, string path)
        {
            int Tipo = TipoOcorrenciaItem(OcorrenciaItem.id);
            if (Sindiveg || (Empresas.Contains((int)OcorrenciaItem.idEmpresa) && TipoOcorrencia.Contains(Tipo)))
            {
                Validacao.Validacao(OcorrenciaItem);
                dc.Atualizar(OcorrenciaItem, OcorrenciaItem.id);
            }

            var anexosBLL = new AnexosBLL(String.Format("{0}{1}{2}", path, "\\wwwroot\\Anexos\\OcorrenciasItens\\", OcorrenciaItem.id));

            anexosBLL.Salvar(OcorrenciaItem.anexos);


            return OcorrenciaItem;
        }

        public void Excluir(int id, bool Sindiveg, List<int> Empresas, List<int> TipoOcorrencia)
        {
            var OcorrenciaItem = dc.Selecionar<OcorrenciasItens>(id);
            int Tipo = TipoOcorrenciaItem(id);
            if (Sindiveg || (Empresas.Contains((int)OcorrenciaItem.idEmpresa) && TipoOcorrencia.Contains(Tipo)))
                dc.Excluir<OcorrenciasItens>(id);
        }

        //public IEnumerable<combo> ListaCombo()
        //{
        //    var str = new StringBuilder()
        //    .Append("select id as idDenunciante,Nome as DescricaoDenunciante ")
        //    .Append("from Denunciantes where excluido = 0 ");

        //    var ListaDenunciantes = dc.Consultar<combo>(str.ToString()).ToList().OrderBy(x => x.DescricaoDenunciante);

        //    str.Clear();

        //    str = new StringBuilder()
        //    .Append("select id as idDenunciado,Nome as DescricaoDenunciado ")
        //    .Append("from Denunciados where excluido = 0");

        //    var ListaDenunciados = dc.Consultar<combo>(str.ToString()).ToList().OrderBy(x => x.DescricaoDenunciado);

        //    str.Clear();

        //    str = new StringBuilder()
        //    .Append("select id as idProduto,Nome as DescricaoProduto ")
        //    .Append("from Produtos where excluido = 0");

        //    var ListaProdutos = dc.Consultar<combo>(str.ToString()).ToList().OrderBy(x => x.DescricaoProduto);

        //    var ListaRetorno = new List<combo>();
        //    ListaRetorno.AddRange(ListaDenunciantes);
        //    ListaRetorno.AddRange(ListaDenunciados);
        //    ListaRetorno.AddRange(ListaProdutos);

        //    return ListaRetorno;
        //}

    }
}
