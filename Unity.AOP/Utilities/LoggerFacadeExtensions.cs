using Microsoft.Practices.Prism.Logging;

namespace Unity.AOP.Utilities
{
    public static class LoggerFacadeExtensions
    {
        public static void Info(this ILoggerFacade facade, string format, params object[] args)
        {
            if (facade != null)
                facade.Log(string.Format(format, args), Category.Info, Priority.None);
        }

        public static void Warning(this ILoggerFacade facade, string format, params object[] args)
        {
            if (facade != null)
                facade.Log(string.Format(format, args), Category.Warn, Priority.None);
        }

        public static void Debug(this ILoggerFacade facade, string format, params object[] args)
        {
            if (facade != null)
                facade.Log(string.Format(format, args), Category.Debug, Priority.None);
        }

        public static void Error(this ILoggerFacade facade, string format, params object[] args)
        {
            if (facade != null)
                facade.Log(string.Format(format, args), Category.Exception, Priority.None);
        }
    }
}
