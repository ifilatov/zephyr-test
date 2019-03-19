using System.Collections.Generic;

namespace ZephyrTest.Models.Jira
{
    public class Version
    {
        public string Value { get; set; }
        public bool Archived { get; set; }
        public string Label { get; set; }
    }

    public class VersionBoard
    {
        public string Type { get; set; }
        public string HasAccessToSoftware { get; set; }
        public List<Version> UnreleasedVersions { get; set; }
        public List<Version> ReleasedVersions { get; set; }
    }
}
