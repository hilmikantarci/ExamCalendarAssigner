using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ExamCalendarAssigner.Web.Controllers
{
    public class JsonController:Controller
    {
        public IActionResult FileJson()
        {
            return View();
        }
    }
}
