using ATM_Test.Helpers;
using ATM_Test.Models;
using ATM_Test.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATM_Test.Controllers;

/// <summary>
/// 
/// </summary>
[ApiController]
[Route("api")]
[Produces("application/json")]
public class DepositController : ControllerBase
{
    private readonly ILogger<DepositController> m_logger;
    private readonly IDepositService m_depositService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger">Logger</param>
    /// <param name="depositService">Deposit service</param>
    public DepositController(ILogger<DepositController> logger, IDepositService depositService)
    {
        m_logger = logger;
        m_depositService = depositService;
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

    /// <summary>
    /// Deposits specified banknotes into the ATM.
    /// </summary>
    /// <param name="depositData">Deposit data</param>
    /// <returns>Deposit result</returns>
    [HttpPost("deposit")]
    [ProducesResponseType(typeof(ulong), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public ActionResult Deposit(Dictionary<string, uint> depositData)
    {
        m_logger.LogInformation($"Deposit request: {depositData.ToLogString()}");

        var valid = IsValid(depositData);
        if (!valid)
        {
            m_logger.LogInformation($"Deposit response: {nameof(StatusCodes.Status400BadRequest)}");
            return StatusCode(StatusCodes.Status400BadRequest);
        }

        try
        {
            m_depositService.Deposit(depositData);
        }
        catch (OverflowException exception)
        {
            m_logger.LogError(0, exception, "Error while add to Quantity", exception.Data);
            return StatusCode(StatusCodes.Status400BadRequest);
        }

        var total = m_depositService.CalculateTotal();
        m_logger.LogInformation($"Deposit response: {total}");

        return Ok(total);
    }
}
