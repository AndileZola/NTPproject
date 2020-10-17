using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NTPproject.Models;

namespace NTPproject.Controllers
{
    public class HomeController : Controller
    {
        private readonly WeighupContext _context;
        private readonly ILogger<HomeController> _logger;
        private IWebHostEnvironment _hostingEnvironment;

        public HomeController(ILogger<HomeController> logger, WeighupContext context, IWebHostEnvironment hostingEnvironment)
        {
            _logger  = logger;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            var cals = _context.Calculators.Include(x=>x.Seller).ToList();
            return View(cals);
        }

        public IActionResult Buy(int id)
        {
            var calc = _context.Calculators.SingleOrDefault(x=>x.BuyerId != null);
            calc.BuyerId = 4;
            var isSold = _context.SaveChanges() > 0;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Add(vmCalculator vmc,IFormFile img)
        {
            Calculator calculator = vmc.Calc;
            calculator.ImagePath  = vmc.Image.FileName;
            calculator.SellerId = 1;
            var calc = _context.Calculators.Add(calculator);           
            SaveDocuments(vmc);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Add()
        {           
            return View();
        }

        bool SaveDocuments(vmCalculator patient)
        {
            List<IFormFile> docFiles = new List<IFormFile> { patient.Image };
            if (docFiles?.Count > 0)
            {
                string webRootPath = _hostingEnvironment.WebRootPath;
                string contentRootPath = _hostingEnvironment.ContentRootPath;
                //var _DestinationDirectory = @"/UploadedFiles/Documents/";
                var _DestinationDirectory = "wwwroot/images/";
                //var _NewDirectory = Guid.NewGuid();
                //_DestinationDirectory = _DestinationDirectory + _NewDirectory;
                try
                {
                    foreach (var supportingDocument in docFiles)
                    {
                        var _NewFile = string.Empty;
                        var _FullPath = string.Empty;
                        var _NewFileExtension = Path.GetExtension(supportingDocument.FileName);
                       // var _NewFileName = Path.ChangeExtension(Path.GetRandomFileName(), _NewFileExtension);

                        _FullPath = _DestinationDirectory + @"/" + supportingDocument.FileName;
                        var exists = Directory.Exists(_DestinationDirectory);
                        //if (!exists)
                        //    Directory.CreateDirectory(webRootPath + _DestinationDirectory);

                        using (FileStream fs = System.IO.File.Create(_FullPath))
                        {                            
                            supportingDocument.CopyTo(fs);
                            fs.Flush();
                        }
                        _NewFile = string.Empty;
                        _FullPath = string.Empty;
                    }
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    goto ExitMethod;
                }
            }
            ExitMethod:
            return false;
        }

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
