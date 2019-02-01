using Digiexpress.ADO;
using Sindiveg.API.Banco;
using Sindiveg.API.Validacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sindiveg.API.BLL
{
    public class ClassesProdutosBLL : IBLL
    {
        private DbServer dc;
        ClassesProdutosValidacao Validacao = new ClassesProdutosValidacao();

        public ClassesProdutosBLL(DbServer dc)
        {
            this.dc = dc;
        }

        public IEnumerable<ClassesProdutos> Lista(bool Sindiveg, List<int> Empresas)
        {
            if (Sindiveg)
                return dc.Lista<ClassesProdutos>();
            else
                return dc.Lista<ClassesProdutos>().Where(x => Empresas.Contains((int)x.idEmpresa));
        }

        public ClassesProdutos Selecionar(int id, bool Sindiveg, List<int> Empresas)
        {
            var ClasseProduto = dc.Selecionar<ClassesProdutos>(id);
            if (Sindiveg || Empresas.Contains((int)ClasseProduto.idEmpresa))
                return ClasseProduto;
            else
                return null;
        }

        public ClassesProdutos Incluir(ClassesProdutos ClasseProduto)
        {
            Validacao.Validacao(ClasseProduto);
            ClasseProduto.id = dc.Inserir(ClasseProduto);

            return ClasseProduto;
        }

        public ClassesProdutos Atualizar(int id, ClassesProdutos ClasseProduto, bool Sindiveg, List<int> Empresas)
        {
            if (Sindiveg || Empresas.Contains((int)ClasseProduto.idEmpresa))
            {
                Validacao.Validacao(ClasseProduto);
                dc.Atualizar(ClasseProduto, id);
            }

            return ClasseProduto;
        }

        public void Excluir(int id, bool Sindiveg, List<int> Empresas)
        {
            var ClasseProduto = dc.Selecionar<ClassesProdutos>(id);
            if (Sindiveg || Empresas.Contains((int)ClasseProduto.idEmpresa))
                dc.Excluir<ClassesProdutos>(id);
        }
    }
}
