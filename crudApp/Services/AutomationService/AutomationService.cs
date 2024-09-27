using crudApp.Persistence.Contexts;
using crudApp.Persistence.Models;
using crudApp.Services.AutomationService.DTOs;
using HtmlAgilityPack;
using System.Globalization;

namespace crudApp.Services.AutomationService
{
    public class AutomationService : IAutomationService
    {

        private readonly ApplicationDbContext _context; // database context

        public AutomationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> RunAutomation(AutomationParameters parameters)
        {
            int recordsUpdatedTotal = 0;
            HttpClient httpClient = new();

            //// if a website gives a forbidden response, adding these headers can fix the issue
            //httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
            //httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            //httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.5");
            //httpClient.DefaultRequestHeaders.Add("Referer", "https://www.dolan-bikes.com/");

            // build a url and retrieve an html document
            string url = $"https://www.dolan-bikes.com/{parameters.pageUrl}/";
            string html = await httpClient.GetStringAsync(url);
            HtmlDocument htmlDocument = new();
            htmlDocument.LoadHtml(html);

            // XPath to select all product containers
            string xpath = ".//section[contains(@class, 'product-bike-category')]";
            HtmlNodeCollection productNodes = htmlDocument.DocumentNode.SelectNodes(xpath);

            // check if found anything
            if (productNodes == null) 
            {
                return 0;
            }

            // remove existing products
            List<Product> existingProducts = _context.Products.ToList();
            _context.RemoveRange(existingProducts);

            foreach (HtmlNode productNode in productNodes)
            {

                // get the product title div and inner text
                string titleText = productNode.SelectSingleNode(".//a[contains(@class, 'product-bike-title')]").InnerText;
                Console.WriteLine(titleText);

                // get the product price div and trim the inner text 
                string priceText = productNode.SelectSingleNode(".//div[@class='product-bike-price']").InnerText;
                priceText = priceText.Replace("From", "").Trim();
                priceText = priceText.Replace("£", "").Trim();

                // try to parse the price as a decimal
                if (decimal.TryParse(priceText, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal price))
                {
                    Console.WriteLine($"Price: {price}");

                    // if all goes well, create a new product
                    Product newProduct = new();
                    newProduct.Name = titleText;
                    newProduct.Price = price;

                    _ = _context.Products.Add(newProduct);
                }
                else
                {
                    Console.WriteLine("invalid price");
                }
            }

            // save changes to the database
            recordsUpdatedTotal = await _context.SaveChangesAsync();

            // return the records change count
            return recordsUpdatedTotal;

        }
    }
}
