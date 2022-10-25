using ATM_Test.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text;

namespace ATM_Test.Helpers
{
    public static class Helper
    {
        public static void LogModels(List<BankNote> models, ILogger logger)
        {
            StringBuilder logBuilder = new StringBuilder();
            logBuilder.AppendLine("------- Database ------");

            foreach (var model in models)
            {
                logBuilder.AppendLine(model.Value.ToString() + " - quantity:" + model.Quantity.ToString());
            }

            logBuilder.Append("------- Database ------");

            logger.LogInformation(logBuilder.ToString());
        }
    }
}
