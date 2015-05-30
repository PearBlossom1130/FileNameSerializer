using System.Reflection;
using System.Resources;
using System;
using FileNameSerializer.Common;
using System.Diagnostics;

namespace FileNameSerializer
{
    class Program
    {
        private const string LOGGER_NAME = "Main";
        private static CommandLineOption _options = new CommandLineOption();
        private static Stopwatch _stopWatch = new Stopwatch();

        static void Main(string[] args)
        {
            _stopWatch.Start();
            AppDomain.CurrentDomain.UnhandledException += GlobalHandler;
            InitCommandLine(args);

            var rm = new ResourceManager("FileNameSerializer.Resource", Assembly.GetExecutingAssembly());
            Logger.GetLogger(LOGGER_NAME).Info(rm.GetString("Start"));

            CheckInputParams();

            var subDirectories = EnvironmentWorker.GetAllSubDirectories(_options.TargetFolder);
            
            if (subDirectories == null)
            {
                Logger.GetLogger(LOGGER_NAME).Info(rm.GetString("End"));
                return;
            }

            EnvironmentWorker.EnqueueDirectories();

            var fileNameSerializer = new FileNameSerializer();

                fileNameSerializer.ChangeFileName();

            _stopWatch.Stop();
            ShowElapsedTime();
            
            Logger.GetLogger(LOGGER_NAME).Info(rm.GetString("End"));
        }

        private static void CheckInputParams()
        {
            if (_options.FileExtension == null)
            {
                EnvironmentWorker.GetFileExtension("FileExtension");
            }
            else
            {
                EnvironmentWorker.FileExtension = _options.FileExtension;
                EnvironmentWorker.FormattedExtension = string.Format("*.{0}", _options.FileExtension);
            }

            if (_options.FileName == null)
            {
                EnvironmentWorker.GetFileNameTemplate("FileNameTemplate");
            }
            else
            {
                EnvironmentWorker.FileNameTemplate = _options.FileName;
            }
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

        private static void ShowElapsedTime()
        {
            var ts = _stopWatch.Elapsed;
            var elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            Console.WriteLine("RunTime : " + elapsedTime);
        }
    }
}
