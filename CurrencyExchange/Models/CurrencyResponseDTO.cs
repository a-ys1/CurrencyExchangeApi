namespace CurrencyExchange.Models
{
    public class CurrencyResponseDTO
    {
        public double BaseValue { get; set; }
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public double Result { get; set; }

        public CurrencyResponseDTO(double amount, string from, string to, double result)
        {
            BaseValue = amount;
            FromCurrency = from;
            ToCurrency = to;
            Result = result;
        }
    }
}
