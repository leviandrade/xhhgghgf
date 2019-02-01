using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sindiveg.API.Banco;
using Sindiveg.API.BLL;

namespace Sindiveg.API.Controllers
{
    [Produces("application/json")]
    [Route("api/OcorrenciasProdutos")]
    public class OcorrenciasProdutoController : Controller
    {
        private OcorrenciasProdutoBLL oBLL;
        private Handler handler;


        public OcorrenciasProdutoController(OcorrenciasProdutoBLL OcorrenciasProdutoBLL, Handler handler)
        {
            oBLL = OcorrenciasProdutoBLL;
            this.handler = handler;
        }

        //[Authorize("Bearer")]
        [HttpGet]
        public IActionResult Get()
        {
            return handler.Handle(this, oBLL.Lista);
        }

        //[Authorize("Bearer")]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return handler.Handle(this, () => oBLL.Selecionar(id));
        }

        //[Authorize("Bearer")]
        [HttpPost]
        public IActionResult Post([FromBody]OcorrenciasProduto OcorrenciasProdutos)
        {
            return handler.Handle(this, () => oBLL.Incluir(OcorrenciasProdutos));
        }

        //[Authorize("Bearer")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]OcorrenciasProduto OcorrenciasProdutos)
        {
            return handler.Handle(this, () => oBLL.Atualizar(id, OcorrenciasProdutos));
        }

        //[Authorize("Bearer")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return handler.Handle(this, () => oBLL.Excluir(id));
        }
    }
}