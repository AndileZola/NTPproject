using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NTPproject.Models
{
    public class vmCalculator
    {
        public Calculator Calc { get; set; }
        public IFormFile Image { get; set; }
    }
}
