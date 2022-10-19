using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AppScan.Utility.Models
{
    public class FalsePositives : List<ExclusionInfo>
    {
        protected string csv = string.Empty;
        protected string separator = ",";

        public FalsePositives(string csv, string repoBasePath, string separator = "\",\"")
        {
            this.csv = csv;
            this.separator = separator;

            foreach (string line in Regex.Split(csv, Environment.NewLine).ToList().Where(s => !string.IsNullOrEmpty(s)))
            {
                string[] values = Regex.Split(line, separator);
                this.Add(new ExclusionInfo { FilePath = Path.Combine(repoBasePath, values[0]), LineNo = int.Parse(values[1]) });
            }
        }
    }
}
