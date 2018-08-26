using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebApiRateLimiter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        // GET api/hotels/city
        [HttpGet("city")]
        public ActionResult<IEnumerable<string>> GetHotelsByCity()
        {
            return new string[] { "hotel1", "hotel2" };
        }

        // GET api/hotels/room
        [HttpGet("room")]
        public ActionResult<IEnumerable<string>> GetHotelsByRoomType()
        {
            return new string[] { "hotel3", "hotel4" };
        }
    }
}