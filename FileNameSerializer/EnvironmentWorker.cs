using System;
using System.Collections.Generic;
using System.IO;
using System.Configuration;
using System.Collections.Concurrent;

namespace FileNameSerializer
{
    using System.Linq;

    public static class EnvironmentWorker
    {
        private const string LOGGER_NAME = "EnvironmentWorker";
        private static string[] subDirectories;
        private readonly static string CurrentDirectory = Directory.GetCurrentDirectory();

        public static string FormattedExtension { get; private set; }
        public static string FileExtension { get; private set; }
        public static string FileNameTemplate { get; private set; }

        public static ConcurrentQueue<string> TargetDirectories = new ConcurrentQueue<string>();

        public static IList<string> GetAllSubDirectories(string rootDir)
        {
            Logger.GetLogger(LOGGER_NAME).Info("GetAllSubDirectories is called.");

            string fullPath = rootDir;
            if (!IsAbsolutePath(rootDir))
            {
                fullPath = CurrentDirectory + "\\" + rootDir;
            }

            if (Directory.Exists(fullPath) == false)
            {
                Logger.GetLogger(LOGGER_NAME).ErrorFormat("The input directory {0} does not exist.", fullPath);
                return null;
            }

            try
            {
                var targetFiles = Directory.GetFiles(fullPath, FormattedExtension, SearchOption.TopDirectoryOnly);
                if (targetFiles.Length != 0)
                {
                    TargetDirectories.Enqueue(fullPath);
                }

                subDirectories = Directory.GetDirectories(@fullPath, "*.*", SearchOption.AllDirectories);
                return subDirectories.ToList();
            }
            catch (Exception ex)
            {
                Logger.GetLogger(LOGGER_NAME).Error(ex.Message, ex);
                return null;
            }
        }

        public static void EnqueueDirectories()
        {
            Logger.GetLogger(LOGGER_NAME).Info("EnqueueDirectories is called.");
            if (subDirectories != null)
            {
                foreach (var dir in subDirectories)
                {
                    var targetFiles = Directory.GetFiles(dir, FormattedExtension, SearchOption.TopDirectoryOnly);
                    if (targetFiles.Length != 0)
                    {
                        TargetDirectories.Enqueue(dir);
                    }
                }
            }
        }

        public static void GetFileExtension(string keyName)
        {
            Logger.GetLogger(LOGGER_NAME).Info("GetFileExtension is called.");

            var appSettings = ConfigurationManager.AppSettings;
            FileExtension = appSettings[keyName];
            FormattedExtension = string.Format("*.{0}", appSettings[keyName]);
        }

        public static void GetFileNameTemplate(string keyName)
        {
            Logger.GetLogger(LOGGER_NAME).Info("GetFileNameTemplate is called.");
            var appSettings = ConfigurationManager.AppSettings;
            FileNameTemplate = appSettings[keyName];
        }

        private static bool IsAbsolutePath(string rootDir)
        {
            return rootDir.Contains(':');
        }
    }
}
