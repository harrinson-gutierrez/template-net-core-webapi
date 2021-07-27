using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Adapter.Email.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers.V1
{

    [ApiVersion("1.0")]
    public class WeatherForecastController : BaseApiController
    {
        private readonly IEmailService EmailService;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IEmailService emailService)
        {
            _logger = logger;
            EmailService = emailService;
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
