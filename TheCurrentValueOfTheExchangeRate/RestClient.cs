using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace TheCurrentValueOfTheExchangeRate
{
    class RestClient
    {
        private static readonly HttpClient client = new HttpClient();
        private const string baseExchangesUrl = "http://api.nbp.pl/api/exchangerates/tables/A/";
        private const string baseRateUrl = "http://api.nbp.pl/api/exchangerates/rates/A/";
        public async Task<JArray> GetExchangeRatesAsync()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync(baseExchangesUrl);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            JArray exchangeRates = JArray.Parse(responseBody);

            return exchangeRates;
        }
        public async Task<decimal> GetCurrencyRate(string currencyCode)
        {
            decimal rate = 1;
            HttpResponseMessage response = await client.GetAsync($"{baseRateUrl}{currencyCode}/");

            if (response.IsSuccessStatusCode)
            {
                // Odczytaj dane w formacie JSON
                string jsonData = await response.Content.ReadAsStringAsync();
                JObject data = JObject.Parse(jsonData);

                // Pobierz kurs waluty
                rate = (decimal)data["rates"][0]["mid"];
            }
            else
            {
                Console.WriteLine("Nie udało się pobrać danych.");
            }
            return rate;
        }
    }
}
