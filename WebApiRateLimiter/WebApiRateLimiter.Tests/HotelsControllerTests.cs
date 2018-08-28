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
            var response = await Client.GetAsync(Constants.HOTELS_BY_CITY_WEBAPI_URL);

            response.EnsureSuccessStatusCode();

            var responseStrong = await response.Content.ReadAsStringAsync();

            Assert.IsFalse(string.IsNullOrWhiteSpace(responseStrong.ToString()));
        }

        [TestMethod]
        public async Task Test_HotelsController_Room_WEBAPI_Is_Up()
        {
            var response = await Client.GetAsync(Constants.HOTELS_BY_ROOM_WEBAPI_URL);

            response.EnsureSuccessStatusCode();

            var responseStrong = await response.Content.ReadAsStringAsync();

            Assert.IsFalse(string.IsNullOrWhiteSpace(responseStrong.ToString()));
        }

        [TestMethod]
        public async Task Test_Get_Hotels_By_City_Limit_Rate_Exceeded()
        {
            for (int i = 0; i < 5; i++)
            {
                await Client.GetAsync(Constants.HOTELS_BY_CITY_WEBAPI_URL);
            }
            var response = await Client.GetAsync(Constants.HOTELS_BY_CITY_WEBAPI_URL);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            Assert.IsTrue(result == Constants.FORBIDDEN_CONTENT);
        }


        [TestMethod]
        public async Task Test_Get_Hotels_By_Room_Limit_Rate_Exceeded()
        {
            for (int i = 0; i < 5; i++)
            {
                await Client.GetAsync(Constants.HOTELS_BY_ROOM_WEBAPI_URL);
            }
            var response = await Client.GetAsync(Constants.HOTELS_BY_ROOM_WEBAPI_URL);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            Assert.IsTrue(result == Constants.FORBIDDEN_CONTENT);
        }

        [TestMethod]
        public async Task Test_Get_Hotels_By_City_Limit_Rate_Exceeded_And_Next_SubSequent_Request_Suspended()
        {
            for (int i = 0; i < 6; i++)
            {
                await Client.GetAsync(Constants.HOTELS_BY_CITY_WEBAPI_URL);
            }
            var response = await Client.GetAsync(Constants.HOTELS_BY_CITY_WEBAPI_URL);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            Assert.IsTrue(result == Constants.SUSPEND_CONTENT);
        }

        [TestMethod]
        public async Task Test_Get_Hotels_By_Room_Limit_Rate_Exceeded_And_Next_SubSequent_Request_Suspended()
        {
            for (int i = 0; i < 6; i++)
            {
                await Client.GetAsync(Constants.HOTELS_BY_ROOM_WEBAPI_URL);
            }
            var response = await Client.GetAsync(Constants.HOTELS_BY_ROOM_WEBAPI_URL);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            Assert.IsTrue(result == Constants.SUSPEND_CONTENT);
        }

        [TestMethod]
        public async Task Test_Get_Hotels_By_City_Limit_Rate_Exceeded_And_Get_Hotels_By_Room_Working()
        {
            for (int i = 0; i < 6; i++)
            {
                await Client.GetAsync(Constants.HOTELS_BY_CITY_WEBAPI_URL);
            }
            var response = await Client.GetAsync(Constants.HOTELS_BY_ROOM_WEBAPI_URL);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            Assert.IsFalse(result != Constants.FORBIDDEN_CONTENT);
        }

        [TestMethod]
        public async Task Test_Get_Hotels_By_Room_Limit_Rate_Exceeded_And_Get_Hotels_By_City_Working()
        {
            for (int i = 0; i < 6; i++)
            {
                await Client.GetAsync(Constants.HOTELS_BY_ROOM_WEBAPI_URL);
            }
            var response = await Client.GetAsync(Constants.HOTELS_BY_CITY_WEBAPI_URL);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            Assert.IsFalse(result != Constants.FORBIDDEN_CONTENT);
        }

        [TestMethod]
        public async Task Test_Get_Hotels_By_City_Suspended_And_Get_Hotels_By_Room_Working()
        {
            for (int i = 0; i < 6; i++)
            {
                await Client.GetAsync(Constants.HOTELS_BY_CITY_WEBAPI_URL);
            }
            var response = await Client.GetAsync(Constants.HOTELS_BY_ROOM_WEBAPI_URL);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            Assert.IsFalse(result != Constants.SUSPEND_CONTENT);
        }

        [TestMethod]
        public async Task Test_Get_Hotels_By_Room_Suspended_And_Get_Hotels_By_City_Working()
        {
            for (int i = 0; i < 6; i++)
            {
                await Client.GetAsync(Constants.HOTELS_BY_ROOM_WEBAPI_URL);
            }
            var response = await Client.GetAsync(Constants.HOTELS_BY_CITY_WEBAPI_URL);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            Assert.IsFalse(result != Constants.SUSPEND_CONTENT);
        }

    }
}