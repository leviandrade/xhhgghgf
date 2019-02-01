using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sindiveg.API.Banco;
using Sindiveg.API.BLL;
using Sindiveg.API.Models;

namespace Sindiveg.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Produtos")]
    public class ProdutosController : Controller
    {
        private ProdutosBLL pBLL;
        private Handler handler;
        private UserInfo userInfo;

        public ProdutosController(ProdutosBLL produtosBLL, Handler handler, UserInfoBLL uiBLL)
        {
            pBLL = produtosBLL;
            this.handler = handler;
            userInfo = uiBLL.UserInfo;
        }

        //[Authorize("Bearer")]
        //[HttpGet]
        //public List<int> ListaPermissoes()
        //{
        //    if (!userInfo.Sindiveg)
        //    {
        //        var EmpresasUsuarios = userInfo.Empresas != string.Empty ? Array.ConvertAll(userInfo.Empresas.Split(","), int.Parse).ToList()
        //          : new List<int>();
        //        EmpresasUsuarios.Add((int)userInfo.idEmpresa);
        //        return EmpresasUsuarios;
        //    }

        //    else
        //        return new List<int>();
        //}

        [Authorize("Bearer")]
        [HttpGet]
        public IActionResult Get()
        {
            var EmpresasUsuarios = new List<int>();
            if (!userInfo.Sindiveg)
            {
                EmpresasUsuarios = userInfo.Empresas != string.Empty ? Array.ConvertAll(userInfo.Empresas.Split(","), int.Parse).ToList()
                 : new List<int>();
                EmpresasUsuarios.Add((int)userInfo.idEmpresa);
            }
            return handler.Handle(this, () => pBLL.ListaProdutos(userInfo.Sindiveg, EmpresasUsuarios));
        }

        [Authorize("Bearer")]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var EmpresasUsuarios = new List<int>();
            if (!userInfo.Sindiveg)
            {
                EmpresasUsuarios = userInfo.Empresas != string.Empty ? Array.ConvertAll(userInfo.Empresas.Split(","), int.Parse).ToList()
                 : new List<int>();
                EmpresasUsuarios.Add((int)userInfo.idEmpresa);
            }
            return handler.Handle(this, () => pBLL.Selecionar(id, userInfo.Sindiveg, EmpresasUsuarios));
        }

        [Authorize("Bearer")]
        [HttpPost]
        public IActionResult Post([FromBody]Produtos Produto)
        {
            Produto.idEmpresa = (int)userInfo.idEmpresa;
            return handler.Handle(this, () => pBLL.Incluir(Produto));
        }

        [Authorize("Bearer")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Produtos Produto)
        {
            var EmpresasUsuarios = new List<int>();
            if (!userInfo.Sindiveg)
            {
                EmpresasUsuarios = userInfo.Empresas != string.Empty ? Array.ConvertAll(userInfo.Empresas.Split(","), int.Parse).ToList()
                 : new List<int>();
                EmpresasUsuarios.Add((int)userInfo.idEmpresa);
            }
            return handler.Handle(this, () => pBLL.Atualizar(id, Produto, userInfo.Sindiveg, EmpresasUsuarios));
        }

        [Authorize("Bearer")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var EmpresasUsuarios = new List<int>();
            if (!userInfo.Sindiveg)
            {
                EmpresasUsuarios = userInfo.Empresas != string.Empty ? Array.ConvertAll(userInfo.Empresas.Split(","), int.Parse).ToList()
                 : new List<int>();
                EmpresasUsuarios.Add((int)userInfo.idEmpresa);
            }
            return handler.Handle(this, () => pBLL.Excluir(id, userInfo.Sindiveg, EmpresasUsuarios));
        }

        [Authorize("Bearer")]
        [HttpGet("ListaCombo")]
        public IActionResult ListaCombo()
        {
            return handler.Handle(this, () => pBLL.ListaCombo());
        }
    }
}