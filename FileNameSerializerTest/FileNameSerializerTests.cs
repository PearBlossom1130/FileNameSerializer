using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileNameSerializerTest
{
    using System;

    using FileNameSerializer;
    using System.IO;

    [TestClass]
    public class FileNameSerializerTests
    {
        private readonly static string DirWithASubDirMp4Files = Directory.GetCurrentDirectory() + "\\DirWithASubDirMp4Files";
        private readonly static string DirWithTwoSubDirsMp4FilesRoot = Directory.GetCurrentDirectory() + "\\DirWithTwoSubDirsMp4FilesRoot";
        private readonly static string DirWithTwoSubDirsMp4FilesSub1 = DirWithTwoSubDirsMp4FilesRoot + "\\DirWithTwoSubDirsMp4FilesSub1";
        private readonly static string DirWithTwoSubDirsMp4FilesSub2 = DirWithTwoSubDirsMp4FilesRoot + "\\DirWithTwoSubDirsMp4FilesSub2";

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            EnvironmentWorker.GetFileExtension("FileExtension");
            EnvironmentWorker.GetFileNameTemplate("FileNameTemplate");
        }

        //[AssemblyInitialize]
        //public static void AssemblyInit(TestContext context)
        //{
        //    var parentDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent;
        //    if (parentDir != null)
        //    {
        //        var testFilePath = parentDir.FullName + "\\TestFiles\\";
        //        Directory.CreateDirectory(DirWithASubDirMp4Files);
        //        Directory.CreateDirectory(DirWithTwoSubDirsMp4FilesRoot);
        //        Directory.CreateDirectory(DirWithTwoSubDirsMp4FilesSub1);
        //        Directory.CreateDirectory(DirWithTwoSubDirsMp4FilesSub2);

        //        var sourceFile1 = string.Format("{0}\\dummy100.mp4", testFilePath);
        //        var sourceFile2 = string.Format("{0}\\sample1.mp4", testFilePath);
        //        var sourceFile3 = string.Format("{0}\\test-89.mp4", testFilePath);

        //        var destinationFile1 = string.Format("{0}\\dummy100.mp4", DirWithASubDirMp4Files);
        //        var destinationFile2 = string.Format("{0}\\sample1.mp4", DirWithASubDirMp4Files);
        //        var destinationFile3 = string.Format("{0}\\test-89.mp4", DirWithASubDirMp4Files);


        //        var destinationFileTwoSub1_1 = string.Format("{0}\\dummy100.mp4", DirWithTwoSubDirsMp4FilesSub1);
        //        var destinationFileTwoSub1_2 = string.Format("{0}\\sample1.mp4", DirWithTwoSubDirsMp4FilesSub1);
        //        var destinationFileTwoSub1_3 = string.Format("{0}\\test-89.mp4", DirWithTwoSubDirsMp4FilesSub1);

        //        var destinationFileTwoSub2_1 = string.Format("{0}\\dummy100.mp4", DirWithTwoSubDirsMp4FilesSub2);
        //        var destinationFileTwoSub2_2 = string.Format("{0}\\sample1.mp4", DirWithTwoSubDirsMp4FilesSub2);
        //        var destinationFileTwoSub2_3 = string.Format("{0}\\test-89.mp4", DirWithTwoSubDirsMp4FilesSub2);

        //        File.Copy(sourceFile1, destinationFile1);
        //        File.Copy(sourceFile2, destinationFile2);
        //        File.Copy(sourceFile3, destinationFile3);

        //        File.Copy(sourceFile1, destinationFileTwoSub1_1);
        //        File.Copy(sourceFile2, destinationFileTwoSub1_2);
        //        File.Copy(sourceFile3, destinationFileTwoSub1_3);

        //        File.Copy(sourceFile1, destinationFileTwoSub2_1);
        //        File.Copy(sourceFile2, destinationFileTwoSub2_2);
        //        File.Copy(sourceFile3, destinationFileTwoSub2_3);

        //    }
        //}

        //[AssemblyCleanup()]
        //public static void AssemblyCleanup()
        //{
        //    Directory.Delete(DirWithASubDirMp4Files, true);
        //    Directory.Delete(DirWithTwoSubDirsMp4FilesRoot, true);
        //}

        [TestMethod]
        public void ShouldReturnOneDirectoryContainingMp4FilesFromRootDir()
        {
            EnvironmentWorker.GetAllSubDirectories(DirWithASubDirMp4Files);
            EnvironmentWorker.EnqueueDirectories();
            
            string directory = null;
            EnvironmentWorker.TargetDirectories.TryPeek(out directory);

            var fns = new FileNameSerializer();

            fns.ChangeFileName();

            var firstFileTime = "4/23/2015 6:26:11.209 PM";
            var secondFileTime = "4/23/2015 6:27:45.837 PM";
            var thirdFileTime = "4/23/2015 6:28:32.526 PM";

            var firstDateTime = DateTime.Parse(firstFileTime);
            var secondDateTime = DateTime.Parse(secondFileTime);
            var thirdDateTime = DateTime.Parse(thirdFileTime);

            var convertedFiles = Directory.GetFiles(directory);
            foreach (var f in convertedFiles)
            {
                var fileInfo = new FileInfo(f);
            }
            Assert.AreEqual(1, EnvironmentWorker.TargetDirectories.Count);
        }

        [TestMethod]
        public void ShouldReturnTwoDirectoryContainingMp4FilesFromSubDirs()
        {
            EnvironmentWorker.GetAllSubDirectories(DirWithTwoSubDirsMp4FilesRoot);
            EnvironmentWorker.EnqueueDirectories();

            Assert.AreEqual(2, EnvironmentWorker.TargetDirectories.Count);
        }
    }
}
