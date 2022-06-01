using System.ComponentModel.DataAnnotations;

namespace CurrencyExchange.Models
{
    public class CurrencyRequestDTO
    {
        [Required]
        public double Value { get; set; }
        [Required]
        public string FromCurrency { get; set; }
        [Required]
        public string ToCurrency { get; set; }

        public CurrencyRequestDTO(double value, string fromCurrency, string toCurrency)
        {
            Value = value;
            FromCurrency = fromCurrency;
            ToCurrency = toCurrency;
        }
    }
}