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
    [Route("api/Validacao")]
    public class ValidacaoController : Controller
    {
        public ValidacaoBLL vBLL;
        public Handler handler;
        public UserInfo userInfo;


        public ValidacaoController(ValidacaoBLL validacaoBLL, Handler handler, UserInfoBLL uiBLL)
        {
            vBLL = validacaoBLL;
            this.handler = handler;
            userInfo = uiBLL.UserInfo;
        }

        [Authorize("Bearer")]
        [HttpGet]
        public IActionResult Get()
        {
            return handler.Handle(this, vBLL.Validar);
        }
    }
}