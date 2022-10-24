using ATM_Test.Models;
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
    public class DepositController : ControllerBase
    {
        private readonly ILogger<DepositController> _logger;

        public DepositController(ILogger<DepositController> logger)
        {
            _logger = logger;
        }

        private static bool IsValid(Dictionary<string, uint> dictionary)
        {
            var denomations = Denomation.GetAll<Denomation>();
            var denomationStrings = denomations.Select(d => d.Unit).ToList().Select(s => s.ToString());

            if (dictionary.Count == 0)
            {
                return false;
            }

            foreach (KeyValuePair<string, uint> denomAndQuantiy in dictionary)
            {
                if (!denomationStrings.Contains(denomAndQuantiy.Key))
                {
                    return false;
                }
                else if (denomAndQuantiy.Value == 0)
                {
                    return false;
                }
            }

            return true;
        }

        [HttpPost("deposit")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public ActionResult Deposit(Dictionary<string, uint> deposit)
        {
            bool valid = IsValid(deposit);
            if (!valid)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            return Ok(new DepositResponse());
        }

    }
}
