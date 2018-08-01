using BSSRestPlanillaConso.Models;
using CLCommom;
using CLDB2;
using System.Web.Http;

namespace BSSRestPlanillaConso.Controllers
{
    public class GpsController : ApiController
    {
        IRHCCXRepository dRHCCX = new RHCCXRepository();
        IRCAURepository dRCAU = new RCAURepository();

        public IHttpActionResult Postgps(FiltroConso fgps)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string res = "";
            RHCCX rhccx = null;
            res = dRCAU.ExisteUSR(fgps.USUARIO, fgps.PASSWORD);
            if (res == "OK")
            {
                rhccx = dRHCCX.GetGPS(fgps.CONSO);
                if (rhccx == null)
                {
                    return NotFound();
                }
            }
            return Json(rhccx);
        }
    }
}
