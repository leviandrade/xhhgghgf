﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sindiveg.API.Banco;
using Sindiveg.API.BLL;

namespace Sindiveg.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Encaminhamento")]
    public class EncaminhamentoController : Controller
    {

        private EncaminhamentoBLL cpBLL;
        private Handler handler;


        public EncaminhamentoController(EncaminhamentoBLL EncaminhamentoBLL, Handler handler)
        {
            cpBLL = EncaminhamentoBLL;
            this.handler = handler;
        }

        //[Authorize("Bearer")]
        [HttpGet]
        public IActionResult Get()
        {
            var lista = handler.Handle(this, cpBLL.Lista);
            return lista;
        }

        //[Authorize("Bearer")]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return handler.Handle(this, () => cpBLL.Selecionar(id));
        }

        //[Authorize("Bearer")]
        [HttpPost]
        public IActionResult Post([FromBody]Encaminhamento Encaminhamento)
        {
            return handler.Handle(this, () => cpBLL.Incluir(Encaminhamento));
        }

        //[Authorize("Bearer")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Encaminhamento Encaminhamento)
        {
            return handler.Handle(this, () => cpBLL.Atualizar(id, Encaminhamento));
        }

        //[Authorize("Bearer")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return handler.Handle(this, () => cpBLL.Excluir(id));
        }
    }
}