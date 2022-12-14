using ATM_Test.Helpers;
using ATM_Test.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace ATM_Test.Controllers
{
    [ApiController]
    [Route("api")]
    [Produces("application/json")]
    public class WithdrawalController : ControllerBase
    {
        private readonly ILogger<WithdrawalController> _logger;

        private readonly IDepositService _depositService;

        private readonly IWithdrawService _withdrawService;

        public WithdrawalController(ILogger<WithdrawalController> logger, IDepositService depositService, IWithdrawService withdrawService)
        {
            _logger = logger;
            _depositService = depositService;
            _withdrawService = withdrawService;
        }

        private static bool IsValid(ulong sum)
        {
            if ( (sum % 1000) == 0)
            {
                return true;
            }

            return false;
        }

        [HttpPost("withdrawal")]
        [ProducesResponseType(typeof(Dictionary<string, uint>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status503ServiceUnavailable)]
        public ActionResult Withdrawal(ulong sum)
        {
            _logger.LogInformation("Withdrawal request - " + sum);

            bool valid = IsValid(sum);
            if (!valid)
            {
                _logger.LogInformation("Withdrawal response: " + nameof(StatusCodes.Status400BadRequest));
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            ulong total = _depositService.CalculateTotal();
            if (sum > total)
            {
                _logger.LogInformation("Withdrawal response: " + nameof(StatusCodes.Status503ServiceUnavailable));
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }

            var withdrawSolution = _withdrawService.Withdraw(sum);
            if (withdrawSolution.Count == 0)
            {
                _logger.LogInformation("Withdrawal response: " + nameof(StatusCodes.Status503ServiceUnavailable));
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }

            _logger.LogInformation("Withdrawal response: " + Helper.DictionaryToLogString(withdrawSolution));
            return Ok(withdrawSolution);
        }
    }
}
