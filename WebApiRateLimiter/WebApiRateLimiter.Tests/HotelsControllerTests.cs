using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebApiRateLimiter.Tests
{
    [TestClass]
    public class HotelsControllerTests : TestServerFixture
    {
        [TestMethod]
        public async Task Test_HotelsController_Is_Up()
        {
            var response = await Client.GetAsync("api/city/bangkok");

            response.EnsureSuccessStatusCode();

            var responseStrong = await response.Content.ReadAsStringAsync();

            Assert.IsFalse(string.IsNullOrWhiteSpace(responseStrong.ToString()));
        }
    }
}
