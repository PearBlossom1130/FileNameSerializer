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
        private readonly static string DirWithTwoSubDirsMp4FilesRoot = Directory.GetCurrentDirectory() + "\\DirWithTwoSubDirsMp4FilesRoot";
        private readonly static string DirWithTwoSubDirsMp4FilesSub1 = DirWithTwoSubDirsMp4FilesRoot + "\\DirWithTwoSubDirsMp4FilesSub1";
        private readonly static string DirWithTwoSubDirsMp4FilesSub2 = DirWithTwoSubDirsMp4FilesRoot + "\\DirWithTwoSubDirsMp4FilesSub2";

        private const string FIRST_FILE_TIME = "4/23/2015 6:26:11.209 PM";
        private const string SECOND_FILE_TIME = "4/23/2015 6:27:45.837 PM";
        private const string THIRD_FILE_TIME = "4/23/2015 6:28:32.526 PM";

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
            
            string directory = null;
            EnvironmentWorker.TargetDirectories.TryPeek(out directory);

            var fns = new FileNameSerializer();

            fns.ChangeFileName();
        
            var convertedFiles = Directory.GetFiles(directory);
            var loop = 0;
            foreach (var fileInfo in convertedFiles.Select(f => new FileInfo(f)))
            {
                Assert.AreEqual(fileInfo.CreationTimeUtc, _fileDateTimeStamps[loop++]);
            }
        }

        [TestMethod]
        public void ShouldMatchTimeStampsFromTwoSubDirsContaingMp4Files()
        {
            EnvironmentWorker.GetAllSubDirectories(DirWithTwoSubDirsMp4FilesRoot);
            EnvironmentWorker.EnqueueDirectories();

            Assert.AreEqual(2, EnvironmentWorker.TargetDirectories.Count);
        }
    }
}
