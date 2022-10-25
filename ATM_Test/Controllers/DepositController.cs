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
        public ActionResult Deposit(Dictionary<string, uint> deposit)
        {
            var lines = deposit.Select(kvp => kvp.Key + ": " + kvp.Value.ToString());
            string dictionaryLog = string.Join(Environment.NewLine, lines);
            _logger.LogInformation("Deposit endpoint - " + dictionaryLog);

            bool valid = IsValid(deposit);
            if (!valid)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            _depositService.Deposit(deposit);

            ulong total = _depositService.CalculateTotal();
            return Ok(total);
        }
    }
}
