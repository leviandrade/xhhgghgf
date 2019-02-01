using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sindiveg.API.BLL;
using Sindiveg.API.Models;

namespace Sindiveg.API.Controllers
{
    [Produces("application/json")]
    [Route("api/UsuariosPermissoes")]
    public class UsuariosPermissoesController : Controller
    {
        private UsuariosPermissoesBLL upBLL;
        private Handler handler;
        private UserInfo userInfo;

        public UsuariosPermissoesController(UsuariosPermissoesBLL usuariosPermissoesBLL, Handler handler, UserInfoBLL uiBLL)
        {
            upBLL = usuariosPermissoesBLL;
            this.handler = handler;
            userInfo = uiBLL.UserInfo;
        }

        [Authorize("Bearer")]
        [HttpGet]
        public IActionResult Get()
        {
            var UsuarioLogado = userInfo.idUsuario;
            return handler.Handle(this, () => upBLL.ListaPermissoesMenu(UsuarioLogado.Value));
        }
    }
}