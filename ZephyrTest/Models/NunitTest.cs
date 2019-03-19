namespace ZephyrTest.Models
{
    class NunitTest
    {
        public bool Failed { get; set; }
        public string ActualResult { get; set; }
        public string ExpectedResult { get; set; }

        public NunitTest()
        {
            Failed = false;
            ActualResult = "0";
            ExpectedResult = "0";
        }
    }
}
