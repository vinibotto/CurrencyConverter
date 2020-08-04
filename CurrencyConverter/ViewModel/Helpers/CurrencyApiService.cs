using CurrencyConverter.Domain;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CurrencyConverter.ViewModel.Helpers
{
    public class CurrencyApiService : ICurrencyApiService
    {
        private readonly IOptions<AppSettings> _appSettings;
        
        public CurrencyApiService(IOptions<AppSettings> appSettings) 
        {
            _appSettings = appSettings;
        }

        public List<Currency> GetAvailableCurrencies()
        {
            Dictionary<string, double> currenciesDict = new Dictionary<string, double>();
            string url = $"{_appSettings.Value.BaseEndpoint}/{_appSettings.Value.ApiKey}/latest/USD";

            using (HttpClient client = new HttpClient())
            {
            
                var response = client.GetAsync(url).Result;
                string json = response.Content.ReadAsStringAsync().Result;

                CurrencyListResponse currencyListResponse = JsonConvert.DeserializeObject<CurrencyListResponse>(json);
                currencyListResponse.conversion_rates.TryGetValue("conversion_rates", out var outObj);
                currenciesDict = JObject.FromObject(outObj).ToObject<Dictionary<string, double>>();
            }
            
            return currenciesDict.Select(c=> new Currency { 
                Code = c.Key,
                Name = TryGetCurrencyName(c.Key)
            }).ToList();
        }

        public Dictionary<string, double> GetConversionRate(string code)
        {
            Dictionary<string, double> currenciesDict = new Dictionary<string, double>();
            string url = $"{_appSettings.Value.BaseEndpoint}/{_appSettings.Value.ApiKey}/latest/{code}";

            using (HttpClient client = new HttpClient())
            {

                var response = client.GetAsync(url).Result;
                string json = response.Content.ReadAsStringAsync().Result;

                CurrencyListResponse currencyListResponse = JsonConvert.DeserializeObject<CurrencyListResponse>(json);
                if (!currencyListResponse.conversion_rates.TryGetValue("conversion_rates", out var outObj))
                    return null;

                currenciesDict = JObject.FromObject(outObj).ToObject<Dictionary<string, double>>();
            }

            return currenciesDict;
        }

        private string TryGetCurrencyName(string ISOCurrencyCode)
        {
            var name = CultureInfo
                .GetCultures(CultureTypes.AllCultures)
                .Where(c => !c.IsNeutralCulture)
                .Select(culture => {
                    try
                    {
                        return new RegionInfo(culture.Name);
                    }
                    catch
                    {
                        return null;
                    }
                })
                .Where(ri => ri != null && ri.ISOCurrencySymbol == ISOCurrencyCode)
                .Select(ri => ri.CurrencyEnglishName)
                .FirstOrDefault();
            return name;
        }
    }

    public class CurrencyListResponse 
    {
        [JsonExtensionData]
        public Dictionary<string, object> conversion_rates { get; set; }
    }
}
