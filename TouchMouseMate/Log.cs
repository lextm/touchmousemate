using log4net;

namespace Lextm.TouchMouseMate
{
    public static class Log
    {
        private static ILog Logger
        {
            get { return LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); }
        }

        public static void Debug(object message)
        {
            if (!Logger.IsDebugEnabled)
            {
                return;
            }

            Logger.Debug(message);
        }

        public static void DebugFormat(string pattern, params object[] args)
        {
            if (!Logger.IsDebugEnabled)
            {
                return;
            }

            Logger.DebugFormat(pattern, args);
        }

        public static void Info(object message)
        {
            Logger.Info(message);
        }

        public static void InfoFormat(string pattern, params object[] args)
        {
            Logger.InfoFormat(pattern, args);
        }
    }
}
