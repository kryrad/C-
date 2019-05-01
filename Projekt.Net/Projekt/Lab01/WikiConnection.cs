using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace Lab01
{
    class WikiConnection
    {
        static readonly string Url = "https://en.wikipedia.org/wiki/Special:Random";

        public static async Task<string> LoadDataAsync()
        {
            try
            {
                Task<string> result;
                using (HttpClient client = new HttpClient())
                using (HttpResponseMessage response = await client.GetAsync(Url))
                using (HttpContent content = response.Content)
                {
                    result = content.ReadAsStringAsync();
                }
                return await result;
            }
            catch (Exception)
            {
                MessageBox.Show("Connection error");
                return string.Empty;
            }
        }
    }
}
