using CurrencyConverter.Data;
using CurrencyConverter.View;
using CurrencyConverter.ViewModel;
using CurrencyConverter.ViewModel.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SQLite;
using System;
using System.IO;
using System.Windows;

namespace CurrencyConverter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public IConfiguration Configuration { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            InitializeDatabase();

            var mainWindow = ServiceProvider.GetRequiredService<ConverterWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services) 
        {
            services.Configure<AppSettings>(Configuration.GetSection(nameof(AppSettings)));

            services.AddScoped<ICurrencyApiService, CurrencyApiService>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();

            services.AddScoped<ConverterWindow>();
            services.AddScoped<NewCurrencyWindow>();
        }

        private void InitializeDatabase() 
        {
            var currencyRepository = ServiceProvider.GetRequiredService<ICurrencyRepository>();

            //Creates table if it doesn't exists.
            currencyRepository.CreateTable();
        }
    }
}
