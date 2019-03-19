using System;
using ZephyrTest.Core.AppObjects;

namespace ZephyrTest.Core.Steps

{
    class TestSteps
    {
        public static string Login(string data)
        {
            return "User is able to login successfully";
        }

        public static string SendMoney(string data)
        {
            return Convert.ToInt32(data)<1000?"Sent":"Error, exceed daily limit";
        }

        public static string AddNumbers(string data)
        {
            try
            {
                return Calculator.Add(data.Split(',')[0].Trim(), data.Split(',')[1].Trim());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static string SubtractNumbers(string data)
        {
            try
            {
                return Calculator.Subtract(data.Split(',')[0].Trim(), data.Split(',')[1].Trim());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static string DivideNumbers(string data)
        {
            try
            {
                return Calculator.Divide(data.Split(',')[0].Trim(), data.Split(',')[1].Trim());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static string MultiplyNumbers(string data)
        {
            try
            {
                return Calculator.Multiply(data.Split(',')[0].Trim(), data.Split(',')[1].Trim());
            }
            catch(Exception e)
            {
                return e.Message;
            }
        }
    }
}
