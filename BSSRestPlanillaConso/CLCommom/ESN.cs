
using System.Collections.Generic;

namespace CLCommom
{
    public class ESN
    {
        public string SNID { get; set; }
        public string SNTYPE { get; set; }
        public string SNCUST { get; set; }
        public string SNSEQ { get; set; }
        public string SNDESC { get; set; }
        public string SNPRT { get; set; }
        public string SNPIC { get; set; }
        public string SNINV { get; set; }
        public string SNSTMT { get; set; }
        public string SNENDT { get; set; }
        public string SNENTM { get; set; }
        public string SNENUS { get; set; }
        public string CAT { get; set; }
    }

    public interface IESNRepository
    {
        string AddNotas(string Prefijo, string Factura);
        List<ESN> GetNotas(string Prefijo, string Factura);
    }
}
