using System.Reflection;
using System.Resources;
using System;
using FileNameSerializer.Common;

namespace FileNameSerializer
{
    class Program
    {
        private const string LOGGER_NAME = "Main";
        private static CommandLineOption _options = new CommandLineOption();

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += GlobalHandler;
            InitCommandLine(args);

            var rm = new ResourceManager("FileNameSerializer.Resource", Assembly.GetExecutingAssembly());
            Logger.GetLogger(LOGGER_NAME).Info(rm.GetString("Start"));
            EnvironmentWorker.GetFileExtension("FileExtension");
            EnvironmentWorker.GetFileNameTemplate("FileNameTemplate");
            var subDirectories = EnvironmentWorker.GetAllSubDirectories(_options.TargetFolder);
            
            if (subDirectories == null)
            {
                Logger.GetLogger(LOGGER_NAME).Info(rm.GetString("End"));
                return;
            }
            else
            {
                EnvironmentWorker.EnqueueDirectories();

                var fileNameSerializer = new FileNameSerializer();

                fileNameSerializer.ChangeFileName();
            }

            Logger.GetLogger(LOGGER_NAME).Info(rm.GetString("End"));
        }

        private static void GlobalHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Console.WriteLine("Unhandled Exception hanppens.");
        }

        private static void InitCommandLine(string[] args)
        {
            CommandLine.Parser.Default.ParseArgumentsStrict(args, _options, OnFail);
        }

        private static void OnFail()
        {
            Environment.Exit(-1);
        }
    }
}
