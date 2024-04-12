using Microsoft.AspNetCore.Mvc;

namespace WebApi1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TrafficForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Empty Road", "Light Traffic", "Medium Traffic", "Heavy Traffic"
        };

        private readonly ILogger<TrafficForecastController> _logger;

        public TrafficForecastController(ILogger<TrafficForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetTrafficForecast")]
        public IEnumerable<TrafficForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new TrafficForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Count = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
