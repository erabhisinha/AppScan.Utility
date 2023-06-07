namespace AppScan.Utility.Models
{
    public class ExclusionInfo
    {
        public string FilePath { get; set; }
        public int LineNo { get; set; } = 0;
        public string Phrase { get; set; }
    }
}
