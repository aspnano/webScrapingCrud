using crudApp.Services.AutomationService.DTOs;
using Newtonsoft.Json;

namespace crudApp.Services.AutomationService
{
    public class AutomationServiceV2 : IAutomationService
    {
        public async Task<int> RunAutomation()
        {

            // here is how you can obtain an api source
            string apiUrl = $"https://catfact.ninja/facts";
            HttpClient httpClient = new();
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json;api_version=2");
            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();
            string jsonResponse = await response.Content.ReadAsStringAsync();
            CatFactsResponseDTO responseObject = JsonConvert.DeserializeObject<CatFactsResponseDTO>(jsonResponse);

            // once the data is deserialized into DTOs you can do anything with them
            foreach (CatFactDTO item in responseObject.Data)
            {
                Console.WriteLine(item.Fact);             
            }

            int recordsUpdatedTotal = 0;
            return recordsUpdatedTotal;
        }
    }
}
