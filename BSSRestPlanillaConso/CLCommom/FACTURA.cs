using System.Collections.Generic;
using System.Xml.Serialization;

namespace CLCommon
{
   
    public class FACTURA
    {
        [XmlElement("ENC", typeof(ENC))]
        public List<ENC> LMdlENC { get; set; }
        [XmlElement("EMI", typeof(EMI))]
        public List<EMI> LMdlEMI { get; set; }
        [XmlElement("ADQ", typeof(ADQ))]
        public List<ADQ> LMdlADQ { get; set; }
        [XmlElement("TOT", typeof(TOT))]
        public List<TOT> LMdlTOT { get; set; }
        [XmlElement("TIM", typeof(TIM))]
        public List<TIM> LMdlTIM { get; set; }
        [XmlElement("TDC", typeof(TDC))]
        public List<TDC> LMdlTDC { get; set; }
        public List<MdlOTC> LMdlOTC { get; set; }
        public List<ANT> LMdlANT { get; set; }
        [XmlElement("DSC", typeof(DSC))]
        public List<DSC> LMdlDSC { get; set; }
        [XmlElement("DRF", typeof(DRF))]
        public List<DRF> LMdlDRF { get; set; }
        public List<MdlITD> LMdlITD { get; set; }
        public List<MdlFID> LMdlFID { get; set; }
        public List<MdlRCP> LMdlRCP { get; set; }
        public List<MdlQFA> LMdlQFA { get; set; }
        public List<AQF> LMdlAQF { get; set; }
        [XmlElement("NOT", typeof(NOT))]
        public List<NOT> LMdlNOT { get; set; }
        [XmlElement("ORC", typeof(ORC))]
        public List<ORC> LMdlORC { get; set; }
        [XmlElement("REF", typeof(REF))]
        public List<REF> LMdlREF { get; set; }
        public List<MdlCRF> LMdlCRF { get; set; }
        [XmlElement("IEN", typeof(IEN))]
        public List<IEN> LMdlIEN { get; set; }
        public List<MdlTET> LMdlTET { get; set; }
        public List<MdlMEP> LMdlMEP { get; set; }
        public List<MdlCNP> LMdlCNP { get; set; }
        public List<MdlCTS> LMdlCTS { get; set; }
        public List<MdlPAY> LMdlPAY { get; set; }
        public List<MdlOVT> LMdlOVT { get; set; }
        public List<MdlRBC> LMdlRBC { get; set; }
        public List<MdlCDN> LMdlCDN { get; set; }
        public List<MdlISH> LMdlISH { get; set; }
        public List<MdlSNO> LMdlSNO { get; set; }
        public List<MdlFE1> LMdlFE1 { get; set; }
        public List<MdlFE2> LMdlFE2 { get; set; }
        public List<MdlCDM> LMdlCDM { get; set; }
        public List<MdlMAN> LMdlMAN { get; set; }
        [XmlElement("ITE", typeof(ITE))]
        public List<ITE> LMdlITE { get; set; }
        public List<MdlNRF> LMdlNRF { get; set; }
        public List<MdlNCN> LMdlNCN { get; set; }
        public List<MdlNDR> LMdlNDR { get; set; }
    }
}
