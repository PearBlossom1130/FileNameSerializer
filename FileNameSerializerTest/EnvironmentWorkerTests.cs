using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using FileNameSerializer;

namespace FileNameSerializerTest
{
    [TestClass]
    public class EnvironmentWorkerTests
    {
        private readonly static string DirWithNoSubDir = Directory.GetCurrentDirectory() + "\\dirWithNoSubDir";
        private readonly static string DirWithASubDirRoot = Directory.GetCurrentDirectory() + "\\dirWithASubDir";
        private const string aSubDir = "\\ASubDir";
        private readonly static string DirWithTwoSiblingSubDirRoot = Directory.GetCurrentDirectory() + "\\dirWithTwoSiblingSubDirRoot";
        private const string aSiblingSubDir1 = "\\ASiblingDir1";
        private const string aSiblingSubDir2 = "\\ASiblingDir2";

        private readonly static string DirWithASubDirMp4Files = Directory.GetCurrentDirectory() + "\\dirWithASubDirMp4Files";
        private readonly static string DirWithTwoSubDirsMp4FilesRoot = Directory.GetCurrentDirectory() + "\\dirWithTwoSubDirsMp4FilesRoot";
        private readonly static string DirWithTwoSubDirsMp4FilesSub1 = DirWithTwoSubDirsMp4FilesRoot + "\\dirWithTwoSubDirsMp4FilesSub1";
        private readonly static string DirWithTwoSubDirsMp4FilesSub2 = DirWithTwoSubDirsMp4FilesRoot + "\\dirWithTwoSubDirsMp4FilesSub2";

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            var parentDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent;
            if (parentDir != null)
            {
                var testFilePath = parentDir.FullName+"\\TestFiles\\";
                Directory.CreateDirectory(DirWithASubDirMp4Files);
                Directory.CreateDirectory(DirWithTwoSubDirsMp4FilesRoot);
                Directory.CreateDirectory(DirWithTwoSubDirsMp4FilesSub1);
                Directory.CreateDirectory(DirWithTwoSubDirsMp4FilesSub2);

                var sourceFile1 = string.Format("{0}\\dummy100.mp4", testFilePath);
                var sourceFile2 = string.Format("{0}\\sample1.mp4", testFilePath);
                var sourceFile3 = string.Format("{0}\\test-89.mp4", testFilePath);

                var destinationFile1 = string.Format("{0}\\dummy100.mp4", DirWithASubDirMp4Files);
                var destinationFile2 = string.Format("{0}\\sample1.mp4", DirWithASubDirMp4Files);
                var destinationFile3 = string.Format("{0}\\test-89.mp4", DirWithASubDirMp4Files);


                var destinationFileTwoSub1_1 = string.Format("{0}\\dummy100.mp4", DirWithTwoSubDirsMp4FilesSub1);
                var destinationFileTwoSub1_2 = string.Format("{0}\\sample1.mp4", DirWithTwoSubDirsMp4FilesSub1);
                var destinationFileTwoSub1_3 = string.Format("{0}\\test-89.mp4", DirWithTwoSubDirsMp4FilesSub1);

                var destinationFileTwoSub2_1 = string.Format("{0}\\dummy100.mp4", DirWithTwoSubDirsMp4FilesSub2);
                var destinationFileTwoSub2_2 = string.Format("{0}\\sample1.mp4", DirWithTwoSubDirsMp4FilesSub2);
                var destinationFileTwoSub2_3 = string.Format("{0}\\test-89.mp4", DirWithTwoSubDirsMp4FilesSub2);

                //File.Copy(sourceFile1, destinationFile1);
                //File.Copy(sourceFile2, destinationFile2);
                //File.Copy(sourceFile3, destinationFile3);

                //File.Copy(sourceFile1, destinationFileTwoSub1_1);
                //File.Copy(sourceFile2, destinationFileTwoSub1_2);
                //File.Copy(sourceFile3, destinationFileTwoSub1_3);

                //File.Copy(sourceFile1, destinationFileTwoSub2_1);
                //File.Copy(sourceFile2, destinationFileTwoSub2_2);
                //File.Copy(sourceFile3, destinationFileTwoSub2_3);

                CopyFileWithTimeStampKept(sourceFile1, destinationFile1);
                CopyFileWithTimeStampKept(sourceFile2, destinationFile2);
                CopyFileWithTimeStampKept(sourceFile3, destinationFile3);

                CopyFileWithTimeStampKept(sourceFile1, destinationFileTwoSub1_1);
                CopyFileWithTimeStampKept(sourceFile2, destinationFileTwoSub1_2);
                CopyFileWithTimeStampKept(sourceFile3, destinationFileTwoSub1_3);
                CopyFileWithTimeStampKept(sourceFile1, destinationFileTwoSub2_1);
                CopyFileWithTimeStampKept(sourceFile2, destinationFileTwoSub2_2);
                CopyFileWithTimeStampKept(sourceFile3, destinationFileTwoSub2_3);

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
            Directory.Delete(DirWithTwoSubDirsMp4FilesRoot, true);
        }

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
           EnvironmentWorker.GetFileExtension("FileExtension");
           EnvironmentWorker.GetFileNameTemplate("FileNameTemplate");

           Directory.CreateDirectory(DirWithNoSubDir);
           Directory.CreateDirectory(DirWithASubDirRoot+aSubDir);
           Directory.CreateDirectory(DirWithTwoSiblingSubDirRoot + aSiblingSubDir1);
           Directory.CreateDirectory(DirWithTwoSiblingSubDirRoot + aSiblingSubDir2);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {

            Directory.Delete(DirWithNoSubDir);
            Directory.Delete(DirWithASubDirRoot + aSubDir);
            Directory.Delete(DirWithASubDirRoot);
            Directory.Delete(DirWithTwoSiblingSubDirRoot + aSiblingSubDir1);
            Directory.Delete(DirWithTwoSiblingSubDirRoot + aSiblingSubDir2);
            Directory.Delete(DirWithTwoSiblingSubDirRoot);
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
            Assert.AreEqual(DirWithASubDirRoot+aSubDir, subDir[0]);
        }

        [TestMethod]
        public void ShouldReturnTwoSiblingSubDirectories()
        {
            var subDirs = EnvironmentWorker.GetAllSubDirectories(DirWithTwoSiblingSubDirRoot);

            Assert.AreEqual(2, subDirs.Count);
            Assert.AreEqual(DirWithTwoSiblingSubDirRoot + aSiblingSubDir1, subDirs[0]);
            Assert.AreEqual(DirWithTwoSiblingSubDirRoot + aSiblingSubDir2, subDirs[1]);
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
        public void ShouldReturnOneDirectoryContainingMp4FilesFromRootDir()
        {
            EnvironmentWorker.GetAllSubDirectories(DirWithASubDirMp4Files);
            EnvironmentWorker.EnqueueDirectories();

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
