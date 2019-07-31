using System;
using System.Linq.Expressions;
using System.Reflection;
using log4net;
using PlaySharp.Toolkit.Logging;

namespace AdvancedMiniMap.Utilities
{
    public static class ConsoleUtility
    {
        private static readonly ILog Log = AssemblyLogs.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static Config _config;
        private static string _prefix = "";

        public static void SetPrefix(string prefix)
        {
            _prefix = prefix;
        }

        public static void SetConfig(Config config)
        {
            _config = config;
        }

        public static void DebugWriteLine(string message)
        {
            if (_config.EnableDebug)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(_prefix + message);
                Log.Debug(_prefix + message);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public static void WarningWriteLine(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(_prefix + message);
            Log.Warn(_prefix + message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void ErrorWriteLine(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(_prefix + message);
            Log.Error(_prefix + message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void InfoWriteLine(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(_prefix + message);
            Log.Info(_prefix + message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static string GetMemberName<T>(Expression<Func<T>> memberExpression)
        {
            MemberExpression expressionBody = (MemberExpression)memberExpression.Body;
            return expressionBody.Member.Name;
        }
    }
}
