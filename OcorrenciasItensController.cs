using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Sindiveg.API.Banco;
using Sindiveg.API.BLL;
using Sindiveg.API.Models;

namespace Sindiveg.API.Controllers
{
    [Produces("application/json")]
    [Route("api/OcorrenciasItens")]
    public class OcorrenciasItensController : Controller
    {
        private OcorrenciasItensBLL oBLL;
        private Handler handler;
        private UserInfo userInfo;
        private IHostingEnvironment _hostingEnvironment;

        public OcorrenciasItensController(OcorrenciasItensBLL ocorrenciasItensBLL, Handler handler,UserInfoBLL uiBLL, IHostingEnvironment environment)
        {
            oBLL = ocorrenciasItensBLL;
            this.handler = handler;
            userInfo = uiBLL.UserInfo;
            _hostingEnvironment = environment;
        }

        //[Authorize("Bearer")]
        //[HttpGet]
        //public List<List<int>> ListaPermissoes()
        //{
        //    if (!userInfo.Sindiveg)
        //    {
        //        var TipoOcorrencia = userInfo.TipoOcorrencia != string.Empty ? Array.ConvertAll(userInfo.TipoOcorrencia.Split(","), int.Parse)
        //    .ToList() : new List<int>();

        //        var EmpresasUsuarios = userInfo.Empresas != string.Empty ? Array.ConvertAll(userInfo.Empresas.Split(","), int.Parse).ToList()
        //            : new List<int>();

        //        EmpresasUsuarios.Add((int)userInfo.idEmpresa);

        //        var Lista = new List<List<int>>();
        //        Lista.Add(TipoOcorrencia);
        //        Lista.Add(EmpresasUsuarios);

        //        return Lista;
        //    }
        //    else
        //        return new List<List<int>>();
        //}

        [Authorize("Bearer")]
        [HttpGet]
        public IActionResult Get()
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
            return handler.Handle(this, () => oBLL.Lista(userInfo.Sindiveg, EmpresasUsuarios, TipoOcorrencia));
        }

        [Authorize("Bearer")]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
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
            return handler.Handle(this, () => oBLL.Selecionar(id, userInfo.Sindiveg, EmpresasUsuarios, TipoOcorrencia, _hostingEnvironment.ContentRootPath));
        }

        [Authorize("Bearer")]
        [HttpPost]
        public IActionResult Post([FromBody]OcorrenciasItens OcorrenciaItem)
        {
            OcorrenciaItem.idEmpresa = (int)userInfo.idEmpresa;
            OcorrenciaItem.path = _hostingEnvironment.ContentRootPath;
            return handler.Handle(this, () => oBLL.Incluir(OcorrenciaItem));
        }

        [Authorize("Bearer")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]OcorrenciasItens OcorrenciaItem)
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

            return handler.Handle(this, () => oBLL.Atualizar(OcorrenciaItem, userInfo.Sindiveg, EmpresasUsuarios, TipoOcorrencia, _hostingEnvironment.ContentRootPath));
        }

        [Authorize("Bearer")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
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
            return handler.Handle(this, () => oBLL.Excluir(id, userInfo.Sindiveg, EmpresasUsuarios, TipoOcorrencia));
        }
    }
}