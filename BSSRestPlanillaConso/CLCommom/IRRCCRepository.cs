
using System.Collections.Generic;

namespace CLCommom
{
    public interface IRRCCRepository
    {
        string Add(RRCC rrcc);
        string AddRPLANI(RRCC rrcc);
        string AddERRPLA(RRCC rrcc);
        List<RRCC> FindConso(string Conso, string rapp);
        string AddReporte(RRCC rrcc);
        RRCC FindId(string Id);
    }
}
