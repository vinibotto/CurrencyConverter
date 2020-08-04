using CurrencyConverter.Data;
using CurrencyConverter.Domain;
using CurrencyConverter.ViewModel.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace CurrencyConverter.View
{
    /// <summary>
    /// Interaction logic for NewCurrencyWindow.xaml
    /// </summary>
    public partial class NewCurrencyWindow : Window
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly ICurrencyApiService _currencyApiService;

        public NewCurrencyWindow()
        {
            InitializeComponent();

            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var config = builder.Build();
            AppSettings settings = new AppSettings();
            config.Bind("AppSettings", settings);

            IOptions<AppSettings> options = Options.Create(settings);

            _currencyRepository = new CurrencyRepository(options);
            _currencyApiService = new CurrencyApiService(options);

            cmbCurrencies.ItemsSource = _currencyApiService.GetAvailableCurrencies();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var currency = cmbCurrencies.SelectedItem as Currency;

            _currencyRepository.AddCurrency(currency);

            Close();
        }
    }
}
