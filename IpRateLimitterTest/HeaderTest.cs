using System;
using System.Linq;
using System.Threading.Tasks;
using IpRateLimiterExample;
using IpRateLimitterTest.Utils;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace IpRateLimitterTest
{
    public class HeaderTest: IClassFixture<CustomWebApplicationFactory<IpRateLimiterExample.Startup>>
    {
        private readonly CustomWebApplicationFactory<IpRateLimiterExample.Startup> _factory;
        private const string ClearURL = "/api/values";
        public HeaderTest(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/api/values/2",1)]
        [InlineData("/api/values", 9)]
        public async Task Response_Contain_Remaining_Header(string url,int remainingCount)
        {
            //Arrange
            var client = _factory.CreateClient();
            await client.DeleteAsync(ClearURL);

            //Act
            var response = await client.GetAsync(url);
            

            //Assert
            response.EnsureSuccessStatusCode();

            var limit_remainingHeader = response.Headers.FirstOrDefault(x => x.Key == "x-rate-limit-remaining");

            Assert.NotNull(limit_remainingHeader);

            var limit_remainingValue = limit_remainingHeader.Value.FirstOrDefault();

            Assert.NotNull(limit_remainingValue);
            Assert.Equal(remainingCount,Convert.ToInt32(limit_remainingValue));
        }
        [Theory]
        [InlineData("/api/values/2", 1)]
        [InlineData("/api/values", 9)]
        public async Task Response_Reach_Limit_0(string url, int remainingCount)
        {
            //Arrange
            var client = _factory.CreateClient();
            await client.DeleteAsync(ClearURL);


            while (remainingCount >= 0)
            {
                //Act
                var response = await client.GetAsync(url);


                //Assert
                response.EnsureSuccessStatusCode();

                var limit_remainingHeader = response.Headers.FirstOrDefault(x => x.Key == "x-rate-limit-remaining");

                Assert.NotNull(limit_remainingHeader);

                var limit_remainingValue = limit_remainingHeader.Value.FirstOrDefault();

                Assert.NotNull(limit_remainingValue);
                Assert.Equal(remainingCount, Convert.ToInt32(limit_remainingValue));
                remainingCount--;
            }
            
        }
    }
}
