using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.IO;

namespace FileNameSerializerTest
{
    using FileNameSerializer;

    [TestClass]
    public class FileNameSerializerTests
    {
        private readonly static string DirWithASubDirMp4Files = Directory.GetCurrentDirectory() + "\\DirWithASubDirMp4Files";
        private readonly static string DirWithTwoSubDirsMp4Files = Directory.GetCurrentDirectory() + "\\DirWithTwoSubDirsMp4Files";

        private const string FIRST_FILE_TIME = "4/23/2015 6:26:11.209 PM";
        private const string SECOND_FILE_TIME = "4/23/2015 6:27:45.837 PM";
        private const string THIRD_FILE_TIME = "4/23/2015 6:28:32.526 PM";
        private readonly string[] _serializedFileName = new [] { "video-1.mp4", "video-2.mp4", "video-3.mp4" };

        private readonly DateTime[] _fileDateTimeStamps = new [] {
                    DateTime.Parse(FIRST_FILE_TIME),
                    DateTime.Parse(SECOND_FILE_TIME),
                    DateTime.Parse(THIRD_FILE_TIME) };

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            EnvironmentWorker.GetFileExtension("FileExtension");
            EnvironmentWorker.GetFileNameTemplate("FileNameTemplate");
        }

        [TestMethod]
        public void ShouldMatchTimeStampsFromASubDirContaingMp4Files()
        {
            EnvironmentWorker.GetAllSubDirectories(DirWithASubDirMp4Files);
            EnvironmentWorker.EnqueueDirectories();

            string directory;
            EnvironmentWorker.TargetDirectories.TryPeek(out directory);

            var fns = new FileNameSerializer();
            fns.ChangeFileName();

            AssertIt(directory);
        }

        [TestMethod]
        public void ShouldMatchTimeStampsFromTwoSubDirsContaingMp4Files()
        {
            EnvironmentWorker.GetAllSubDirectories(DirWithTwoSubDirsMp4Files);
            EnvironmentWorker.EnqueueDirectories();

            var directories = new string[2];
            var i = 0;
            foreach(var td in EnvironmentWorker.TargetDirectories)
            {
                directories[i++] = td;
            }

            new FileNameSerializer().ChangeFileName();

            foreach (var directory in directories)
            {
                AssertIt(directory);
            }
        }

        private void AssertIt(string directory)
        {
            var convertedFiles = Directory.GetFiles(directory);
            var loop = 0;
            foreach (var fileInfo in convertedFiles.OrderBy(f => (new FileInfo(f)).CreationTimeUtc).Select(f => new FileInfo(f)))
            {
                Assert.AreEqual(fileInfo.CreationTimeUtc, _fileDateTimeStamps[loop]);
                Assert.IsTrue(fileInfo.FullName.Contains(_serializedFileName[loop++]));
            }
        }
    }
}
