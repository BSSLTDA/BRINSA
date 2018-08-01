
using System.Collections.Generic;

namespace CLCommom
{
    public interface IRDCCRepository
    {
        RDCC GetInfo(string DPLANI);
        List<RDCC> Get(string DCONSO);
        List<RDCC> Getdata(string DCONSO);
        List<RDCC> Getdata2(string DCONSO);
        string Update(RDCC rdcc);
        string Updatedata(RDCC rdcc);
        List<RDCC> GetDPLANI(string DCONSO);
        string UpdateConso(string Conso);
        string UpdateDNUMENT(string Conso);
        List<RDCC> GetDespacho(string DCONSO);
        List<RDCC> GetDestino(string DCONSO);
        List<RDCC> GetPlanilla(string DCONSO);
    }
}
