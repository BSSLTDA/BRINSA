using System.Collections.Generic;

namespace CLCommon
{
    public class ZCC
    {
        public string CCCODE { get; set; }
    }

    public interface IZCCRepository
    {
        List<ZCC> GetTablesRut();
    }
}
