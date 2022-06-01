using CurrencyExchange.Models;
using CurrencyExchange.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyExchangeController : ControllerBase
    {

        private readonly ICurrencyService<Currency> repository;

        public CurrencyExchangeController(ICurrencyService<Currency> repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Gets a list of currencies using EUR as a base rate.
        /// </summary>
        /// <returns>A list of currencies available using EUR as a base rate</returns>
        /// <remarks>
        /// Sample request:
        ///     GET /currencies
        /// </remarks>
        /// <response code="200">Returns the list of currencies available 
        /// using EUR as a base rate</response>
        /// <response code="404">If no resources were found</response>
        /// 
        [HttpGet("~/api/currencies")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<Currency>>> GetRatesEuro()
        {
            var currencies = await repository.GetRatesEuroAsync();

            if (currencies is not null)
            {
                return Ok(currencies);
            }

            return NotFound();
        }

        /// <summary>
        /// Gets a list of currencies using baseCurrency as a base rate.
        /// </summary>
        /// <returns>A list of currencies available using 
        /// baseCurrency as a base rate</returns>
        /// <param name="baseCurrency"></param>
        /// <remarks>
        /// Sample request:
        ///     GET /currencies/GBP:
        /// </remarks>
        /// <response code="200">Returns the list of currencies available using baseCurrency as a base rate</response>
        /// <response code="404">If no resources were found</response>
        ///
        [HttpGet("~/api/currencies/{baseCurrency}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<Currency>>> GetRatesById(string baseCurrency)
        {
            var currencies = await repository.GetRatesByIdAsync(baseCurrency);

            if (currencies is not null)
            {
                return Ok(currencies);
            }

            return NotFound(baseCurrency);
        }

        /// <summary>
        /// Gets the conversion value of the currency.
        /// </summary>
        /// <returns>The conversion value of the currency.</returns>
        /// <param name="currencyRequestDTO"></param>
        /// <remarks>
        /// Sample request:
        ///
        ///       POST:
        ///     {
        ///         Value:10:00,
        ///         fromCurrency: GBP,
        ///         toCurrency: EUR
        ///     }
        /// </remarks>
        /// <response code="200">The conversion value of the currency</response>
        /// <response code="404">If no resources were found</response>
        ///
        [HttpPost("~/api/currencies/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<ActionResult<CurrencyResponseDTO>> Convert(CurrencyRequestDTO currencyRequestDTO)
        {
            if (await repository.Convert(currencyRequestDTO) is null)
            {
                return NotFound(currencyRequestDTO);
            }

            return Ok(await repository.Convert(currencyRequestDTO));
        }

        /// <summary>
        /// Gets the currency rates using EUR as a base rate in the past {days}. 
        /// Ex if {days} is 2, then get the currency rates of 2 days ago.
        /// </summary>
        /// <returns>The conversion value of the currency.</returns>
        /// <param name="days"></param>
        /// <remarks>
        /// Sample request:
        ///       GET/2
        /// </remarks>
        /// <response code="200">The rates from the past {days} using EUR as a base rate</response>
        /// <response code="404">If no resources were found</response>
        ///
        [HttpGet("~/api/currencies/historic/{days}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<Currency>>> GetHistoricRates(int days)
        {
            return Ok(await repository.GetHistoric(days));
        }
    }
}