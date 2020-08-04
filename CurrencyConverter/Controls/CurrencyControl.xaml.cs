using CurrencyConverter.Data;
using CurrencyConverter.Domain;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace CurrencyConverter.Controls
{
    /// <summary>
    /// Interaction logic for CurrencyControl.xaml
    /// </summary>
    public partial class CurrencyControl : UserControl
    {
        
        public CurrencyControl()
        {
            InitializeComponent();
        }

        public Currency Currency
        {
            get { return (Currency)GetValue(CurrencyProperty); }
            set { SetValue(CurrencyProperty, value); }
        }

        public static readonly DependencyProperty CurrencyProperty =
            DependencyProperty.Register("Currency", typeof(Currency), typeof(CurrencyControl), new PropertyMetadata(null, SetCurrency));

        private static void SetCurrency(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CurrencyControl control = d as CurrencyControl;

            if (control == null)
                return;

            Currency currency = (e.NewValue as Currency);
            string code = currency.Code;
            control.nameTextBlock.Text = currency.Name;
            control.codeTextBlock.Text = code.ToUpper();

            if (currency.Amount > 0) 
            {
                control.valueTextBlock.Text = TryGetSymbol(code.ToUpper()) + string.Format("{0:#,##0.00}", currency.Amount);
            }
            
            try
            {
                var uri = new Uri($"pack://application:,,,/CurrencyConverter;component/images/{code.ToLower()}.png");
                control.flagImage.Source = new BitmapImage(uri);
            }
            catch 
            {
                control.flagImage.Source = new BitmapImage();
            }
            
        }

        private static string TryGetSymbol(string ISOCurrencyCode)
        {
            var symbol = CultureInfo
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
                .Select(ri => ri.CurrencySymbol)
                .FirstOrDefault();
            return symbol;
        }

    }
}
