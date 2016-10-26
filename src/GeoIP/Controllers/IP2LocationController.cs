﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using GeoIP.Utils;
using Newtonsoft.Json;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using GeoIP.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GeoIP.Controllers
{
    [Route("api/[controller]")]
    public class IP2LocationController : Controller
    {
        private readonly IHostingEnvironment hostingEnvironment;

        public IP2LocationController(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        // GET api/ip2location?ipaddress=123.123.123.123
        [HttpGet]
        public string GetGeoLocation([FromQuery]string ipAddress)
        {
            var error = new Validation()
                                .IsNotNullOrEmpty(ipAddress)
                                .IsIPAddress(ipAddress)
                                .Validate();

            if (error != null)
            {
                return JsonConvert.SerializeObject(error);
            }


            var result = QueryWebService(ipAddress).Result;
            

            return result;
        }

        private async Task<string> QueryWebService(string ipAddress)
        {
            var url = $"http://localhost:3000/ip2location?ipaddress={ipAddress}";

            using (var client = new HttpClient())
            using (var response = await client.GetAsync(url))
            using (var content = response.Content)
            {
                var queriedResult = await content.ReadAsStringAsync();

                return queriedResult;
            }
        }
    }
}
