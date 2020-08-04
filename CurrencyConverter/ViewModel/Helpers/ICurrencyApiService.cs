using CurrencyConverter.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CurrencyConverter.ViewModel.Helpers
{
    public interface ICurrencyApiService
    {
        public List<Currency> GetAvailableCurrencies();

        public Dictionary<string, double> GetConversionRate(string code);
    }
}
