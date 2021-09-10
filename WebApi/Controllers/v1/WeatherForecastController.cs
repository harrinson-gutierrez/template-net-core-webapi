using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Adapter.Email.Interfaces;
using Infrastructure.Adapter.SQS.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers.V1
{

    [ApiVersion("1.0")]
    public class WeatherForecastController : BaseApiController
    {
        private readonly IEmailService EmailService;
        private readonly ISqsService SqsService;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, 
                                         IEmailService emailService,
                                         ISqsService sqsService)
        {
            _logger = logger;
            EmailService = emailService;
            SqsService = sqsService;
        }

        [HttpGet]
        [Route("SQS")]
        public async Task<IActionResult> SendSQS()
        {
            await SqsService.PostMessageAsync("prueba3", new WeatherForecast()
            {
                Date = DateTime.Now,
                Summary = "KAKA",
                TemperatureC = 42
            });
            return new OkResult();
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

        [HttpPost]
        [Route("Email")]
        public IActionResult SendEmail()
        {
            EmailService.SendEmail(new Infrastructure.Adapter.Email.Models.EmailRequest()
            {
                HtmlBody = "HOLA MUNDO",
                Receivers = new List<string>() { "contacto@bofugroupcolombia.com" },
                Subject ="prueba",
                TextBody =" hola mundo"
            });

            return new OkResult();
        }
    }
}
