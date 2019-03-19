using System;
using ZephyrTest.Enums;

namespace ZephyrTest.Models
{
    class StepBinding
    {
        public Drivers Driver { get; set; }
        public Func<string, string> StepReference { get; set; }
    }
}
