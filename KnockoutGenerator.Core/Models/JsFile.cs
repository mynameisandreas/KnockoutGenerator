using System.Collections.Generic;
using System.IO;

namespace KnockoutGenerator.Core.Models
{
    public class JsFile
    {
        public string FullPath { get; set; }
        public string FileName
        {
            get { return Path.GetFileName(FullPath); }
        }

        public string FileNameWithOutExtension
        {
            get { return Path.GetFileNameWithoutExtension(FullPath); }
        }

        public List<JsClass> Files { get; set; }
    }
}
