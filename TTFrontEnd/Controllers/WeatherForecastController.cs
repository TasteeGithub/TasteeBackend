using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TasteeFrontEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        [Route("test")]
        public string Test()
        {
            return "lsdkflsdfk";
        }
    }
}


//Scaffold-DbContext "Host=localhost;Port=5432;Database=TT;Username=devhn;Password=devhn@2019" Npgsql.EntityFrameworkCore.PostgreSQL -OutputDir Models/DataContext -Force
//Scaffold-DbContext "Server=MINHTHU-PC\SQLEXPRESS;Database=TT;Trusted_Connection=True;MultipleActiveResultSets=true" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models/SqlDataContext -Force
//Scaffold-DbContext "Data Source=112.78.2.36;Initial Catalog=tas77143_tastee;Persist Security Info=True;User ID=tas77143_tastee;Password=cU9&yt11" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models/SqlDataContext -Force