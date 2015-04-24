using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FileNameSerializer
{
    public class FileNameSerializer : IFileNameSerializer
    {
        private const string LOGGER_NAME = "FileNameSerializer";
        private readonly Action _action = null;
        private readonly int _numberOfProcessors = Environment.ProcessorCount;

        public FileNameSerializer()
        {
            _action = () =>
            {
                string targetDirectory;
                while (EnvironmentWorker.TargetDirectories.TryDequeue(out targetDirectory))
                {
                    var targetFiles = Directory.GetFiles(targetDirectory);
                    var dirTime = new Dictionary<string, DateTime>();

                    foreach (var f in targetFiles)
                    {
                        var creatTime = File.GetCreationTimeUtc(f);
                        dirTime.Add(f, creatTime);
                    }

                    var sortedDirTimeByTimeAsceding = from entry in dirTime orderby entry.Value ascending select entry;

                    var basePath = Path.GetDirectoryName(sortedDirTimeByTimeAsceding.First().Key);

                    var number = 1;
                    foreach (var i in sortedDirTimeByTimeAsceding)
                    {
                        var destinationFile = string.Format(basePath + "\\{0}-{1}.{2}", EnvironmentWorker.FileNameTemplate, number++, EnvironmentWorker.FileExtension);
                        File.Move(i.Key, destinationFile);
                    }
                }
            };
        }

        public void ChangeFileName()
        {
            var actions = new Action[_numberOfProcessors];

            for(var i = 0; i < _numberOfProcessors; i++)
            {
                actions[i] = _action;
            }

            Parallel.Invoke(actions);
        }
    }
}
