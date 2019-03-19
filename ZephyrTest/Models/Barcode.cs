using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZephyrTest.Models
{
    class BarcodeRecord
    {
        public string Type { get; set; }
        public string Barcode { get; set; }

        public BarcodeRecord(string type, string barcode)
        {
            Type = type;
            Barcode = barcode;
        }
    }
}
