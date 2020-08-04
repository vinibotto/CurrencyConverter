using CurrencyConverter.Controls;
using CurrencyConverter.Data;
using CurrencyConverter.Domain;
using CurrencyConverter.ViewModel.Helpers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CurrencyConverter.View
{
    /// <summary>
    /// Interaction logic for ConverterWindow.xaml
    /// </summary>
    public partial class ConverterWindow : Window
    {
        private readonly ICurrencyApiService _currencyApiService;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly AppSettings _appSettings;


        public ConverterWindow(ICurrencyApiService currencyApiService, IOptions<AppSettings> appSettings, ICurrencyRepository currencyRepository)
        {
            InitializeComponent();

            _appSettings = appSettings.Value;
            _currencyApiService = currencyApiService;
            _currencyRepository = currencyRepository;


            LoadList();
        }

        private void BtnAddCurrency_Click(object sender, RoutedEventArgs e)
        {
            NewCurrencyWindow window = new NewCurrencyWindow();
            window.ShowDialog();

            LoadList();
        }

        private void BtnAddConvertedCurrency_Click(object sender, RoutedEventArgs e)
        {
            NewCurrencyWindow window = new NewCurrencyWindow();
            window.ShowDialog();

            LoadList();
        }

        void LoadList()
        {
            var currenciesList = _currencyRepository.GetCurrencies();
            
            lvwConvertedCurrencies.ItemsSource = currenciesList;
        }

        private void lvwCurrencies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (lvwCurrencies.SelectedItem == null)
            //{
            //    lblCurrency.Content = "";
            //    btnConvert.IsEnabled = false;
            //}
            //else 
            //{
            //    lblCurrency.Content = ((Currency)lvwCurrencies.SelectedItem).Code;
            //    btnConvert.IsEnabled = true;
            //}
            
        }


        private void btnConvert_Click(object sender, RoutedEventArgs e)
        {
            string code = cmbCurrencies.Text;
            
            double.TryParse(textValue.Text, out double parsed);

            if (lvwConvertedCurrencies == null || lvwConvertedCurrencies.ItemsSource == null || lvwConvertedCurrencies.Items.IsEmpty)
                return;

            Dictionary<string, double> rates = _currencyApiService.GetConversionRate(code);
            if (rates == null) 
            { 
                MessageBox.Show("Conversion not available for this currency.");
                return;
            }
            List<Currency> updatedValues = new List<Currency>();
            foreach (var item in lvwConvertedCurrencies.Items)
            {
                var currencyObj = item as Currency;
                rates.TryGetValue(currencyObj.Code.ToUpper(), out double rate);
                var conversion = parsed * rate;
                currencyObj.Amount = conversion;
                updatedValues.Add(currencyObj);
            }

            lvwConvertedCurrencies.ItemsSource = updatedValues;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var btnItem = sender as Button;
            Currency currency = (btnItem.DataContext as Currency);
            _currencyRepository.DeleteCurrency(currency);
            LoadList();
        }

        private void cmbCurrencies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = cmbCurrencies.SelectedItem as ComboBoxItem;
            if (string.IsNullOrEmpty((string)item.Content))
            {
                lblCurrency.Content = "";
                btnConvert.IsEnabled = false;
            }
            else
            {
                lblCurrency.Content = (string)item.Content;
                btnConvert.IsEnabled = true;
            }
        }
    }
}
