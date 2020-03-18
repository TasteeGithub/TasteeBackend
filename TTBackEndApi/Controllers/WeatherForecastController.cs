using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using TTBackEndApi.Models.DataContext;
using TTBackEndApi.Services;

namespace TTBackEndApi.Controllers
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

        private ITTService<Operator> _sv;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ITTService<Operator> sv)
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

//Scaffold-DbContext "Host=localhost;Port=5432;Database=SW_Inside;Username=devhn;Password=devhn@2019" Npgsql.EntityFrameworkCore.PostgreSQL -OutputDir Models/DataContext -Force -Tables Operator

//    -- Table: public."Operator"

//-- DROP TABLE public."Operator";

//CREATE TABLE public."Operator"
//(
//    "UserId" bigint NOT NULL,
//    "UserName" character varying(50) COLLATE pg_catalog."default" NOT NULL,
//    "Password" character varying(100) COLLATE pg_catalog."default" NOT NULL,
//    "FullName" character varying(200) COLLATE pg_catalog."default" NOT NULL,
//    "CreatedDate" timestamp without time zone NOT NULL,
//    "Email" character varying COLLATE pg_catalog."default" NOT NULL,
//    CONSTRAINT "Operator_pkey" PRIMARY KEY ("UserId")
//)
//WITH(
//    OIDS = FALSE
//)
//TABLESPACE pg_default;

//ALTER TABLE public."Operator"
//    OWNER to devhn;
//ALTER TABLE "public"."Operator" 
//  ALTER COLUMN "UserId" ADD GENERATED ALWAYS AS IDENTITY;