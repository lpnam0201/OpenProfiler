namespace OpenProfiler.WPF.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Serialization;

    [XmlRoot(Namespace = "log4net", ElementName = "event")]
    public class Event
    {
        [XmlIgnore]
        public Guid SessionId
        {
            get
            {
                return this.Properties
                    .Where(pr => pr.Name == "sessionId")
                    .Select(pr => Guid.Parse(pr.Value))
                    .SingleOrDefault();
            }
        }

        [XmlAttribute("logger")]
        public string Logger { get; set; }

        [XmlAttribute("timestamp")]
        public DateTime TimeStamp { get; set; }

        [XmlAttribute("level")]
        public string Level { get; set; }

        [XmlAttribute("thread")]
        public int Thread { get; set; }

        [XmlAttribute("domain")]
        public string Domain { get; set; }

        [XmlElement("message")]
        public string Message { get; set; }

        [XmlArray("properties")]
        [XmlArrayItem("data")]
        public Property[] Properties { get; set; }
    }
}
