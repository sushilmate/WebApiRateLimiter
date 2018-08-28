using System;
using System.Net.Http;

namespace WebApiRateLimiter.Demo
{

    namespace OwinSelfhostSample
    {
        public class Program
        {
            static void Main()
            {
                string baseAddress = "http://localhost:63131/";

                // Create HttpCient and make a request to api/values 
                HttpClient client = new HttpClient();
                

                var response = client.GetAsync(baseAddress + "api/city/bangkok").Result;

                Console.WriteLine(response);
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                Console.ReadLine();
            }
        }
    }
}