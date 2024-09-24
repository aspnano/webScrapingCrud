using crudApp.Persistence.Contexts;
using crudApp.Services.AutomationService.DTOs;
using HtmlAgilityPack;
using System.Globalization;
using System.Text.RegularExpressions;

namespace crudApp.Services.AutomationService
{
    public class AutomationService : IAutomationService
    {

        private readonly ApplicationDbContext _context; // database context

        public AutomationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> RunAutomation()
        {
            int recordsUpdatedTotal = 0;


            HttpClient httpClient = new();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
            httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.5");
            httpClient.DefaultRequestHeaders.Add("Referer", "https://www.purecycles.com/");

            string url = $"https://www.purecycles.com/collections/bicycles";

            string html = await httpClient.GetStringAsync(url);
            HtmlDocument htmlDocument = new();
            htmlDocument.LoadHtml(html);

            // XPath to select all voyage rows

            string xpath = ".//div[contains(@class, 'grid-product__content')]";
            HtmlNodeCollection productNodes = htmlDocument.DocumentNode.SelectNodes(xpath);

            if (productNodes == null) // check if found anything
            {
                return 0;
            }

            foreach (HtmlNode productNode in productNodes)
            {
                string titleText = productNode.SelectSingleNode(".//div[contains(@class, 'grid-product__title')]").InnerText;
                Console.WriteLine(titleText);

                string priceText = productNode.SelectSingleNode(".//div[contains(@class, 'grid-product__price')]").InnerText;
                priceText = priceText.Replace("$", "").Trim();

                // Convert to decimal
                if (decimal.TryParse(priceText, NumberStyles.Currency, CultureInfo.InvariantCulture, out decimal priceResult))
                {
                    Console.WriteLine($"The price is: {priceResult}");
                } else
                {
                    Console.WriteLine($"Invalid Price");

                    string salePricePattern = @"(?<=from\s)\$\d+\.\d+";
                    Match match = Regex.Match(priceText, salePricePattern);
                    if (match.Success)
                    {
                        string price = match.Groups[1].Value;
                        Console.WriteLine($"The price is: {price}");
                    }
                    else
                    {
                        Console.WriteLine("Price not found in the HTML snippet.");
                    }
                }

            }



            return recordsUpdatedTotal;

        }
    }
}
