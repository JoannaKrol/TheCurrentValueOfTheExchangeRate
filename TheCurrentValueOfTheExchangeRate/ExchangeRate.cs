namespace TheCurrentValueOfTheExchangeRate
{
    class ExchangeRate
    {
        public string DisplayValue { get; set; }
        public int HiddenValue { get; set; }

        public ExchangeRate(string displayValue, int hiddenValue)
        {
            DisplayValue = displayValue;
            HiddenValue = hiddenValue;
        }

        public override string ToString()
        {
            return DisplayValue;
        }
    }
}
