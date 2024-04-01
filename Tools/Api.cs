using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace kurs11135.Tools
{
    internal static class Api
    {
        static HttpClient client = new HttpClient();
        static string host = "https://localhost:7199/api/";
        static JsonSerializerOptions options = new JsonSerializerOptions
        {
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,
            PropertyNameCaseInsensitive = true
        };


        public static async Task<string> Post(string controller, object body, string method)
        {
            try
            {
                string url = host + controller;
                if (!string.IsNullOrEmpty(method))
                    url += $"/{method}";

                string json = "";
                if (body != null)
                {
                    json = JsonSerializer.Serialize(body, body.GetType(), options);
                    if (HasUnsafeCommands(json))
                    {
                        MessageBox.Show("Введены недопустимые команды");
                        return "";
                    }
                }
                var response = await client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsStringAsync();
                else
                {
                    MessageBox.Show(await response.Content.ReadAsStringAsync());
                    return "";
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return "";
            }
        }

        public static T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, options);
        }

        private static bool HasUnsafeCommands(string input)
        {
            return false;
        }


        public static async Task<string> Put(string controller, object body, string method)
        {
            try
            {
                string url = host + controller;
                if (!string.IsNullOrEmpty(method))
                    url += $"/{method}";

                string json = "";
                if (body != null)
                {
                    json = JsonSerializer.Serialize(body, body.GetType(), options);
                    if (HasUnsafeCommands(json))
                    {
                        MessageBox.Show("ERROR :D");
                        return "";
                    }
                }

                var response = await client.PutAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsStringAsync();
                else
                {
                    MessageBox.Show(await response.Content.ReadAsStringAsync());
                    return "";
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return "";
            }
        }





    }
}
