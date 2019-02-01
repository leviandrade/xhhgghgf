using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Sindiveg.FrontEnd.Controllers
{
    public class ClassesProdutosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}