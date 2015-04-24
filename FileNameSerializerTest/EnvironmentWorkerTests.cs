using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using FileNameSerializer;

namespace FileNameSerializerTest
{
    using System;
    using System.Diagnostics;

    [TestClass]
    public class EnvironmentWorkerTests
    {
        private readonly static string DirWithNoSubDir = Directory.GetCurrentDirectory() + "\\dirWithNoSubDir";
        private readonly static string DirWithASubDirRoot = Directory.GetCurrentDirectory() + "\\dirWithASubDir";
        private const string SUB_DIR = "\\ASubDir";
        private readonly static string DirWithTwoSiblingSubDirRoot = Directory.GetCurrentDirectory() + "\\dirWithTwoSiblingSubDirRoot";
        private const string SIBLING_SUB_DIR1 = "\\ASiblingDir1";
        private const string SIBLING_SUB_DIR2 = "\\ASiblingDir2";

        private readonly static string DirWithASubDirMp4Files = Directory.GetCurrentDirectory() + "\\dirWithASubDirMp4Files";
        private readonly static string DirWithTwoSubDirsMp4Files = Directory.GetCurrentDirectory() + "\\dirWithTwoSubDirsMp4Files";
        private readonly static string DirWithTwoSubDirsMp4FilesSub1 = DirWithTwoSubDirsMp4Files + "\\dirWithTwoSubDirsMp4FilesSub1";
        private readonly static string DirWithTwoSubDirsMp4FilesSub2 = DirWithTwoSubDirsMp4Files + "\\dirWithTwoSubDirsMp4FilesSub2";

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            var parentDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent;
            if (parentDir != null)
            {
                var testFilePath = parentDir.FullName+"\\TestFiles\\";
                ModifyCreationTime();

                Directory.CreateDirectory(DirWithASubDirMp4Files);
                Directory.CreateDirectory(DirWithTwoSubDirsMp4Files);
                Directory.CreateDirectory(DirWithTwoSubDirsMp4FilesSub1);
                Directory.CreateDirectory(DirWithTwoSubDirsMp4FilesSub2);

                var sourceFile1 = string.Format("{0}\\dummy100.mp4", testFilePath);
                var sourceFile2 = string.Format("{0}\\sample1.mp4", testFilePath);
                var sourceFile3 = string.Format("{0}\\test-89.mp4", testFilePath);

                var destinationFile1 = string.Format("{0}\\dummy100.mp4", DirWithASubDirMp4Files);
                var destinationFile2 = string.Format("{0}\\sample1.mp4", DirWithASubDirMp4Files);
                var destinationFile3 = string.Format("{0}\\test-89.mp4", DirWithASubDirMp4Files);


                var destinationFileTwoSub11 = string.Format("{0}\\dummy100.mp4", DirWithTwoSubDirsMp4FilesSub1);
                var destinationFileTwoSub12 = string.Format("{0}\\sample1.mp4", DirWithTwoSubDirsMp4FilesSub1);
                var destinationFileTwoSub13 = string.Format("{0}\\test-89.mp4", DirWithTwoSubDirsMp4FilesSub1);

                var destinationFileTwoSub21 = string.Format("{0}\\dummy100.mp4", DirWithTwoSubDirsMp4FilesSub2);
                var destinationFileTwoSub22 = string.Format("{0}\\sample1.mp4", DirWithTwoSubDirsMp4FilesSub2);
                var destinationFileTwoSub23 = string.Format("{0}\\test-89.mp4", DirWithTwoSubDirsMp4FilesSub2);

                CopyFileWithTimeStampKept(sourceFile1, destinationFile1);
                CopyFileWithTimeStampKept(sourceFile2, destinationFile2);
                CopyFileWithTimeStampKept(sourceFile3, destinationFile3);

                CopyFileWithTimeStampKept(sourceFile1, destinationFileTwoSub11);
                CopyFileWithTimeStampKept(sourceFile2, destinationFileTwoSub12);
                CopyFileWithTimeStampKept(sourceFile3, destinationFileTwoSub13);
                CopyFileWithTimeStampKept(sourceFile1, destinationFileTwoSub21);
                CopyFileWithTimeStampKept(sourceFile2, destinationFileTwoSub22);
                CopyFileWithTimeStampKept(sourceFile3, destinationFileTwoSub23);
            }
        }

        private static void ModifyCreationTime()
        {
            var parentDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent;
            if (parentDir != null)
            {
                var testFilePath = parentDir.FullName + "\\TestFiles\\";
                
                var firstFileTime = "4/23/2015 6:26:11.209 PM";
                var secondFileTime = "4/23/2015 6:27:45.837 PM";
                var thirdFileTime = "4/23/2015 6:28:32.526 PM";

                File.SetCreationTimeUtc(testFilePath+"dummy100.mp4", DateTime.Parse(firstFileTime));
                File.SetCreationTimeUtc(testFilePath+"sample1.mp4", DateTime.Parse(secondFileTime));
                File.SetCreationTimeUtc(testFilePath+"test-89.mp4", DateTime.Parse(thirdFileTime));
            }
        }

