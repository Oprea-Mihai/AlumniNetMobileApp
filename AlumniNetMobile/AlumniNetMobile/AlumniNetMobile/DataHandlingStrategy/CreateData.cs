﻿using AlumniNetMobile.Common;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AlumniNetMobile.DataHandlingStrategy
{
    public class CreateData : ManageDataStrategy
    {
        public override async Task<string> ManageData(HttpClient httpClient, string endPoint, string json)
        {
            var uri = new Uri(Uri + endPoint);
            try
            {
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(uri, stringContent);

                if (!response.IsSuccessStatusCode) return string.Empty;

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

    }
}
