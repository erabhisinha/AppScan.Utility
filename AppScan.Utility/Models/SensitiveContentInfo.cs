using System;

namespace AppScan.Utility.Models
{
    public class SensitiveContentInfo
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
