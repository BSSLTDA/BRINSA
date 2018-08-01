using System.Collections.Generic;

namespace CLCommom
{
    public class RFNCDET
    {
        public string DLINEA { get; set; }
        public string DCODPRO { get; set; }
        public string DDESPRO { get; set; }
        public string DUNIMED { get; set; }
        public decimal DCANTID { get; set; }
        public decimal DVALUNI { get; set; }
        public decimal DVALTOT { get; set; }
        public decimal DFVALTOT { get; set; }
        public string DFPORIMP { get; set; }
        public decimal DVALIMP { get; set; }
    }
    public interface IRFNCDETRepository
    {
        List<RFNCDET> Find(string Prefijo, string Nota);
    }
}
