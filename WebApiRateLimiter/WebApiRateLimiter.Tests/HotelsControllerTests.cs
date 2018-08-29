using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiRateLimiter.Data.Model;

namespace WebApiRateLimiter.Tests
{
    [TestClass]
    public class HotelsControllerTests : TestServerFixture
    {
        [TestMethod]
        public async Task Test_HotelsController_City_WEBAPI_Is_Ok()
        {
            var response = await Client.GetAsync(Constants.HOTELS_BY_CITY_WEBAPI_URL);

            response.EnsureSuccessStatusCode();

            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task Test_HotelsController_Room_WEBAPI_Is_Ok()
        {
            var response = await Client.GetAsync(Constants.HOTELS_BY_ROOM_WEBAPI_URL);

            response.EnsureSuccessStatusCode();

            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task Test_HotelsController_City_WEBAPI_Is_Returns_Hotel()
        {
            var response = await Client.GetAsync(Constants.HOTELS_BY_CITY_WEBAPI_URL);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            var hotels = JsonConvert.DeserializeObject<IEnumerable<Hotel>>(result);

            Assert.IsTrue(hotels.Count() != 0);
        }

        [TestMethod]
        public async Task Test_HotelsController_Room_WEBAPI_Is_Returns_Hotel()
        {
            var response = await Client.GetAsync(Constants.HOTELS_BY_ROOM_WEBAPI_URL);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            var hotels = JsonConvert.DeserializeObject<IEnumerable<Hotel>>(result);

            Assert.IsTrue(hotels.Count() != 0);
        }

        [TestMethod]
        public async Task Test_HotelsController_City_WEBAPI_Is_Returns_Hotels_Sorted_Asc_By_Price()
        {
            var response = await Client.GetAsync(Constants.HOTELS_BY_CITY_WEBAPI_URL + "/asc");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            var hotels = JsonConvert.DeserializeObject<IEnumerable<Hotel>>(result).ToList();

            Assert.IsTrue(hotels.Min(x => x.Price) == hotels.First().Price);
            Assert.IsTrue(hotels.Max(x => x.Price) == hotels.Last().Price);
        }

        [TestMethod]
        public async Task Test_HotelsController_City_WEBAPI_Is_Returns_Hotels_Sorted_Desc_By_Price()
        {
            var response = await Client.GetAsync(Constants.HOTELS_BY_CITY_WEBAPI_URL + "/desc");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            var hotels = JsonConvert.DeserializeObject<IEnumerable<Hotel>>(result).ToList();

            Assert.IsTrue(hotels.Min(x => x.Price) == hotels.Last().Price);
            Assert.IsTrue(hotels.Max(x => x.Price) == hotels.First().Price);
        }

        [TestMethod]
        public async Task Test_HotelsController_Room_WEBAPI_Is_Returns_Hotels_Sorted_Asc_By_Price()
        {
            var response = await Client.GetAsync(Constants.HOTELS_BY_ROOM_WEBAPI_URL + "/asc");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            var hotels = JsonConvert.DeserializeObject<IEnumerable<Hotel>>(result).ToList();

            Assert.IsTrue(hotels.Min(x => x.Price) == hotels.First().Price);
            Assert.IsTrue(hotels.Max(x => x.Price) == hotels.Last().Price);
        }

        [TestMethod]
        public async Task Test_HotelsController_Room_WEBAPI_Is_Returns_Hotels_Sorted_Desc_By_Price()
        {
            var response = await Client.GetAsync(Constants.HOTELS_BY_ROOM_WEBAPI_URL + "/desc");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            var hotels = JsonConvert.DeserializeObject<IEnumerable<Hotel>>(result).ToList();

            Assert.IsTrue(hotels.Min(x => x.Price) == hotels.Last().Price);
            Assert.IsTrue(hotels.Max(x => x.Price) == hotels.First().Price);
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

            Assert.IsTrue(result != Constants.FORBIDDEN_CONTENT);
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

            Assert.IsTrue(result != Constants.FORBIDDEN_CONTENT);
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

            Assert.IsTrue(result != Constants.SUSPEND_CONTENT);
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

            Assert.IsTrue(result != Constants.SUSPEND_CONTENT);
        }

        [TestMethod]
        public async Task Test_Hotels_Services_Failing_Due_To_Wrong_End_Point()
        {
            var response = await Client.GetAsync("api/abc");

            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NotFound);
        }
    }
}