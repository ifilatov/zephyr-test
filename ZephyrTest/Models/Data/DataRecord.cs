using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZephyrTest.Models
{
    class DataRecord
    {
        public string EventCode { get; set; }
        public string EventDescription { get; set; }
        public string Barcode { get; set; }
        public string Dnc { get; set; }

        public DataRecord(string eventCode, string eventDescription, string barcode, string dnc)
        {
            EventCode = eventCode;
            EventDescription = eventDescription;
            Barcode = barcode;
            Dnc = dnc;
        }
    }
}
