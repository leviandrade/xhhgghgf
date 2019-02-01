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
    [Route("api/Configuracoes")]
    public class ConfiguracoesController : Controller
    {
        private ConfiguracoesBLL cBLL;
        private Handler handler;
        private UserInfo userInfo;


        public ConfiguracoesController(ConfiguracoesBLL configuracoesBLL, Handler handler, UserInfoBLL uiBLL)
        {
            cBLL = configuracoesBLL;
            this.handler = handler;
            userInfo = uiBLL.UserInfo;
        }

        [Authorize("Bearer")]
        [HttpGet]
        public IActionResult Get()
        {
            return handler.Handle(this, cBLL.Lista);
        }

        [Authorize("Bearer")]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return handler.Handle(this, () => cBLL.Selecionar(id));
        }

        [Authorize("Bearer")]
        [HttpPost]
        public IActionResult Post([FromBody]Configuracoes config)
        {
            return handler.Handle(this, () => cBLL.Incluir(config, userInfo));
        }

        [Authorize("Bearer")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Configuracoes config)
        {
            return handler.Handle(this, () => cBLL.Atualizar(config, userInfo));
        }

        [Authorize("Bearer")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return handler.Handle(this, () => cBLL.Excluir(id));
        }
    }
}