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
    public class ProdutosBLL : IBLL
    {
        private DbServer dc;
        ProdutosValidacao validacao = new ProdutosValidacao();

        public ProdutosBLL(DbServer dc)
        {
            this.dc = dc;
        }

        public IEnumerable<Produtos> Lista()
        {
            return dc.Lista<Produtos>();
        }

        public IEnumerable<Produto> ListaProdutos(bool Sindiveg, List<int> Empresas)
        {
            var str = new StringBuilder()
                .Append("select p.id,p.idEmpresaProduto,idClasseProduto, p.Nome,E.RazaoSocial,p.idEmpresa, ")
                .Append("c.Descricao as ClasseProduto from Produtos as p inner join Empresas as E on E.id = p.idEmpresaProduto ")
                .Append("inner join ClassesProdutos as c on c.id = p.idClasseProduto ")
                .Append("where p.Excluido = 0 ");

            var Lista = dc.Consultar<Produto>(str.ToString()).ToList().OrderByDescending(x => x.id);

            if (Sindiveg)
                return Lista;
            else
                return Lista.Where(x => Empresas.Contains(x.idEmpresa));
        }

        public Produtos Selecionar(int id)
        {
            var produto = dc.Selecionar<Produtos>(id);
            return produto;
        }

        public Produtos Selecionar(int id, bool Sindiveg, List<int> Empresas)
        {
            var produto = dc.Selecionar<Produtos>(id);
            if (Sindiveg || Empresas.Contains((int)produto.idEmpresa))
                return produto;
            else
                return null;
        }

        public Produtos Incluir(Produtos Produto)
        {
            validacao.Validacao(Produto);
            Produto.id = dc.Inserir(Produto);

            return Produto;
        }

        public Produtos Atualizar(int id, Produtos Produto, bool Sindiveg, List<int> Empresas)
        {
            if (Sindiveg || Empresas.Contains((int)Produto.idEmpresa))
            {
                validacao.Validacao(Produto);
                dc.Atualizar(Produto, id);
            }

            return Produto;
        }

        public void Excluir(int id, bool Sindiveg, List<int> Empresas)
        {
            var produto = dc.Selecionar<Produtos>(id);
            if (Sindiveg || Empresas.Contains((int)produto.idEmpresa))
                dc.Excluir<Produtos>(id);
        }

        public IEnumerable<ComboProdutos> ListaCombo()
        {
            var str = new StringBuilder()
                .Append("select id as idEmpresa, RazaoSocial as nomeEmpresa ")
                .Append("from Empresas where excluido = 0");

            var ListaEmpresas = dc.Consultar<ComboProdutos>(str.ToString()).ToList().OrderBy(x => x.nomeEmpresa);

            str.Clear();

            str = new StringBuilder()
                .Append("select id as idClasseProduto, Descricao as DescricaoClasseProduto ")
                .Append("from ClassesProdutos where excluido = 0");

            var ListaClassesProdutos = dc.Consultar<ComboProdutos>(str.ToString()).ToList().OrderBy(x => x.DescricaoClasseProduto);

            var ListaRetorno = new List<ComboProdutos>();

            ListaRetorno.AddRange(ListaEmpresas);
            ListaRetorno.AddRange(ListaClassesProdutos);

            return ListaRetorno;
        }

    }
}
