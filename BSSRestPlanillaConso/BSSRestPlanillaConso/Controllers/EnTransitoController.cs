using BSSRestPlanillaConso.Models;
using CLCommom;
using CLDB2;
using System.Collections.Generic;
using System.Web.Http;

namespace BSSRestPlanillaConso.Controllers
{
    public class EnTransitoController : ApiController
    {
        IRCAURepository dRCAU = new RCAURepository();
        IRHCCRepository dRHCC = new RHCCRepository();
        List<RHCCEnTransito> lmRHCC = new List<RHCCEnTransito>();
        RCAU mRCAU = new RCAU();

        /// <summary>
        /// Consolidados en transito
        /// </summary>
        /// <remarks>
        /// Ejemplo de muestra:
        ///
        ///     POST /En Transito
        ///     {
        ///        "USUARIO": "Usr",
        ///        "PASSWORD": "Pss"
        ///     }
        ///
        /// </remarks>
        public IHttpActionResult Postentransito(FiltroEnTransito fentran)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string res = "";

            res = dRCAU.ExisteUSR(fentran.USUARIO, fentran.PASSWORD);
            if (res == "OK")
            {
                //mRCAU = dRCAU.DatosUSR(fentran.USUARIO, fentran.PASSWORD);
                //if (mRCAU.UQRY != "")
                //{mRCAU.UQRY
                lmRHCC = dRHCC.GetEnTransito("5115");
                    if (lmRHCC == null)
                    {
                        return NotFound();
                    }
                    //lmRHCC[0].TotalRegistros = lmRHCC.Count.ToString();
                //}
                //else
                //{
                //    return BadRequest("El Usuario no tiene una transportadora asignada");
                //}
            }
            return Json(lmRHCC);
        }
    }
}
