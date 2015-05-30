using System.Collections.Generic;
using log4net;
using log4net.Config;

namespace FileNameSerializer.Common
{
    public class Logger
    {
        private static readonly Dictionary<string, ILog> Loggers = new Dictionary<string, ILog>();

        public static ILog GetLogger(string name)
        {
            if (!Loggers.ContainsKey(name))
            {
                Loggers[name] = LogManager.GetLogger(name);
                XmlConfigurator.Configure();
            }

            return Loggers[name];
        }
    }
}
