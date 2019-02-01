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
    public class CategoriasProdutosBLL : IBLL
    {
        private DbServer dc;
        CategoriasProdutosValidacao Validacao = new CategoriasProdutosValidacao();

        public CategoriasProdutosBLL(DbServer dc)
        {
            this.dc = dc;
        }

        public IEnumerable<CategoriasProdutos> Lista(bool Sindiveg, List<int> Empresas)
        {
            if (Sindiveg)
                return dc.Lista<CategoriasProdutos>();
            else
                return dc.Lista<CategoriasProdutos>().Where(x => Empresas.Contains((int)x.idEmpresa));
        }

        public CategoriasProdutos Selecionar(int id)
        {
            var CategoriaProduto = dc.Selecionar<CategoriasProdutos>(id);
            return CategoriaProduto;
        }

        public CategoriasProdutos Selecionar(int id, bool Sindiveg, List<int> Empresas)
        {
            var CategoriaProduto = dc.Selecionar<CategoriasProdutos>(id);
            if (Sindiveg || Empresas.Contains((int)CategoriaProduto.idEmpresa))
                return CategoriaProduto;
            else
                return null;
        }

        public CategoriasProdutos Incluir(CategoriasProdutos CategoriaProduto)
        {
            Validacao.Validacao(CategoriaProduto);
            CategoriaProduto.id = dc.Inserir(CategoriaProduto);

            return CategoriaProduto;
        }

        public CategoriasProdutos Atualizar(int id, CategoriasProdutos CategoriaProduto, bool Sindiveg, List<int> Empresas)
        {
            if (Sindiveg || Empresas.Contains((int)CategoriaProduto.idEmpresa))
            {
                Validacao.Validacao(CategoriaProduto);
                dc.Atualizar(CategoriaProduto, id);
            }

            return CategoriaProduto;
        }

        public void Excluir(int id, bool Sindiveg, List<int> Empresas)
        {
            var CategoriaProduto = dc.Selecionar<CategoriasProdutos>(id);
            if (Sindiveg || Empresas.Contains((int)CategoriaProduto.idEmpresa))
                dc.Excluir<CategoriasProdutos>(id);
        }
    }
}
