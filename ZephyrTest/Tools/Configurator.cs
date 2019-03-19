using Microsoft.Extensions.Configuration;
using System.IO;

namespace ZephyrTest.Tools
{
    class Configurator
    {
        public static IConfigurationRoot GetConfiguration(string name) =>
            new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory() + "\\ZephyrTest\\Configs\\")
                    .AddJsonFile(name + ".json")
                    .Build();
    }
}
