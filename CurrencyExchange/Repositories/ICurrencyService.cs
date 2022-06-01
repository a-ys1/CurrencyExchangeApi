using CurrencyExchange.Models;

namespace CurrencyExchange.Repositories
{
    public interface ICurrencyService<T>
    {
        Task<IEnumerable<T>> GetRatesEuroAsync();
        Task<IEnumerable<T>>? GetRatesByIdAsync(string id);
        IEnumerable<T>? ChangeBaseRate(string id);
        Task<IEnumerable<T>> GetHistoric(int days);
        Task<CurrencyResponseDTO>? Convert(CurrencyRequestDTO currencyRequestDTO);
    }
}
