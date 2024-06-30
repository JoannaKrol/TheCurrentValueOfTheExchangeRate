using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TheCurrentValueOfTheExchangeRate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<ExchangeValue> CurrencyList = new List<ExchangeValue>();

        public MainWindow()
        {
            GetExchangeValues();
            InitializeComponent();
        }

        private void Count_Click(object sender, RoutedEventArgs e)
        {
            if (CheckFields()) 
            {
                Counter(Decimal.Parse(FromValueTextBox.Text));
            }

        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+{,}[^0-9]2");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void Counter(decimal fromValue)
        {
            decimal resultValue = fromValue;
            RestClient restClient = new RestClient();

            if (startExchange.SelectedValue.ToString() != "PLN")
            {
                var rate = await restClient.GetCurrencyRate(startExchange.SelectedValue.ToString());
                resultValue = resultValue / rate;
            }

            if (endExchange.SelectedValue.ToString() != "PLN")
            {
                var rate = await restClient.GetCurrencyRate(endExchange.SelectedValue.ToString());
                resultValue = resultValue * rate;
            }
            ToValueTextBox.Text = Math.Round(resultValue, 2).ToString();
        }

        public async void GetExchangeValues()
        {
            RestClient client = new RestClient();

            try
            {
                JArray exchangeRates = await client.GetExchangeRatesAsync();


                // Przykładowe przetwarzanie danych
                foreach (var table in exchangeRates)
                {
                    var rates = table["rates"];
                    foreach (var rate in rates)
                    {
                        string currency = rate["currency"].ToString();
                        string code = rate["code"].ToString();

                        CurrencyList.Add(new ExchangeValue(currency, code));

                    }
                }
                CurrencyList.Add(new ExchangeValue("Polski Złoty", "PLN"));
                CurrencyList = CurrencyList.OrderBy(c => c.Code).ToList();
            }
            catch (HttpRequestException e)
            {
                System.Diagnostics.Debug.WriteLine($"Request error: {e.Message}");
            }

            List<ExchangeRate> items = new List<ExchangeRate>();

            int number = 0;
            foreach (ExchangeValue rate in CurrencyList)
            {
                number++;
                items.Add(new ExchangeRate(rate.Code, number));
            }


            startExchange.ItemsSource = items;
            endExchange.ItemsSource = items;
        }

        private bool CheckFields()
        {
            var resultOfFieldsCheck = true;

            if (string.IsNullOrEmpty(FromValueTextBox.Text)) 
            {
                resultOfFieldsCheck = false;
                MessageBox.Show("Start value of rate can not be empty");
            }

            if (!Decimal.TryParse(FromValueTextBox.Text, out var result))
            {
                resultOfFieldsCheck = false;
                MessageBox.Show($"Value {FromValueTextBox.Text} is incorrect");
            }

            var fromValueParts = FromValueTextBox.Text.Split([',', '.']);

            if(fromValueParts.Length == 2 && fromValueParts[1].Length > 2)
            {
                resultOfFieldsCheck = false;
                MessageBox.Show("The amount can have a maximum of two decimal digits");
            }

            if (startExchange.SelectedValue == null || startExchange.SelectedValue.ToString() == string.Empty)
            {
                resultOfFieldsCheck = false;
                MessageBox.Show("Start exchange can not be empty");
            }

            if (string.IsNullOrEmpty(endExchange.SelectedValue?.ToString()))
            {
                resultOfFieldsCheck = false;
                MessageBox.Show("End exchange can not be empty");
            }

            return resultOfFieldsCheck;
        }
    }
}