using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using NUnit.Framework;
using ZephyrTest.Models.Data;

namespace ZephyrTest.Core.Steps
{

    class SAPSteps
    {

        private static readonly string[] Headers = "Group_Id,Site_Number,Designated_Bilingual_Site,Calendar,Company_Code,Purch_Organization,Sales_Org,Distr_Channel,Division,Annual_Sales,Sales_Currency,Sales_Year,Recon_Account,Sort_Key,Interest_Indic,Int_Calc_Freq,Payt_Terms,Cr_Memo_Terms,Acct_Statement,Item_Proposal,Order_Prob,Price_Group,Cust_Price_Proc,Delivery_Priority,Shipping_Conditions,Delivering_Site,Max_Part_Deliveries,Order_Combination,Delivery_Priority_Pod,Incoterms,Del_Payt_Terms,Site_Tax_Indicator,Valuation_Area,Grouping_Code,Inventory_Mgt_Profile,Revaluat_Profile,Subsequent_Listing_Ind,Listing_Procedures,Supplying_Site,Sup_Site_From,Sup_Site_To,Open_Mon_Am,Close_Mon_Am,Open_Mon_Pm,Close_Mon_Pm,Open_Tue_Am,Close_Tue_Am,Open_Tue_Pm,Close_Tue_Pm,Open_Wed_Am,Close_Wed_Am,Open_Wed_Pm,Close_Wed_Pm,Open_Thu_Am,Close_Thu_Am,Open_Thu_Pm,Close_Thu_Pm,Open_Fri_Am,Close_Fri_Am,Open_Fri_Pm,Close_Fri_Pm,Open_Sat_Am,Close_Sat_Am,Open_Sat_Pm,Close_Sat_Pm,Open_Sun_Am,Close_Sun_Am,Open_Sun_Pm,Close_Sun_Pm,Opening_Date,Closing_Date,Business_Name,Po_Name,Legal_Name,Other_Name,Search_Term1,Street_House_Number,City,Postal_Code,Country,Region,Transportation_Zone,Site_Default_Language,Telephone,Fax,Email,Contact_Person_Name,Department,Function,Contact_Language,Contact_Person_Phone,Contact_Person_Extension,Bank_Country,Bank_Key,Bank_Account,Auto_Debit_Auth,Prev_Acct_No,Payment_Methods,Record_Payment_History,Account_Memo,Pos_Inbound_Profile,Blocking_Reason,Block_From,Block_To,All_Sales_Areas,All_Sales_Areas_Check_Mark,Location_Code,Nielsen_Id,Customer_Class,Industry,Industry_Code1,Legal_Status,Sales_District,Sales_Office,Sales_Group,Customer_Group,Cust_Stats_Grp,Tax_Classification,Gst_Number,Confirmation_For_Licenses,License_Valid_From,License_Valid_To".ToUpper().Split(',');
        private static Dictionary<string, Dictionary<string, string>> Sites= new Dictionary<string, Dictionary<string, string>>();

        public static string ValidateFileImport(string data)
        {
            JsonConvert.DeserializeObject<List<FilePath>>(data).ForEach(file =>
            {
                ReadFileToDictionary(file.Filename);
                ImportFileToRiposte(file.Filename);
            });
            return ValitateSites();
        }

        private static void ImportFileToRiposte(string filename)
        {
            Process p = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = "cmd.exe",
                    Arguments = "/C c:\\WebRiposte\\Agents\\EGAConvertAndInsertData.exe schemaname " + filename
                }
            };
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
        }

        private static void ReadFileToDictionary(string filepath)
        {
            new List<string>(File.ReadAllLines(filepath)).ForEach(line =>
            {
                var _site = new Dictionary<string,string>();
                var parsedData = line.Split(',');
                for(var i = 1; i < Headers.Length; i++)
                {
                    _site[Headers[i]] = parsedData[i].Trim();
                }
                Sites[parsedData[0]] = _site;
            });
        }

        private static string GetRiposteSitesData(string siteId, string property)
        {
            Process p = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = "cmd.exe",
                    Arguments = string.Format("/C chcp 65001 && c:\\WebRiposte\\tools\\WebRiposteObject.exe Get /AssetManager/AMContainers/PostOffice/Properties/{0} \"\" {1}", property, siteId)
                }
            };
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            return new Regex("(?<=Value:)[^>]*").Match(output).Value;
        }

        private static string ValitateSites()
        {
            try
            {
                foreach(var siteId in Sites.Keys)
                {
                    Console.WriteLine("SITE: "+siteId);
                    foreach(var property in Sites[siteId].Keys)
                    {
                        Console.WriteLine(property + "=" + Sites[siteId][property]);
                        var ER = Sites[siteId][property];
                        var AR = GetRiposteSitesData(siteId, property);
                        Assert.AreEqual(ER, AR, string.Format("GroupId:{0}, PropertyName:{1}",siteId, property));
                    }
                }
            }
            catch(Exception e)
            {
                return e.Message;
            }
            return "Validation successful";
        }
    }
}
