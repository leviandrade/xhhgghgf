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
    [Route("api/Usuarios")]
    public class UsuariosController : Controller
    {
        private UsuariosBLL uBLL;
        private Handler handler;
        private UserInfo userInfo;


        public UsuariosController(UsuariosBLL usuariosBLL, Handler handler, UserInfoBLL uiBLL)
        {
            uBLL = usuariosBLL;
            this.handler = handler;
            userInfo = uiBLL.UserInfo;
        }

        [Authorize("Bearer")]
        [HttpGet]
        public IActionResult Get()
        {
            return handler.Handle(this, uBLL.Lista);
        }

        [Authorize("Bearer")]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return handler.Handle(this, () => uBLL.Selecionar(id));
        }

        [Authorize("Bearer")]
        [HttpPost]
        public IActionResult Post([FromBody]Usuarios usuario)
        {
            return handler.Handle(this, () => uBLL.Incluir(usuario));
        }

        [Authorize("Bearer")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Usuarios usuario)
        {
            return handler.Handle(this, () => uBLL.Atualizar(id, usuario));
        }

        [Authorize("Bearer")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return handler.Handle(this, () => uBLL.Excluir(id));
        }
    }
}