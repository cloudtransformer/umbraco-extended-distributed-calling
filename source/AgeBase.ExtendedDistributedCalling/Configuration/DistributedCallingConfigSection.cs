using System.Configuration;

namespace AgeBase.ExtendedDistributedCalling.Configuration
{
    public class DistributedCallingConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("enabled")]
        public bool Enabled
        {
            get { return (bool) this["enabled"]; }
            set { this["enabled"] = value; }
        }

        [ConfigurationProperty("user")]
        public int User
        {
            get { return (int)this["user"]; }
            set { this["user"] = value; }
        }

        [ConfigurationProperty("assembly")]
        public string Assembly
        {
            get { return this["assembly"].ToString(); }
            set { this["assembly"] = value; }
        }

        [ConfigurationProperty("type")]
        public string Type
        {
            get { return this["type"].ToString(); }
            set { this["type"] = value; }
        }
    }
}