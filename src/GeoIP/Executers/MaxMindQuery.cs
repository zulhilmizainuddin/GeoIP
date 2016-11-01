﻿using GeoIP.Models;
using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoIP.Executers
{
    public class MaxMindQuery : IQuery
    {
        public async Task<string> Query(string ipAddress, string dataSource)
        {
            Object result;

            try
            {
                using (var reader = new DatabaseReader(dataSource))
                {
                    var city = reader.City(ipAddress);

                    result = new Geolocation()
                    {
                        IPAddress = ipAddress,
                        City = city.City.Name,
                        Country = city.Country.Name,
                        Latitude = city.Location.Latitude,
                        Longitude = city.Location.Longitude
                    };

                }
            }
            catch (AddressNotFoundException ex)
            {
                result = new Error()
                {
                    ErrorMessage = ex.Message
                };
            }

            return JsonConvert.SerializeObject(result);
        }
    }
}
