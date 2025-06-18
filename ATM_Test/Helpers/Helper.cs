using ATM_Test.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text;

namespace ATM_Test.Helpers;

/// <summary>
/// Helper class for logging and converting dictionaries to strings.
/// </summary>
public static class Helper
{
    private const string DatabaseLogHeaderAndFooter = "------- Database ------";

    /// <summary>
    /// Logs the details of BankNote entities to the provided logger.
    /// </summary>
    /// <param name="models">BankNote entities</param>
    /// <param name="logger">Logger</param>
    public static void LogModels(List<BankNote> models, ILogger logger)
    {
        var logBuilder = new StringBuilder();
        logBuilder.AppendLine(DatabaseLogHeaderAndFooter);

        foreach (var model in models)
        {
            logBuilder.AppendLine($"{model.Value} - quantity: {model.Quantity}");
        }

        logBuilder.Append(DatabaseLogHeaderAndFooter);

        logger.LogInformation(logBuilder.ToString());
    }
}
