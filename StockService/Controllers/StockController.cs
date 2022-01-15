using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace StockService
{
    [ApiController]
    [Route("[controller]")]
    public class StockController : Controller
    {

        static readonly HttpClient client = new HttpClient();

        private readonly ILogger<StockController> _logger;

        public StockController(ILogger<StockController> logger)
        {
            _logger = logger;
        }


        [HttpGet("Get/{stock_code}")]
        public async Task<List<Stock>> Get(string stock_code)
        {

            try
            {
                HttpResponseMessage response = await client.GetAsync(string.Format("https://stooq.com/q/l/?s={0}&f=sd2t2ohlcv&h&e=csv", stock_code));
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                return Utils.CSVConverter.GetValues(responseBody);
            }
            catch (Exception ex)
            {
                return new List<Stock>();
            }


        }
    }
}
