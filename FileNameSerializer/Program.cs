using System.Reflection;
using System.Resources;

namespace FileNameSerializer
{
    class Program
    {
        private const string LOGGER_NAME = "Main";

        static void Main(string[] args)
        {
            var rm = new ResourceManager("FileNameSerializer.Resource", Assembly.GetExecutingAssembly());
            Logger.GetLogger(LOGGER_NAME).Info(rm.GetString("Start"));
            if (args.Length == 0)
            {
                EnvironmentWorker.ShowUsage();
                Logger.GetLogger(LOGGER_NAME).Info(rm.GetString("InputArgIsZero"));
                return;
            }

            EnvironmentWorker.GetFileExtension("FileExtension");
            EnvironmentWorker.GetFileNameTemplate("FileNameTemplate");
            var subDirectories = EnvironmentWorker.GetAllSubDirectories(args[0]);
            if (subDirectories == null)
                return;

            EnvironmentWorker.EnqueueDirectories();

            var fileNameSerializer = new FileNameSerializer();
            
            fileNameSerializer.ChangeFileName();

            Logger.GetLogger(LOGGER_NAME).Info(rm.GetString("End"));
        }
    }
}
