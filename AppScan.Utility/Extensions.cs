namespace AppScan.Utility
{
    public static class Extensions
    {
        public static string SanitizedForCSV(this string input, string csvDelimiter)
        {
            if (!string.IsNullOrEmpty(input))
            {
                return input.Replace(csvDelimiter, string.Empty).Trim();
            }
            return input;
        }
    }
}
