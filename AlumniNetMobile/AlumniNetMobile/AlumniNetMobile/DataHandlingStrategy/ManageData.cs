﻿using AlumniNetMobile.Common;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AlumniNetMobile.DataHandlingStrategy
{
    public class ManageData : IManageData
    {
        private ManageDataStrategy _manageDataStrategy;
        private HttpClient _httpClient;

        public void SetStrategy(ManageDataStrategy manageDataStrategy)
        {
            _manageDataStrategy = manageDataStrategy;
        }


        public async Task<T> GetDataAndDeserializeIt<T>(string url, string json = "", string token = "")
        {
            _httpClient = new HttpClient(DependencyService.Get<IHttpClientHandlerService>().GetInsecureHandler());
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            if (token.Length > 0)
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var data = await _manageDataStrategy.ManageData(_httpClient, url, json);

            if (data == string.Empty)
                Console.WriteLine(url + " returned an empty string"); //DO NOT REMOVE BKPOINT WHILE DEBUGGING
            var deserializedData = JsonConvert.DeserializeObject<T>(data);

            return deserializedData;
        }

    }
}
