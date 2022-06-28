using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RedisCaching.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IDistributedCache _distributedCache;

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IDistributedCache distributedCache)
        {
            _logger = logger;
            _distributedCache = distributedCache;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {

            CancellationTokenSource source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromSeconds(30));

            var cacheKey = "cityList";
            string serializedList;
            List<string> cityList = new List<string>();

            var redisCityList = await _distributedCache.GetAsync(cacheKey, source.Token);
            if (redisCityList != null)
            {
                serializedList = Encoding.UTF8.GetString(redisCityList);
                cityList = JsonConvert.DeserializeObject<List<string>>(serializedList);
            }
            else
            {
                cityList = GetFromDb();

                serializedList = JsonConvert.SerializeObject(cityList);
                redisCityList = Encoding.UTF8.GetBytes(serializedList);

                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));

                //AbsoluteExpiration will expire the entry after a set amount of time.
                //SlidingExpiration will expire the entry if it hasn't been accessed in a set amount of time.

                await _distributedCache.SetAsync(cacheKey, redisCityList, options);
            }
            return Ok(cityList);
        }

        private List<string> GetFromDb()
        {
            // Sample code getting from db
            return new List<string> {
                  "Dhaka", "Chittagong", "Khulna", "Rajshahi", "Sylhet", "Comilla", "Gazipur", "Mymensingh", "Barisal", "Rangpur","Narayanganj"
            }.ToList();
        }
    }
}
