using System.Collections.Generic;

namespace CLCommom
{
    public interface IRFPARAMRepository
    {
        RFPARAM GetDirPDF(string cctabl, string cccode);
        List<RFPARAM> GetParams();
        string MontoEscrito(string num);
    }
}
