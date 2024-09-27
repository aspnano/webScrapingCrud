using crudApp.Services.AutomationService.DTOs;
using Newtonsoft.Json;
using System.Globalization;

namespace crudApp.Services.AutomationService
{
    public class AutomationServiceV2 : IAutomationService
    {

        public async Task<int> RunAutomation(AutomationParameters parameters)
        {

            int recordsUpdatedTotal = 0;

            // here is how you can obtain an api source
            string apiUrl = $"https://www.antarctica21.com/wp-json/filter/expeditions";
            HttpClient httpClient = new();
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json;api_version=2");
            HttpResponseMessage response = await httpClient.PostAsync(apiUrl, null);
            response.EnsureSuccessStatusCode();
            string jsonResponse = await response.Content.ReadAsStringAsync();
            ExpeditionResponseDTO responseObject = JsonConvert.DeserializeObject<ExpeditionResponseDTO>(jsonResponse);


            // once the data is deserialized into DTOs you can do anything with them
            foreach (ExpeditionItemDto result in responseObject.Result)
            {
                Console.WriteLine(result.ProductName + "-" + result.StartDate);
                if (decimal.TryParse(result.StartPrice, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal price))
                {
                    Console.WriteLine($"Price: {price}");
                }
                else
                {
                    Console.WriteLine("invalid price");
                }
            }


            return recordsUpdatedTotal;
        }
    }
}
