using System;
using ZephyrTest.Core.AppObjects;
using ZephyrTest.Tools;

namespace ZephyrTest.Core.Steps
{
    class ScanSteps
    {
        public static string ScanBarcode(string data)
        {
            string parsedData = "";
            if (data.Contains("BarcodeList"))
            {
                var EventDataList = FileReader.GetBarcodeList();
                var currentType = data.Split(':')[1].Trim();
                var currentBarcode = EventDataList.Find(x => x.Type == currentType);
                parsedData = currentBarcode.Barcode;
            }
            try
            {
                return Scanner.ScanBarcode(parsedData);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}