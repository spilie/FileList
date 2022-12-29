using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileList.Model
{
    public class FolderInfo
    {
        public string Name { get; set; }

        public string FullName { get; set; }

        public List<DocInfo> Docs { get; set; }
    }
}
