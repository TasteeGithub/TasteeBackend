
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TTBackEnd.Shared
{
    public static class Utils
    {
        public static async Task<string> GetApiAsync(HttpClient client, string apiLink)
        {
            string result = string.Empty;
            string baseUrl = apiLink;
            using (HttpResponseMessage res = await client.GetAsync(baseUrl))
            using (HttpContent content = res.Content)
            {
                string data = await content.ReadAsStringAsync();
                if (data != null)
                {
                    result = data;
                }
            }
            return result;
        }

        public static async Task<string> PostApiAsync(HttpClient client,string relativeUri, object model)
        {
            try
            {
                Uri uri = new Uri(relativeUri);
                HttpContent httpContent = CreateHttpContent(model);
                HttpResponseMessage response = await client.PostAsync(uri, httpContent);

                if (!response.IsSuccessStatusCode)
                {
                    return string.Empty;
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                System.Diagnostics.Debug.WriteLine(jsonResponse);

                return jsonResponse;
            }
            catch (Exception ex)
            {
                //TODO: log loi
                return ex.Message;
            }
        }

        public static async Task<string> PutApiAsync(HttpClient client,string relativeUri, object model)
        {
            try
            {
                Uri uri = new Uri(relativeUri);
                HttpContent httpContent = CreateHttpContent(model);
                HttpResponseMessage response = await client.PutAsync(uri, httpContent);

                if (!response.IsSuccessStatusCode)
                {
                    return string.Empty;
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                System.Diagnostics.Debug.WriteLine(jsonResponse);

                return jsonResponse;
            }
            catch (Exception ex)
            {
                //TODO: log loi
                return string.Empty;
            }
        }

        public static HttpContent CreateHttpContent<T>(T content)
        {
            var json = JsonConvert.SerializeObject(content, MicrosoftDateFormatSettings);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private static JsonSerializerSettings MicrosoftDateFormatSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };
            }
        }

        public static string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }

        //public static DateRange ParseDateRange(string dateRange, char splitChar)
        //{
        //    return new DateRange()
        //    {
        //        FromDate = DateTime.ParseExact(dateRange.Split(splitChar)[0].Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
        //        ToDate = DateTime.ParseExact(dateRange.Split(splitChar)[1].Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture)
        //    };
        //}
    }
}