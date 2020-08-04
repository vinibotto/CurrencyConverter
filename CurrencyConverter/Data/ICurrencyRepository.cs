using CurrencyConverter.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace CurrencyConverter.Data
{
    public interface ICurrencyRepository
    {
        public void CreateTable();

        public void SetFavorite(int id, bool favorite);

        public bool AddCurrency(Currency currency);

        public void DeleteCurrency(Currency id);

        public Currency GetCurrency(int id);

        public IEnumerable<Currency> GetCurrencies();
    }
}
