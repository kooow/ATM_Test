using ATM_Test.Helpers;
using ATM_Test.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace ATM_Test.Controllers;

/// <summary>
/// Withdrawal controller for handling ATM withdrawal requests.
/// </summary>
[ApiController]
[Route("api")]
[Produces("application/json")]
public class WithdrawalController : ControllerBase
{
    private readonly ILogger<WithdrawalController> m_logger;
    private readonly IDepositService m_depositService;
    private readonly IWithdrawService m_withdrawService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger">Logger</param>
    /// <param name="depositService">Deposit service</param>
    /// <param name="withdrawService">Withdraw service</param>
    public WithdrawalController(ILogger<WithdrawalController> logger, IDepositService depositService, IWithdrawService withdrawService)
    {
        m_logger = logger;
        m_depositService = depositService;
        m_withdrawService = withdrawService;
    }

    private static bool IsValidSummary(ulong sum)
    {
        if ((sum % 1000) == 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Withdraws a specified sum from the ATM.
    /// </summary>
    /// <param name="sum">Sum amount</param>
    /// <returns>Withdrawal response</returns>
    [HttpPost("withdrawal")]
    [ProducesResponseType(typeof(Dictionary<string, uint>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status503ServiceUnavailable)]
    public ActionResult Withdrawal(ulong sum)
    {
        m_logger.LogInformation($"Withdrawal request - {sum}");

        var valid = IsValidSummary(sum);
        if (!valid)
        {
            m_logger.LogInformation($"Withdrawal response: {nameof(StatusCodes.Status400BadRequest)}");
            return StatusCode(StatusCodes.Status400BadRequest);
        }

        var total = m_depositService.CalculateTotal();
        if (sum > total)
        {
            m_logger.LogInformation($"Withdrawal response: {nameof(StatusCodes.Status503ServiceUnavailable)}");
            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }

        var withdrawSolution = m_withdrawService.Withdraw(sum);
        if (withdrawSolution.Count == 0)
        {
            m_logger.LogInformation($"Withdrawal response: {nameof(StatusCodes.Status503ServiceUnavailable)}");
            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }

        m_logger.LogInformation($"Withdrawal response: {withdrawSolution.ToLogString()}");
        return Ok(withdrawSolution);
    }
}
