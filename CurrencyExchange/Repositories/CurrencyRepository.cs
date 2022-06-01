using CurrencyExchange.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CurrencyExchange.Repositories
{
    public class CurrencyRepository : ICurrencyRepository<Currency>
    {
        readonly private List<Currency> currencies = new();
        readonly HttpClient Client = new();

        public CurrencyRepository()
        {
            Client = new();
            Client.BaseAddress = new Uri("https://api.exchangerate.host/");
        }

        /* Returns a collection with the rates relative to EUR */
        public async Task<IEnumerable<Currency>> GetRatesEuroAsync()
        {
            HttpResponseMessage response = await Client.GetAsync("latest");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var jRoot = JObject.Parse(responseBody);

            if (jRoot is not null)
            {
                var rates = jRoot.SelectToken("rates");
                if (rates is not null)
                {
                    var dict = rates.ToObject<Dictionary<string, double>>();
                    if (dict is not null)
                    {
                        foreach (var item in dict)
                        {
                            currencies.Add(new Currency(item.Key, item.Value));
                        }
                    }
                }
            }

            return currencies;
        }

        /* Returns a collection with the rates relative to EUR of the date specified */
        public async Task<IEnumerable<Currency>> GetHistoric(string dateTime)
        {
            HttpResponseMessage response = await Client.GetAsync(dateTime);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var jRoot = JObject.Parse(responseBody);

            if (jRoot is not null)
            {
                var rates = jRoot.SelectToken("rates");
                if (rates != null)
                {
                    var dict = rates.ToObject<Dictionary<string, double>>();
                    if (dict is not null)
                    {
                        foreach (var item in dict)
                        {
                            currencies.Add(new Currency(item.Key, item.Value));
                        }
                    }
                }
            }
            return currencies;
        }
    }
}
