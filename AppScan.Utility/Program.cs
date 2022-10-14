using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AppScan.Utility
{
    class Program
    {
        private static ILogger _logger = null;
        private static LoggerConfiguration _loggerConfiguration = null;
        static void Main(string[] args)
        {
            _loggerConfiguration = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console();
            InitializeLogger(args);
            if (args != null && args.Length == 3)
            {
                string regularExp = args[0];
                string inputDir = args[1];
                string reportPath = args[2];
                Scan(regularExp, inputDir, reportPath);
            }
            else
            {
                _logger.Warning("Parameters not found to scan a application");
            }
        }

        private static void InitializeLogger(string[] args)
        {
            if (args?.Count() > 1)
            {
                _loggerConfiguration = _loggerConfiguration.WriteTo.File($"{args[2]}/appscan.txt");
            }
            _logger = _loggerConfiguration.CreateLogger();
        }

        static void Scan(string expression, string folderPath, string outputPath)
        {
            //string expression1 = @"(pwd(\s)*(=){1}(\s)*(\""|\').*(\""|\'))|(password(\s)*(=){1}(\s)*(\""|\').*(\""|\'))";
            Regex pwdReg = new Regex(expression, RegexOptions.IgnoreCase);
            //string folderPath = @"D:\UCM\Staging30\Web";
            List<string> extensions = new List<string> { "*.config", "*.cs" };

            DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
            List<SensitiveContentInfo> sensitiveContents = new List<SensitiveContentInfo>();
            foreach (string pattern in extensions)
            {
                FileInfo[] filesToScan = directoryInfo.GetFiles(pattern, SearchOption.AllDirectories);
                foreach (FileInfo fileInfo in filesToScan)
                {
                    _logger.Information("Scanning File {@fileName}", fileInfo.Name);
                    string[] fileLines = File.ReadAllLines(fileInfo.FullName);
                    string fileText = File.ReadAllText(fileInfo.FullName);
                    bool isKeyWordIdentified = pwdReg.IsMatch(fileText);
                    if (isKeyWordIdentified)
                    {
                        MatchCollection allMatches = pwdReg.Matches(fileText);
                        _logger.Information("{@fileName} {@noOfOccurencesIdentified}", fileInfo.Name, allMatches.Count);
                        foreach (Match match in allMatches)
                        {
                            int lineNo = LineFromPos(fileText, match.Index);
                            if (lineNo > 0)
                            {
                                sensitiveContents.Add(new SensitiveContentInfo
                                {
                                    FileName = fileInfo.FullName,
                                    LineNumber = lineNo,
                                    LineContent = fileLines[lineNo - 1],
                                    KeywordIdentified = match.Value
                                });
                            }
                        }
                    }
                }
            }
            var reportBuilder = new StringBuilder();
            reportBuilder.AppendLine($"File Name\tLine Number\tLine Content\tKeyword Identified");
            _logger.Information("List of all keywords identified are:");
            foreach (var info in sensitiveContents)
            {
                reportBuilder.AppendLine(info.ToCSV());
                _logger.Information("{@object}", info);
            }
            File.WriteAllText($"{outputPath}\\Report-{DateTime.Now.ToString("hh_mm_ss_tt")}.csv", reportBuilder.ToString());
            Console.ReadLine();
        }

        public static int LineFromPos(string input, int indexPosition)
        {
            int lineNumber = 1;
            for (int i = 0; i < indexPosition; i++)
            {
                if (input[i] == '\n') lineNumber++;
            }
            return lineNumber;
        }

        class SensitiveContentInfo
        {
            public string FileName { get; set; }
            public string KeywordIdentified { get; set; }
            public int LineNumber { get; set; }
            public string LineContent { get; set; }

            public override string ToString()
            {
                return $"File Name: {FileName} {Environment.NewLine} Line No: {LineNumber} {Environment.NewLine} Line Content: {LineContent} {Environment.NewLine} Keyword Identified: {KeywordIdentified} {Environment.NewLine}";
            }

            public string ToCSV()
            {
                return $"{FileName}\t{LineNumber}\t{LineContent}\t{KeywordIdentified}";
            }
        }
    }
}
