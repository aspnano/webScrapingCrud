using crudApp.Persistence.Contexts;
using crudApp.Persistence.Models;
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

        public async Task<int> RunAutomation()
        {
            int recordsUpdatedTotal = 0;
            HttpClient httpClient = new();

            // if a website gives a forbidden response, adding these headers can fix the issue, but they are not always necessary
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
            httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.5");
            httpClient.DefaultRequestHeaders.Add("Referer", "https://boardgamegeek.com/");

            // build a url and retrieve an html document
            string url = $"https://boardgamegeek.com/browse/boardgame";
            string html = await httpClient.GetStringAsync(url);
            HtmlDocument htmlDocument = new();
            htmlDocument.LoadHtml(html);

            // XPath to select all product containers
            string xpath = ".//table[@class='collection_table']//tr[@id='row_']";

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
                string titleText = productNode.SelectSingleNode(".//a[contains(@class, 'primary')]").InnerText.Trim();
                Console.WriteLine(titleText);

                // get the product rating div and inner text 
                string ratingText = productNode.SelectSingleNode(".//td[@class='collection_bggrating']").InnerText.Trim();

                // try to parse the rating as a decimal
                if (decimal.TryParse(ratingText, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal rating))
                {
                    Console.WriteLine($"Rating: {rating}");

                    // if all goes well, create a new product
                    Product newProduct = new();
                    newProduct.Name = titleText;
                    newProduct.Rating = rating;

                    _ = _context.Products.Add(newProduct);
                }
                else
                {
                    Console.WriteLine("invalid rating");
                }
            }

            // save changes to the database
            recordsUpdatedTotal = await _context.SaveChangesAsync();

            // return the records change count
            return recordsUpdatedTotal;

        }
    }
}
