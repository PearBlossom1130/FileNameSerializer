using CommandLine;
using CommandLine.Text;

namespace FileNameSerializer.Common
{
    public class CommandLineOption
    {
        [Option('d', "directory", Required = true)]
        public string TargetFolder { get; set; }

        [Option('e', "extension", HelpText="Please specify a file extension. Ex) -e mp4")]
        public string FileExtension { get; set; }

        [Option('f', "filename", HelpText="Please specify a file name to use as template.\n\tEx) -f video")]
        public string FileName { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this);
        }
    }
}
