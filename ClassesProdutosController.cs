using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sindiveg.API.Banco;
using Sindiveg.API.BLL;
using Sindiveg.API.Models;

namespace Sindiveg.API.Controllers
{
    [Produces("application/json")]
    [Route("api/ClassesProdutos")]
    public class ClassesProdutosController : Controller
    {

        private ClassesProdutosBLL cpBLL;
        private Handler handler;
        private UserInfo userInfo;

        public ClassesProdutosController(ClassesProdutosBLL classesProdutosBLL, Handler handler, UserInfoBLL uiBLL)
        {
            cpBLL = classesProdutosBLL;
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
            return handler.Handle(this, () => cpBLL.Lista(userInfo.Sindiveg, EmpresasUsuarios));
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
            return handler.Handle(this, () => cpBLL.Selecionar(id, userInfo.Sindiveg, EmpresasUsuarios));
        }

        [Authorize("Bearer")]
        [HttpPost]
        public IActionResult Post([FromBody]ClassesProdutos classeProduto)
        {
            classeProduto.idEmpresa = (int)userInfo.idEmpresa;
            return handler.Handle(this, () => cpBLL.Incluir(classeProduto));
        }

        [Authorize("Bearer")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]ClassesProdutos classeProduto)
        {
            var EmpresasUsuarios = new List<int>();
            if (!userInfo.Sindiveg)
            {
                EmpresasUsuarios = userInfo.Empresas != string.Empty ? Array.ConvertAll(userInfo.Empresas.Split(","), int.Parse).ToList()
                 : new List<int>();
                EmpresasUsuarios.Add((int)userInfo.idEmpresa);
            }
            return handler.Handle(this, () => cpBLL.Atualizar(id, classeProduto, userInfo.Sindiveg, EmpresasUsuarios));
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
            return handler.Handle(this, () => cpBLL.Excluir(id, userInfo.Sindiveg, EmpresasUsuarios));
        }
    }
}