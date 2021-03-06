﻿using System;
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
    [Route("api/Locais")]
    public class LocaisController : Controller
    {
        private LocaisBLL lBLL;
        private Handler handler;
        private UserInfo userInfo;

        public LocaisController(LocaisBLL lBLL, Handler handler, UserInfoBLL uiBLL)
        {
            this.lBLL = lBLL;
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
            return handler.Handle(this, () => lBLL.Lista(userInfo.Sindiveg, EmpresasUsuarios));
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
            return handler.Handle(this, () => lBLL.Selecionar(id, userInfo.Sindiveg, EmpresasUsuarios));
        }

        [Authorize("Bearer")]
        [HttpPost]
        public IActionResult Post([FromBody]Locais Local)
        {
            Local.idEmpresa = (int)userInfo.idEmpresa;
            return handler.Handle(this, () => lBLL.Incluir(Local));
        }

        [Authorize("Bearer")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Locais Local)
        {
            var EmpresasUsuarios = new List<int>();
            if (!userInfo.Sindiveg)
            {
                EmpresasUsuarios = userInfo.Empresas != string.Empty ? Array.ConvertAll(userInfo.Empresas.Split(","), int.Parse).ToList()
                 : new List<int>();
                EmpresasUsuarios.Add((int)userInfo.idEmpresa);
            }
            return handler.Handle(this, () => lBLL.Atualizar(id, Local, userInfo.Sindiveg, EmpresasUsuarios));
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
            return handler.Handle(this, () => lBLL.Excluir(id, userInfo.Sindiveg, EmpresasUsuarios));
        }

    }
}