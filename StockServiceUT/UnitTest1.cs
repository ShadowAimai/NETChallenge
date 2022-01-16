using System;
using Xunit;
using StockService;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace StockServiceUT
{
    public class UnitTest1 : IClassFixture<TestSetup>
    {
        private ServiceProvider _serviceProvider;
        private StockController _controller;

        public UnitTest1(TestSetup testSetup)
        {
            _serviceProvider = testSetup.ServiceProvider;
            _controller = _serviceProvider.GetService<StockController>();
        }

        [Fact]
        public async Task Empty_Code_Should_Return_Empty_List()
        {
            List<Stock> result = await _controller.Get("");
            Assert.Empty(result);
        }

        [Fact]
        public async Task Valid_Code_Should_Return_Valid_List_And_Values()
        {
            List<Stock> result = await _controller.Get("aapl.us");

            decimal close = 0;

            bool IsNumber = decimal.TryParse(result[0].Close.Replace("$", String.Empty), out close);

            bool validClose = close > 0;

            Assert.True(validClose);
        }

        [Fact]
        public async Task Invalid_Code_Should_Return_Valid_List_And_Invalid_Values()
        {
            List<Stock> result = await _controller.Get("FAIL");

            decimal close = 0;

            bool IsNumber = decimal.TryParse(result[0].Close.Replace("$", String.Empty), out close);

            Assert.False(IsNumber);
            Assert.Equal("N/D", result[0].Close);
        }

    }
}
