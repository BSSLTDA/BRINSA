
using System.Collections.Generic;

namespace CLCommom
{
    public interface IRECCRepository
    {
        RECC GetPlani(string Planilla);
        List<RECC> GetData(string Conso);
        List<RECC> GetData2(RECC recc);
        List<RECC> GetData3(string Conso);
        void Delete(string Conso);
        string UpdateConso(string Conso);
        string UpdateConso2(string Conso);
        string UpdatePlani(string EPLANI, string EID);
        string UpdateESTS(string EFINENT, string EPLANI);
        string UpdateESTS2(string EFINENT, string EPLANI, string ECELOK);
        string UpdateSelect(string CONSO);
        string Add(string DPLANI);
    }
}
