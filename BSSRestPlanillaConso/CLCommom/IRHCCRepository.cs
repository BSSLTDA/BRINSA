using System.Collections.Generic;

namespace CLCommom
{
    public interface IRHCCRepository
    {
        RHCC GetConsoSalida(RHCC rhcc);
        string Update(RHCC rhcc);
        string UpdateHREPORT(RHCC rhcc);
        string UpdateHESTAD(RHCC rhcc);
        RHCC GetConsoTrafico(string Cons);
        List<RHCCEnTransito> GetEnTransito(string hprove);
        RHCC GetCONSO(string conso);
    }
}
