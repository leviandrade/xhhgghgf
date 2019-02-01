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
    [Route("api/OcorrenciasDenunciantes")]
    public class OcorrenciasDenunciantesController : Controller
    {
        private OcorrenciasDenunciantesBLL oBLL;
        private Handler handler;


        public OcorrenciasDenunciantesController(OcorrenciasDenunciantesBLL ocorrenciasDenunciantesBLL, Handler handler)
        {
            oBLL = ocorrenciasDenunciantesBLL;
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

        [HttpGet("{idOcorrencia}")]
        public IActionResult ListaporOcorrencia(int idOcorrencia)
        {
            return handler.Handle(this, () => oBLL.Selecionar(idOcorrencia));
        }

        //[Authorize("Bearer")]
        [HttpPost]
        public IActionResult Post([FromBody]OcorrenciasDenunciantes OcorrenciaDenunciantes)
        {
            return handler.Handle(this, () => oBLL.Incluir(OcorrenciaDenunciantes));
        }

        //[Authorize("Bearer")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]OcorrenciasDenunciantes OcorrenciaDenunciantes)
        {
            return handler.Handle(this, () => oBLL.Atualizar(id, OcorrenciaDenunciantes));
        }

        //[Authorize("Bearer")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return handler.Handle(this, () => oBLL.Excluir(id));
        }
    }
}