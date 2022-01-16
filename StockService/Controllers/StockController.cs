using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace StockService
{
    [ApiController]
    [Route("[controller]")]
    public class StockController : Controller
    {
        private readonly IConfiguration _config;

        static readonly HttpClient client = new HttpClient();

        public StockController(IConfiguration config)
        {
            _config = config;
        }


        [HttpGet("Get/{stock_code}")]
        public async Task<List<Stock>> Get(string stock_code)
        {

            try
            {
                string pre = _config.GetSection("CSVUri").GetSection("CSVUriPre").Value;
                string post = _config.GetSection("CSVUri").GetSection("CSVUriPost").Value;

                HttpResponseMessage response = await client.GetAsync(pre + stock_code + post);
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
