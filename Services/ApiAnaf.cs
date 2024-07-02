using CRM_App.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace CRM_App.Services
{
    public class ApiAnaf
    {
        public static async Task<Customer> GetCustomerFromAnaf(string CUI)
        {
			try
			{
				string apiURL = "https://webservicesp.anaf.ro/PlatitorTvaRest/api/v8/ws/tva";

				var payLoad = new List<object>
				{
					new { cui = CUI, data = DateTime.Now.ToString("yyyy-MM-dd") }
                };

                var response = await PostJsonAsync(apiURL, payLoad);

                var parsedResponse = JsonConvert.DeserializeObject<dynamic>(response);

                if (parsedResponse.cod == 200 && parsedResponse.found != null)
                {
                    var dateGenerale = parsedResponse.found[0].date_generale;

                    Customer customer = new()
                    {
                        Name = dateGenerale.denumire,
                        Address = dateGenerale.adresa,
                        Phone = dateGenerale.telefon
                    };

                    return customer;
                }
                else
                {
                    return null;
                }
            }
			catch (Exception ex) 
			{
                Debug.WriteLine(ex.StackTrace + Environment.NewLine + ex.Message);
           		return null;
			}
        }

        public static async Task<string> PostJsonAsync(string url, object data)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(data);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(url, content);
                    response.EnsureSuccessStatusCode();

                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine(ex.StackTrace + Environment.NewLine + ex.Message);
                return null;
            }
            
        }

    }
}
