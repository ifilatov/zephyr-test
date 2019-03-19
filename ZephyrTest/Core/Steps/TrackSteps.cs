using System;
using ZephyrTest.Core.AppObjects;
using ZephyrTest.Models;
using ZephyrTest.Tools;

namespace ZephyrTest.Core.Steps
{
    class TrackSteps
    {
        public static string TrackPackage(string data)
        {
            //Event Data File: 0174
            //1234qwerty
            string parsedData = "";
            if(data.Contains("Event Data File"))
            {
                var EventDataList = FileReader.GetEventData();
                var currentEventCode = data.Split(':')[1].Trim();
                var currentDataRecord = EventDataList.Find(x => x.EventCode == currentEventCode);
                parsedData = currentDataRecord.Barcode;
            }
            else
            {
                parsedData = data;
            }

            //DataRecord result = list.Find(x => x.EventCode == eventCode);
            try
            {
                return PackageTracker.TrackPackage(parsedData);
            }
            catch (Exception e)
            { 
           
                return e.Message;
            }
        }
    }
}
