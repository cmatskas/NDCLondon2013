﻿using System;
using System.Net.Http;
using System.Threading;
using Newtonsoft.Json;

namespace BackEndSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = "http://localhost:46246/backend";

            using (var http = new HttpClient())
            {
                while (true)
                {
                    var message = new
                    {
                        message = String.Format("Time from back-end system: {0}", DateTimeOffset.Now)
                    };

                    http.PostAsync(url, new StringContent(JsonConvert.SerializeObject(message))).Wait();

                    Console.WriteLine("Sent message {0}", message);

                    Thread.Sleep(1000);
                }
            }
        }
    }
}
