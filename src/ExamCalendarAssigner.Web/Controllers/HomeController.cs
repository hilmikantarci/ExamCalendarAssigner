using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ExamCalendarAssigner.Web.Models;
using ExamCalendarAssigner.Data.Model;
using Newtonsoft.Json;
using System.IO;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace ExamCalendarAssigner.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHostEnvironment _env;

        public HomeController(ILogger<HomeController> logger,
            IHostEnvironment env
            )
        {
            _logger = logger;
            _env = env;
        }

        public IActionResult Index()
        {
            //string json = System.IO.File.ReadAllText(@"C:\Users\Hilmi\Desktop\data.json");
            //SınavTakvimModel model = JsonConvert.DeserializeObject<SınavTakvimModel>(json);
            return View();
        }
        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Upload(IFormFile files)
        {

            var content = string.Empty;
            //string datapath = $"{_env.ContentRootPath}\\Models\\Data_${DateTime.Now.ToString("yyyymmdd_hhMMss")}.json";
            string datapath = $"{_env.ContentRootPath}\\Models\\data.json";


            using (var reader = new StreamReader(files.OpenReadStream()))
            {
                content = reader.ReadToEnd();
            }                   


            SınavTakvimModel model = JsonConvert.DeserializeObject<SınavTakvimModel>(content);
            System.IO.File.WriteAllText(datapath, "");
            System.IO.File.WriteAllText(datapath, content);

            return View();

        }
        //public async Task<IActionResult> Upload(List<IFormFile> files)
        //{
        //    //files.SaveAs(Server.MapPath())

        //    //return View("index");
        //    foreach (var file in files)
        //    {
        //        if (file.ContentLength > 0)
        //        {
        //            var fileName = Path.GetFileName(file.FileName);
        //            var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
        //            file.SaveAs(path);
        //        }
        //    }
        //    return RedirectToAction("Index");
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
