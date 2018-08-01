using System;

namespace CLCommom
{
    public class RFNCCAB
    {
        public string FPREFIJ { get; set; }
        public string FNOTA { get; set; }
        public string FFECNC { get; set; }
        public string FFECVEN { get; set; }
        public string FPEDIDO { get; set; }
        public string FMONEDA { get; set; }
        public string FCLIENT { get; set; }
        public string FNOMCLI { get; set; }
        public string FDIRCLI1 { get; set; }
        public string FDIRCLI3 { get; set; }
        public string FDEPCLI { get; set; }
        public string FPAICLI { get; set; }
        public string FNIT { get; set; }
        public string FNOMPEN { get; set; }
        public string FDIRPEN1 { get; set; }
        public string FDIRPEN3 { get; set; }
        public string FDEPPEN { get; set; }
        public string FPAIPEN { get; set; }
        public decimal FSUBTOT { get; set; }
        public decimal FIMPUES { get; set; }
        public decimal FTOTNC { get; set; }
        public DateTime FACRTDAT { get; set; }
        public string TotLineas { get; set; }
        public string SUFD05 { get; set; }
        public string SUFD06 { get; set; }
        public string SUFD13 { get; set; }
        public string SUFD14 { get; set; }
        public string SUFD17 { get; set; }
        public string SUFD19 { get; set; }
        public string CPHON { get; set; }
        public string CMAD6 { get; set; }
        public string CCON { get; set; }        
    }
    public interface IRFNCCABRepository
    {
        RFNCCAB Find(string Prefijo, string Nota);
        string UpdNotaIdNme(string Prefijo, string Nota, string Id, string Nme);
        string UpdNotaResPDF(string Prefijo, string Nota, string Resultado);
    }
}
