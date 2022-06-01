namespace CurrencyExchange.Repositories
{
    public interface ICurrencyRepository<T>
    {
        Task<IEnumerable<T>> GetRatesEuroAsync();
        Task<IEnumerable<T>> GetHistoric(string dateTime);
    }
}
