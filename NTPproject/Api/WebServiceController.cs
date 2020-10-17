using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NTPproject
{
    //[Route("api/[controller]")]
    [ApiController]
    public class WebServiceController : Controller
    {
        private readonly WeighupContext _context;
        private readonly ILogger<WebServiceController> _logger;
        private IWebHostEnvironment _hostingEnvironment;
        public WebServiceController(ILogger<WebServiceController> logger, WeighupContext context, IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }
        // GET: api/<WebServiceController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<WebServiceController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        [Route("api/Buy/{id}")]
        public IActionResult Buy(int id)
        {
            var calc = _context.Calculators.SingleOrDefault(x => x.BuyerId != null);
            calc.BuyerId = 4;
            var isSold = _context.SaveChanges() > 0;
            return Json(isSold);
        }

        // POST api/<WebServiceController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        // PUT api/<WebServiceController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<WebServiceController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
