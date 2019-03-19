using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using ZephyrTest.Models;

namespace ZephyrTest.Tools
{
    class FileReader
    {
        public static List<DataRecord> GetEventData()
        {
            string filepath = Directory.GetCurrentDirectory() + "\\ZephyrTest\\Data\\Event_Data.csv";
            var dataRecords = new List<DataRecord>();
            new List<string>(File.ReadAllLines(filepath)).ForEach(line =>
            {
                var dataRow = line.Split(',');
                dataRecords.Add(new DataRecord(dataRow[0], dataRow[1], dataRow[2], dataRow[3]));
            });

            return dataRecords;
        }
        public static List<BarcodeRecord> GetBarcodeList()
        {
            string filepath = Directory.GetCurrentDirectory() + "\\ZephyrTest\\Data\\BarcodeList.csv";
            var barcodeList = new List<BarcodeRecord>();
            new List<string>(File.ReadAllLines(filepath)).ForEach(line =>
            {
                var dataRow = line.Split(',');
                barcodeList.Add(new BarcodeRecord(dataRow[0], dataRow[1]));
            });

            return barcodeList;
        }
    }
}
