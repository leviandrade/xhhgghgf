﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Sindiveg.FrontEnd.Controllers
{
    public class DashboardQuantidadeOcorrenciasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}