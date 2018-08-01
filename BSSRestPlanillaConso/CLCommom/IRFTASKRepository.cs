using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLCommom
{
    public interface IRFTASKRepository
    {
        string Add(RFTASK rftask);
        RFTASK FindTask(string Id);
        string CallPgmJva(RFTASK datos);
        string AddEndTraffic(RFTASK rftask);
        string AddAviso(RFTASK rftask);
    }
}
