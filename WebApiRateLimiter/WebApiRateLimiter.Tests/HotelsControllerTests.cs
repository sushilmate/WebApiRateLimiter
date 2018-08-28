using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace WebApiRateLimiter.Tests
{
    [TestClass]
    public class HotelsControllerTests : TestServerFixture
    {
        [TestMethod]
        public async Task Test_HotelsController_City_WEBAPI_Is_Up()
        {
            var response = await Client.GetAsync("api/city/bangkok");

            response.EnsureSuccessStatusCode();

            var responseStrong = await response.Content.ReadAsStringAsync();

            Assert.IsFalse(string.IsNullOrWhiteSpace(responseStrong.ToString()));
        }

        [TestMethod]
        public async Task Test_HotelsController_Room_WEBAPI_Is_Up()
        {
            var response = await Client.GetAsync("api/room/deluxe");

            response.EnsureSuccessStatusCode();

            var responseStrong = await response.Content.ReadAsStringAsync();

            Assert.IsFalse(string.IsNullOrWhiteSpace(responseStrong.ToString()));
        }

        [TestMethod]
        public async Task Test_Get_Hotels_By_City_Limit_Rate_Exceeded()
        {
            for (int i = 0; i < 5; i++)
            {
                await Client.GetAsync("api/city/bangkok");
            }
            var response = await Client.GetAsync("api/city/bangkok");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            Assert.IsTrue(result == "Web API Rate Limit Exceeded.");
        }


        [TestMethod]
        public async Task Test_Get_Hotels_By_Room_Limit_Rate_Exceeded()
        {
            for (int i = 0; i < 5; i++)
            {
                await Client.GetAsync("api/room/deluxe");
            }
            var response = await Client.GetAsync("api/room/deluxe");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            Assert.IsTrue(result == "Web API Rate Limit Exceeded.");
        }

        [TestMethod]
        public async Task Test_Get_Hotels_By_City_Limit_Rate_Exceeded_And_Next_SubSequent_Request_Suspended()
        {
            for (int i = 0; i < 6; i++)
            {
                await Client.GetAsync("api/city/bangkok");
            }
            var response = await Client.GetAsync("api/city/bangkok");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            Assert.IsTrue(result == "Web API is suspended, Please try after sometime.");
        }

        [TestMethod]
        public async Task Test_Get_Hotels_By_Room_Limit_Rate_Exceeded_And_Next_SubSequent_Request_Suspended()
        {
            for (int i = 0; i < 6; i++)
            {
                await Client.GetAsync("api/room/deluxe");
            }
            var response = await Client.GetAsync("api/room/deluxe");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            Assert.IsTrue(result == "Web API is suspended, Please try after sometime.");
        }

        [TestMethod]
        public async Task Test_Get_Hotels_By_City_Limit_Rate_Exceeded_And_Get_Hotels_By_Room_Working()
        {
            for (int i = 0; i < 6; i++)
            {
                await Client.GetAsync("api/city/bangkok");
            }
            var response = await Client.GetAsync("api/room/deluxe");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            Assert.IsFalse(result != "Web API Rate Limit Exceeded.");
        }

        [TestMethod]
        public async Task Test_Get_Hotels_By_Room_Limit_Rate_Exceeded_And_Get_Hotels_By_City_Working()
        {
            for (int i = 0; i < 6; i++)
            {
                await Client.GetAsync("api/city/deluxe");
            }
            var response = await Client.GetAsync("api/city/bangkok");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            Assert.IsFalse(result != "Web API Rate Limit Exceeded.");
        }

        [TestMethod]
        public async Task Test_Get_Hotels_By_City_Suspended_And_Get_Hotels_By_Room_Working()
        {
            for (int i = 0; i < 6; i++)
            {
                await Client.GetAsync("api/city/bangkok");
            }
            var response = await Client.GetAsync("api/room/deluxe");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            Assert.IsFalse(result != "Web API is suspended, Please try after sometime.");
        }

        [TestMethod]
        public async Task Test_Get_Hotels_By_Room_Suspended_And_Get_Hotels_By_City_Working()
        {
            for (int i = 0; i < 6; i++)
            {
                await Client.GetAsync("api/city/deluxe");
            }
            var response = await Client.GetAsync("api/city/bangkok");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            Assert.IsFalse(result != "Web API is suspended, Please try after sometime.");
        }

    }
}