
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CLCommon
{
    public class TII
    {
        public string TII_1 { get; set; }
        public string TII_2 { get; set; }
        public string TII_3 { get; set; }
        [XmlElement("IIM", typeof(IIM))]
        public List<IIM> LMdlIIM { get; set; }
    }
}
