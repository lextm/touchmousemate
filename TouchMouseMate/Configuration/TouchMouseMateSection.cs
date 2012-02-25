using System.Configuration;
using System.Globalization;

namespace Lextm.TouchMouseMate.Configuration
{
    public class TouchMouseMateSection : ConfigurationSection
    {
        /// <summary>
        /// Gets the StringValue setting.
        /// </summary>
        [ConfigurationProperty("minClickTimeout", IsRequired = true)]
        [IntegerValidator]
        public int MinClickTimeout
        {
            get { return (int)base["minClickTimeout"]; }
        }

        /// <summary>
        /// Gets the StringValue setting.
        /// </summary>
        [ConfigurationProperty("maxClickTimeout", IsRequired = true)]
        [IntegerValidator]
        public int MaxClickTimeout
        {
            get { return (int)base["maxClickTimeout"]; }
        }

        [ConfigurationProperty("middleClick", IsRequired = true)]
        public bool MiddleClick
        {
            get { return ((bool)base["middleClick"]); }
            set { base["middleClick"] = value; }
        }

        [ConfigurationProperty("touchOverClick", IsRequired = true)]
        public bool TouchOverClick
        {
            get { return (bool)base["touchOverClick"]; }
            set { base["touchOverClick"] = value; }
        }
    }
}
