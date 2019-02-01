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
    [Route("api/Dashboards")]
    public class DashboardsController : Controller
    {
        private DashboardsBLL dBLL;
        private Handler handler;
        private UserInfo userInfo;

        public DashboardsController(DashboardsBLL dBLL, Handler handler, UserInfoBLL uiBLL)
        {
            this.dBLL = dBLL;
            this.handler = handler;
            userInfo = uiBLL.UserInfo;
        }

        [Authorize("Bearer")]
        [HttpPost("DashboardOcorrencias")]
        public IActionResult DashboardOcorrencias([FromBody]FiltroDashboard f)
        {
            var EmpresasUsuarios = new List<int>();
            var TipoOcorrencia = new List<int>();
            if (!userInfo.Sindiveg)
            {
                TipoOcorrencia = userInfo.TipoOcorrencia != string.Empty ? Array.ConvertAll(userInfo.TipoOcorrencia.Split(","), int.Parse)
            .ToList() : new List<int>();

                EmpresasUsuarios = userInfo.Empresas != string.Empty ? Array.ConvertAll(userInfo.Empresas.Split(","), int.Parse).ToList()
                    : new List<int>();

                EmpresasUsuarios.Add((int)userInfo.idEmpresa);
            }
            return handler.Handle(this, () => dBLL.DashboardOcorrencias(f, userInfo.Sindiveg, EmpresasUsuarios, TipoOcorrencia));
        }

        [Authorize("Bearer")]
        [HttpPost("DashboardOcorrenciasUF")]
        public IActionResult DashboardOcorrenciasUF([FromBody]FiltroDashboard f)
        {
            var EmpresasUsuarios = new List<int>();
            var TipoOcorrencia = new List<int>();
            if (!userInfo.Sindiveg)
            {
                TipoOcorrencia = userInfo.TipoOcorrencia != string.Empty ? Array.ConvertAll(userInfo.TipoOcorrencia.Split(","), int.Parse)
            .ToList() : new List<int>();

                EmpresasUsuarios = userInfo.Empresas != string.Empty ? Array.ConvertAll(userInfo.Empresas.Split(","), int.Parse).ToList()
                    : new List<int>();

                EmpresasUsuarios.Add((int)userInfo.idEmpresa);
            }
            return handler.Handle(this, () => dBLL.DashboardOcorrenciasUF(f, userInfo.Sindiveg, EmpresasUsuarios, TipoOcorrencia));
        }

        [Authorize("Bearer")]
        [HttpPost("DashboardLitrosProduto")]
        public IActionResult DashboarLitrosProduto([FromBody]FiltroDashboard f)
        {
            var EmpresasUsuarios = new List<int>();
            var TipoOcorrencia = new List<int>();
            if (!userInfo.Sindiveg)
            {
                TipoOcorrencia = userInfo.TipoOcorrencia != string.Empty ? Array.ConvertAll(userInfo.TipoOcorrencia.Split(","), int.Parse)
            .ToList() : new List<int>();

                EmpresasUsuarios = userInfo.Empresas != string.Empty ? Array.ConvertAll(userInfo.Empresas.Split(","), int.Parse).ToList()
                    : new List<int>();

                EmpresasUsuarios.Add((int)userInfo.idEmpresa);
            }
            return handler.Handle(this, () => dBLL.DashboarLitrosProduto(f, userInfo.Sindiveg, EmpresasUsuarios, TipoOcorrencia));
        }

        [Authorize("Bearer")]
        [HttpPost("DashboardRegistroProduto")]
        public IActionResult DashboardRegistroProduto([FromBody]FiltroDashboard f)
        {
            var EmpresasUsuarios = new List<int>();
            var TipoOcorrencia = new List<int>();
            if (!userInfo.Sindiveg)
            {
                TipoOcorrencia = userInfo.TipoOcorrencia != string.Empty ? Array.ConvertAll(userInfo.TipoOcorrencia.Split(","), int.Parse)
            .ToList() : new List<int>();

                EmpresasUsuarios = userInfo.Empresas != string.Empty ? Array.ConvertAll(userInfo.Empresas.Split(","), int.Parse).ToList()
                    : new List<int>();

                EmpresasUsuarios.Add((int)userInfo.idEmpresa);
            }
            return handler.Handle(this, () => dBLL.DashboardRegistroProduto(f, userInfo.Sindiveg, EmpresasUsuarios, TipoOcorrencia));
        }

        [Authorize("Bearer")]
        [HttpPost("DashboardDamage")]
        public IActionResult DashboardDamage([FromBody]FiltroDashboard f)
        {
            var EmpresasUsuarios = new List<int>();
            var TipoOcorrencia = new List<int>();
            if (!userInfo.Sindiveg)
            {
                TipoOcorrencia = userInfo.TipoOcorrencia != string.Empty ? Array.ConvertAll(userInfo.TipoOcorrencia.Split(","), int.Parse)
            .ToList() : new List<int>();

                EmpresasUsuarios = userInfo.Empresas != string.Empty ? Array.ConvertAll(userInfo.Empresas.Split(","), int.Parse).ToList()
                    : new List<int>();

                EmpresasUsuarios.Add((int)userInfo.idEmpresa);
            }
            return handler.Handle(this, () => dBLL.DashboardDamage(f, userInfo.Sindiveg, EmpresasUsuarios, TipoOcorrencia));
        }

        [Authorize("Bearer")]
        [HttpPost("DashboardTipoOcorrencia")]
        public IActionResult DashboardTipoOcorrencia([FromBody]FiltroDashboard f)
        {
            var EmpresasUsuarios = new List<int>();
            var TipoOcorrencia = new List<int>();
            if (!userInfo.Sindiveg)
            {
                TipoOcorrencia = userInfo.TipoOcorrencia != string.Empty ? Array.ConvertAll(userInfo.TipoOcorrencia.Split(","), int.Parse)
            .ToList() : new List<int>();

                EmpresasUsuarios = userInfo.Empresas != string.Empty ? Array.ConvertAll(userInfo.Empresas.Split(","), int.Parse).ToList()
                    : new List<int>();

                EmpresasUsuarios.Add((int)userInfo.idEmpresa);
            }
            return handler.Handle(this, () => dBLL.DashboardTipoOcorrencia(f, userInfo.Sindiveg, EmpresasUsuarios, TipoOcorrencia));
        }
        [Authorize("Bearer")]
        [HttpPost("DashboardGeral")]
        public IActionResult DashboardGeral([FromBody]FiltroDashboard f)
        {
            var EmpresasUsuarios = new List<int>();
            var TipoOcorrencia = new List<int>();
            if (!userInfo.Sindiveg)
            {
                TipoOcorrencia = userInfo.TipoOcorrencia != string.Empty ? Array.ConvertAll(userInfo.TipoOcorrencia.Split(","), int.Parse)
            .ToList() : new List<int>();

                EmpresasUsuarios = userInfo.Empresas != string.Empty ? Array.ConvertAll(userInfo.Empresas.Split(","), int.Parse).ToList()
                    : new List<int>();

                EmpresasUsuarios.Add((int)userInfo.idEmpresa);
            }
            return handler.Handle(this, () => dBLL.DashboardGeral(f, userInfo.Sindiveg, EmpresasUsuarios, TipoOcorrencia));
        }
    }
}