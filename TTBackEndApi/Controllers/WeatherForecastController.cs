using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TTBackEndApi.Models.DataContext;
using TTBackEndApi.Services;
using URF.Core.Abstractions.Services;

namespace TTBackEndApi.Controllers
{
    //[Authorize(]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        IRequestService _sv;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IRequestService sv)
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


        [HttpGet]
        public IActionResult Get()
        {
            var ttt = _sv.Queryable().Take(2);
            return Ok(ttt);
            
        }
    }
}

//Scaffold-DbContext "Host=13.229.0.235;Port=4432;Database=sw_inside;Username=u_inside;Password=7mjJGj7dc7waQah" Npgsql.EntityFrameworkCore.PostgreSQL -OutputDir Models/DataContext -Force -Tables ISW_REQUESTS,ISW_REQUEST_HISTORY,ISW_HOLD_UNHOLD_MAPPING