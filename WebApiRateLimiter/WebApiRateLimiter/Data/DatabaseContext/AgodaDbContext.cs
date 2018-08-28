using WebApiRateLimiter.Data.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WebApiRateLimiter.Data.DatabaseContext
{
    public static class AgodaDbContext
    {
        public static IEnumerable<Hotel> Hotels { get; set; }

        static AgodaDbContext()
        {
            Hotels = File.ReadAllLines(@"Data\DatabaseContext\MockData\hoteldb.csv")
                                           .Skip(1)
                                           .Select(v => ConvertCsvToHotel(v))
                                           .ToList();
        }

        public static Hotel ConvertCsvToHotel(string line)
        {
            string[] values = line.Split(',');
            var hotel = new Hotel
            {
                City = values[0],
                HotelId = Convert.ToInt32(values[1]),
                Room = values[2],
                Price = Convert.ToInt32(values[3])
            };
            return hotel;
        }
    }
}