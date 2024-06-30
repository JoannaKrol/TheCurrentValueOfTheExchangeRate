namespace TheCurrentValueOfTheExchangeRate
{
    public class ExchangeValue
    {
        public string Currency { get; set; }
        public string Code { get; set; }

        public ExchangeValue(string currency, string code)
        {
            Currency = currency;
            Code = code;
        }

        public override string ToString()
        {
            return Code;
        }
    }
}
