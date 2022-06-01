namespace CurrencyExchange.Models
{
    public class Currency
    {
        public string Id { get; set; }
        public double Rate { get; set; }

        public Currency(string id, double rate)
        {
            Id = id;
            Rate = rate;
        }
    }
}
