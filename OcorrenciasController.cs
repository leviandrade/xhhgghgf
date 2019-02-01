using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Sindiveg.FrontEnd.Controllers
{
    public class OcorrenciasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Editar()
        {
            return View();
        }

        public IActionResult FormOcorrenciasContrabando()
        {
            return View();
        }

        public IActionResult FormOcorrenciasFalsificacao()
        {
            return View();
        }

        public IActionResult FormOcorrenciasRouboCarga()
        {
            return View();
        }
    }
}