using ATM_Test.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATM_Test.Controllers
{
    [ApiController]
    [Route("api")]
    [Produces("application/json")]
    public class WithdrawalController : ControllerBase
    {
        private readonly ILogger<WithdrawalController> _logger;

        private readonly IDepositService _depositService;

        public WithdrawalController(ILogger<WithdrawalController> logger, IDepositService depositService)
        {
            _logger = logger;
            _depositService = depositService;
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
        public ActionResult Withdrawal(ulong sum)
        {
            bool valid = IsValid(sum);
            if (!valid)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            return Ok(new Dictionary<string, uint>());
        }
    }
}
