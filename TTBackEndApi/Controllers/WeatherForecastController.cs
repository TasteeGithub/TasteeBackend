using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using TTFrontEnd.Models.DataContext;
using TTFrontEnd.Models.SqlDataContext;
using TTFrontEnd.Services;

namespace TTFrontEnd.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private ITTService<Roles> _sv;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ITTService<Roles> sv)
        {
            _logger = logger;
            _sv = sv;
        }

        //[HttpGet]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    _logger.LogInformation("Thu test log");
        //    var rng = new Random();
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = rng.Next(-20, 55),
        //        Summary = Summaries[rng.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}

        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Get()
        {
            var ttt = _sv.Queryable();
            return Ok(ttt);
        }
    }
}

//Scaffold-DbContext "Host=localhost;Port=5432;Database=SW_Inside;Username=devhn;Password=devhn@2019" Npgsql.EntityFrameworkCore.PostgreSQL -OutputDir Models/DataContext -Force
//Scaffold-DbContext "Server=MINHTHU-PC\SQLEXPRESS;Database=TT;Trusted_Connection=True;MultipleActiveResultSets=true" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models/SqlDataContext -Force