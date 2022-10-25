using ATM_Test.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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

        public static string DictionaryToLogString(Dictionary<uint, ulong> dictionary)
        {
            var lines = dictionary.Select(kvp => kvp.Key + ": " + kvp.Value.ToString());
            string dictionaryLog = string.Join(Environment.NewLine, lines);
            return dictionaryLog;
        }

        public static string DictionaryToLogString(Dictionary<string, uint> dictionary)
        {
            var lines = dictionary.Select(kvp => kvp.Key + ": " + kvp.Value.ToString());
            string dictionaryLog = string.Join(Environment.NewLine, lines);
            return dictionaryLog;
        }
    }
}
