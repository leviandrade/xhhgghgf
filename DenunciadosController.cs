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
    [Route("api/Denunciados")]
    public class DenunciadosController : Controller
    {
        private DenunciadosBLL dBLL;
        private Handler handler;
        private UserInfo userInfo;

        public DenunciadosController(DenunciadosBLL dBLL, Handler handler, UserInfoBLL uiBLL)
        {
            this.dBLL = dBLL;
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
            return handler.Handle(this, () => dBLL.ListaDenunciados(userInfo.Sindiveg, EmpresasUsuarios));
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
            return handler.Handle(this, () => dBLL.Selecionar(id, userInfo.Sindiveg, EmpresasUsuarios));
        }

        [Authorize("Bearer")]
        [HttpPost]
        public IActionResult Post([FromBody]Denunciados Denunciado)
        {
            Denunciado.idEmpresa = (int)userInfo.idEmpresa;
            return handler.Handle(this, () => dBLL.Incluir(Denunciado));
        }

        [Authorize("Bearer")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Denunciados Denunciado)
        {
            var EmpresasUsuarios = new List<int>();
            if (!userInfo.Sindiveg)
            {
                EmpresasUsuarios = userInfo.Empresas != string.Empty ? Array.ConvertAll(userInfo.Empresas.Split(","), int.Parse).ToList()
                 : new List<int>();
                EmpresasUsuarios.Add((int)userInfo.idEmpresa);
            }
            return handler.Handle(this, () => dBLL.Atualizar(id, Denunciado, userInfo.Sindiveg, EmpresasUsuarios));
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
            return handler.Handle(this, () => dBLL.Excluir(id, userInfo.Sindiveg, EmpresasUsuarios));
        }

        [Authorize("Bearer")]
        [HttpGet("ListaCombo")]
        public IActionResult ListaCombo()
        {
            return handler.Handle(this, () => dBLL.ListaCombo());
        }
    }
}