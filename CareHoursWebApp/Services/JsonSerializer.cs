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
    public class JsonSerializer<T> where T : class
    {
        private const string JSON_CONTENT_TYPE = "application/json";
        private readonly Encoding ENCODING = Encoding.UTF8;

        public String Serialize(T t)
        {
            var ser = new DataContractJsonSerializer(typeof(T));
            var ms = new MemoryStream();
            ser.WriteObject(ms, t);
            byte[] json = ms.ToArray();
            return ENCODING.GetString(json, 0, json.Length);
        }

        public async Task<T> DeserializeAsync(Task<Stream> s)
        {
            var ser = new DataContractJsonSerializer(typeof(T));
            return ser.ReadObject(await s) as T;
        }

        public StringContent JsonHttpStringContent(T t)
        {
            return new StringContent(Serialize(t), ENCODING, JSON_CONTENT_TYPE);
        }
    }
}
