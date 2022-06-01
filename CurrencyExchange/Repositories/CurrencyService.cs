using CurrencyExchange.Models;

namespace CurrencyExchange.Repositories
{
    public class CurrencyService : ICurrencyService<Currency>
    {
        private List<Currency> currencies = new();
        private readonly ICurrencyRepository<Currency> repository;

        public CurrencyService(ICurrencyRepository<Currency> repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<Currency>> GetRatesEuroAsync()
        {
            return await repository.GetRatesEuroAsync();
        }

        public async Task<IEnumerable<Currency>>? GetRatesByIdAsync(string id)
        {
            currencies = (List<Currency>)await repository.GetRatesEuroAsync();

            if (currencies is null || !currencies.Exists(x => x.Id == id.ToUpper()))
            {
                return null;
            }

            ChangeBaseRate(id.ToUpper());

            return currencies;
        }

        public IEnumerable<Currency>? ChangeBaseRate(string id)
        {
            int newBaseIndex = currencies.FindIndex(0, currencies.Count - 1, x => x.Id == id);

            if (newBaseIndex == -1)
            {
                return null;
            }

            double newBase = currencies[newBaseIndex].Rate;

            foreach (Currency currency in currencies)
            {
                currency.Rate /= newBase;
            }

            return currencies;
        }

        public async Task<CurrencyResponseDTO>? Convert(CurrencyRequestDTO currencyRequestDTO)
        {
            if (currencyRequestDTO is not null)
            {
                var currencies = await repository.GetRatesEuroAsync();

                double toRate = 0;
                double fromRate = 0;
                int found = 0;

                foreach (var currency in currencies)
                {
                    if (currency.Id == currencyRequestDTO.ToCurrency.ToUpper())
                    {
                        toRate = currency.Rate;
                        found++;
                    }
                    if (currency.Id == currencyRequestDTO.FromCurrency.ToUpper())
                    {
                        fromRate = currency.Rate;
                        found++;
                    }
                }

                if (found > 1)
                {
                    return new CurrencyResponseDTO(currencyRequestDTO.Value, currencyRequestDTO.FromCurrency, currencyRequestDTO.ToCurrency, currencyRequestDTO.Value * toRate / fromRate);
                }
            }

            return null;
        }

        public async Task<IEnumerable<Currency>> GetHistoric(int days)
        {
            if (days < 0)
            {
                days *= -1;
            }

            DateTime dateTime = DateTime.Now.AddDays(-days);

            return await repository.GetHistoric(dateTime.ToShortDateString());
        }
    }
}
