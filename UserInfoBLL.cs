using Microsoft.AspNetCore.Http;
using Sindiveg.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sindiveg.API.BLL
{
    public class UserInfoBLL
    {
        private readonly IHttpContextAccessor _context;
        public UserInfoBLL(IHttpContextAccessor context)
        {
            _context = context;
        }

        public UserInfo UserInfo
        {
            get
            {
                var user = _context.HttpContext.User;
                int? idUsuario = null;
                int? idEmpresa = null;
                bool Sindiveg = false;

                string claimIdUsuario = user.FindFirstValue("idUsuario");
                string claimIdEmpresa = user.FindFirstValue("idEmpresa");
                string claimEmpresa = user.FindFirstValue("Empresas");
                string claimTipoOcorrencia = user.FindFirstValue("TipoOcorrencia");
                string claimSindiveg = user.FindFirstValue("Sindiveg");

                if (!string.IsNullOrWhiteSpace(claimIdUsuario))
                    idUsuario = int.Parse(claimIdUsuario);

                if (!string.IsNullOrWhiteSpace(claimIdEmpresa))
                    idEmpresa = int.Parse(claimIdEmpresa);

                if (!string.IsNullOrWhiteSpace(claimSindiveg))
                    Sindiveg = bool.Parse(claimSindiveg);

                return new UserInfo
                {
                    Login = user.Identity.Name,
                    idUsuario = idUsuario,
                    idEmpresa = idEmpresa,
                    Empresas = claimEmpresa,
                    TipoOcorrencia = claimTipoOcorrencia,
                    Sindiveg = Sindiveg
                };
            }
        }
    }
}
