
namespace CLCommon
{
    public class RCO
    {
        public string COEXID { get; set; }
        public string CVATNM { get; set; }
        public string COAUTN { get; set; }
        public string COAUTB { get; set; }
        public string CMPNAM { get; set; }
        public string CMPAD1 { get; set; }
        public string COSTE { get; set; }
        public string COADR3 { get; set; }
        public string CMPOST { get; set; }
        public string COCRCC { get; set; }
        public string COCRNO { get; set; }
        public string COADR4 { get; set; }
    }

    public interface IRCORepository
    {
        RCO GetCompany();
    }
}
