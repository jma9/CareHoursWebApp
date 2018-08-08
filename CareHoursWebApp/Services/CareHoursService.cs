﻿using CareHoursWebApp.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace CareHoursWebApp.Services
{
    public class CareHoursService : ICareHoursService
    {
        private const string JSON_CONTENT_TYPE = "application/json";
        private const string CAREHOURS_BASE_URI = "https://jma.azure-api.net/api/child/{0}/Carehours";
        private const string SUBSCRIPTION_KEY_HEADER = "Ocp-Apim-Subscription-Key";

        private HttpClient client = new HttpClient();

        public CareHoursService(IConfiguration configuration)
        {
            client.DefaultRequestHeaders.Add(SUBSCRIPTION_KEY_HEADER, configuration["AppSettings:SubscriptionKey"]);
        }

        private List<CareHours> JsonDeserializeCarehoursList(String jsonCarehours)
        {
            var carehoursListSerializer = new DataContractJsonSerializer(typeof(List<CareHours>));
            return carehoursListSerializer.ReadObject(new MemoryStream(Encoding.UTF8.GetBytes(jsonCarehours))) as List<CareHours>;
        }
        private String JsonSerializeCareHours(CareHours careHours)
        {
            var careHoursSerializer = new DataContractJsonSerializer(typeof(CareHours));
            var ms = new MemoryStream();
            careHoursSerializer.WriteObject(ms, careHours);
            byte[] json = ms.ToArray();
            return Encoding.UTF8.GetString(json, 0, json.Length);
        }

        public IEnumerable<CareHours> GetCareHoursForChild(int childId)
        {
            return JsonDeserializeCarehoursList(client.GetAsync(String.Format(CAREHOURS_BASE_URI, childId)).Result.Content.ReadAsStringAsync().Result);
        }

        public void Create(CareHours careHours)
        {
            var jsonResponse = client.PostAsync(String.Format(CAREHOURS_BASE_URI, careHours.ChildId),
                new StringContent(JsonSerializeCareHours(careHours), Encoding.UTF8, JSON_CONTENT_TYPE)).Result.Content.ReadAsStringAsync().Result;
        }
    }
}