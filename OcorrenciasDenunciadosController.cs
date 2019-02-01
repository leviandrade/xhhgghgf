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
    [Route("api/OcorrenciasDenunciados")]
    public class OcorrenciasDenunciadosController : Controller
    {
        private OcorrenciasDenunciadosBLL oBLL;
        private Handler handler;


        public OcorrenciasDenunciadosController(OcorrenciasDenunciadosBLL ocorrenciasDenunciadosBLL, Handler handler)
        {
            oBLL = ocorrenciasDenunciadosBLL;
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
        public IActionResult Post([FromBody]OcorrenciasDenunciados OcorrenciaDenunciado)
        {
            return handler.Handle(this, () => oBLL.Incluir(OcorrenciaDenunciado));
        }

        //[Authorize("Bearer")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]OcorrenciasDenunciados OcorrenciaDenunciado)
        {
            return handler.Handle(this, () => oBLL.Atualizar(id, OcorrenciaDenunciado));
        }

        //[Authorize("Bearer")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return handler.Handle(this, () => oBLL.Excluir(id));
        }
    }
}