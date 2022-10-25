using ATM_Test.Helpers;
using ATM_Test.Models;
using ATM_Test.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATM_Test.Controllers
{
    [ApiController]
    [Route("api")]
    [Produces("application/json")]
    public class DepositController : ControllerBase
    {
        private readonly ILogger<DepositController> _logger;

        private readonly IDepositService _depositService;

        public DepositController(ILogger<DepositController> logger, IDepositService depositService)
        {
            _logger = logger;
            _depositService = depositService;
        }

        private static bool IsValid(Dictionary<string, uint> dictionary)
        {
            var denomations = Denomation.GetAll<Denomation>();
            var denomationStrings = denomations.Select(d => d.Unit).ToList().Select(s => s.ToString());

            if (dictionary.Count == 0)
            {
                return false;
            }

            foreach (KeyValuePair<string, uint> denomAndQuantity in dictionary)
            {
                if (!denomationStrings.Contains(denomAndQuantity.Key))
                {
                    return false;
                }
                else if (denomAndQuantity.Value == 0)
                {
                    return false;
                }
            }

            return true;
        }

        [HttpPost("deposit")]
        [ProducesResponseType(typeof(ulong), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public ActionResult Deposit(Dictionary<string, uint> depositData)
        {   
            _logger.LogInformation("Deposit request: " + Helper.DictionaryToLogString(depositData));

            bool valid = IsValid(depositData);
            if (!valid)
            {
                _logger.LogInformation("Deposit response: " + nameof(StatusCodes.Status400BadRequest));
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            try
            {
                _depositService.Deposit(depositData);
            }
            catch (OverflowException exception)
            {
                _logger.LogError(0, exception, "Error while add to Quantity", exception.Data);
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            
            ulong total = _depositService.CalculateTotal();

            _logger.LogInformation("Deposit response: " + total);

            return Ok(total);
        }
    }
}
