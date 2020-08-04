using CurrencyConverter.Domain;
using Microsoft.Extensions.Options;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyConverter.Data
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly IOptions<AppSettings> _appSettings;

        public CurrencyRepository(IOptions<AppSettings> appSettings) 
        {
            _appSettings = appSettings;   
        }

        public bool AddCurrency(Currency currency)
        {
            using SQLiteConnection connection = new SQLiteConnection(_appSettings.Value.CurrencyDb);
            return connection.Insert(currency) > 0;
        }

        public void CreateTable()
        {
            using SQLiteConnection connection = new SQLiteConnection(_appSettings.Value.CurrencyDb);
            connection.CreateTable<Currency>();
        }

        public void DeleteCurrency(Currency currency)
        {
            using SQLiteConnection connection = new SQLiteConnection(_appSettings.Value.CurrencyDb);
            connection.Delete(currency);
        }

        public IEnumerable<Currency> GetCurrencies()
        {
            using SQLiteConnection connection = new SQLiteConnection(_appSettings.Value.CurrencyDb);
            return (connection.Table<Currency>().ToList()).ToList();
        }

        public Currency GetCurrency(int id)
        {
            throw new NotImplementedException();
        }

        public void SetFavorite(int id, bool favorite)
        {
            throw new NotImplementedException();
        }
    }
}
