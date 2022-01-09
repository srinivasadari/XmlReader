using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlReader.FileWatcher.Models
{
   public class CsvFileData
    {
        public string Header { get; set; }

        public string FileName { get; set; }

        public List<string> BlockMeterReading { get; set; }
        public string Footer { get; set; }
    }
}