        private static void CopyFileWithTimeStampKept(string sourcePath, string destPath)
        {
            var sourceFileInfo = new FileInfo(sourcePath);

            sourceFileInfo.CopyTo(destPath, true);
            var destFileInfo = new FileInfo(destPath)
                {
                    CreationTimeUtc = sourceFileInfo.CreationTimeUtc,
                    LastWriteTimeUtc = sourceFileInfo.LastWriteTimeUtc,
                    LastAccessTimeUtc = sourceFileInfo.LastAccessTimeUtc
                };
        }

        [AssemblyCleanup()]
        public static void AssemblyCleanup()
        {
            Directory.Delete(DirWithASubDirMp4Files, true);
            Directory.Delete(DirWithTwoSubDirsMp4Files, true);
        }

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
           EnvironmentWorker.GetFileExtension("FileExtension");
           EnvironmentWorker.GetFileNameTemplate("FileNameTemplate");

           Directory.CreateDirectory(DirWithNoSubDir);
           Directory.CreateDirectory(DirWithASubDirRoot+SUB_DIR);
           Directory.CreateDirectory(DirWithTwoSiblingSubDirRoot + SIBLING_SUB_DIR1);
           Directory.CreateDirectory(DirWithTwoSiblingSubDirRoot + SIBLING_SUB_DIR2);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Directory.Delete(DirWithNoSubDir);
            Directory.Delete(DirWithASubDirRoot + SUB_DIR);
            Directory.Delete(DirWithASubDirRoot);
            Directory.Delete(DirWithTwoSiblingSubDirRoot + SIBLING_SUB_DIR1);
            Directory.Delete(DirWithTwoSiblingSubDirRoot + SIBLING_SUB_DIR2);
            Directory.Delete(DirWithTwoSiblingSubDirRoot);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            string item = null;
            while (EnvironmentWorker.TargetDirectories.TryDequeue(out item)) { }
        }
        
        [TestMethod]
        public void ShouldReturnZeroSubDirectory()
        {
            var subDirs = EnvironmentWorker.GetAllSubDirectories(DirWithNoSubDir);

            Assert.AreEqual(0, subDirs.Count);
        }

        [TestMethod]
        public void ShouldReturnASubDirectory()
        {
            var subDir = EnvironmentWorker.GetAllSubDirectories(DirWithASubDirRoot);

            Assert.AreEqual(1, subDir.Count);
            Assert.AreEqual(DirWithASubDirRoot+SUB_DIR, subDir[0]);
        }

        [TestMethod]
        public void ShouldReturnTwoSiblingSubDirectories()
        {
            var subDirs = EnvironmentWorker.GetAllSubDirectories(DirWithTwoSiblingSubDirRoot);

            Assert.AreEqual(2, subDirs.Count);
            Assert.AreEqual(DirWithTwoSiblingSubDirRoot + SIBLING_SUB_DIR1, subDirs[0]);
            Assert.AreEqual(DirWithTwoSiblingSubDirRoot + SIBLING_SUB_DIR2, subDirs[1]);
        }

        [TestMethod]
        public void ShouldReturnNullWhenNonExistDir()
        {
            var subDirs = EnvironmentWorker.GetAllSubDirectories("ThisDirectoryNameDoesNotExist");

            Assert.AreEqual(null, subDirs);
        }

        [TestMethod]
        public void ShouldSetFileExtensionToMp4()
        {
            Assert.AreEqual("mp4", EnvironmentWorker.FileExtension);
            Assert.AreEqual("*.mp4", EnvironmentWorker.FormattedExtension);
        }

        [TestMethod]
        public void ShouldSetFilenameToVideo()
        {
            Assert.AreEqual("video", EnvironmentWorker.FileNameTemplate);
        }

        [TestMethod]
        public void ShouldReturnTwoDirectoryContainingMp4FilesFromSubDirs()
        {
            EnvironmentWorker.GetAllSubDirectories(DirWithTwoSubDirsMp4Files);
            EnvironmentWorker.EnqueueDirectories();
            foreach (var d in EnvironmentWorker.TargetDirectories) Debug.WriteLine(d);

            Assert.AreEqual(2, EnvironmentWorker.TargetDirectories.Count);
        }

        [TestMethod]
        public void ShouldReturnOneDirectoryContainingMp4FilesFromRootDir()
        {
            EnvironmentWorker.GetAllSubDirectories(DirWithASubDirMp4Files);
            EnvironmentWorker.EnqueueDirectories();

            Assert.AreEqual(1, EnvironmentWorker.TargetDirectories.Count);
        }
    }
}
