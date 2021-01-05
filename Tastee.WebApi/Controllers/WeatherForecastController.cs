using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Tastee.WebApi.Controllers
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
        private readonly IMediator _mediator;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
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

        [HttpGet]
        [Route("detail")]
        public async Task<string> Detail()
        {
            RequestModel rq = new RequestModel { Id = "123" };
            return await _mediator.Send(rq);
        }

    }


    public class RequestModel : IRequest<string>
    {
        public string Id { get; set; }
    }

    public class ResponseModel
    {
        public string Name { get; set; }
    }

    public class DetailHandler : IRequestHandler<RequestModel, string>
    {
        public async Task<string> Handle(RequestModel request, CancellationToken cancellationToken)
        {
            return "Nguyen Minh Thu";
        }

        //public async Task<string> Handle(RequestModel request, CancellationToken cancellationToken)
        //{
        //    return "Nguyen Minh Thu";
        //}
    }


    public class WeatherForecast
    {
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public string Summary { get; set; }
    }
}


//Scaffold-DbContext "Host=localhost;Port=5432;Database=TT;Username=devhn;Password=devhn@2019" Npgsql.EntityFrameworkCore.PostgreSQL -OutputDir Models/DataContext -Force
//Scaffold-DbContext "Server=MINHTHU-PC\SQLEXPRESS;Database=TT;Trusted_Connection=True;MultipleActiveResultSets=true" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models/SqlDataContext -Force
//Scaffold-DbContext "Data Source=MINHTHU-PC;Initial Catalog=tastee;Persist Security Info=True;User ID=tas77143_tastee;Password=K5EOcP3MPgUpc" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Context -Force