using BSSRestPlanillaConso.Models;
using CLCommom;
using CLDB2;
using System;
using System.Web.Http;

namespace BSSRestPlanillaConso.Controllers
{
    public class ReporteController : ApiController
    {
        IRHCCRepository dRHCC = new RHCCRepository();
        IRRCCRepository dRRCC = new RRCCRepository();
        IRCAURepository dRCAU = new RCAURepository();
        RHCC mRHCC = new RHCC();
        RRCC mRRCC = new RRCC();

        //public IHttpActionResult PostReporte(FiltroConso recibe)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    string res = "";

        //    res = dRCAU.ExisteUSR(recibe.USUARIO, recibe.PASSWORD);
        //    if (res == "OK")
        //    {
        //        mRHCC = dRHCC.GetCONSO(recibe.CONSO.Trim());
        //        if (mRHCC != null)
        //        {
        //            if (mRHCC.HSTS >= 50 && mRHCC.HSTS <= 59)
        //            {
        //                mRRCC.RTREP = "41";
        //                mRRCC.RCONSO = recibe.CONSO.Trim();
        //                mRRCC.RTIPORI = "2";
        //                mRRCC.RTIPNOV = "3";
        //                mRRCC.RREPORT = recibe.REPORTE;
        //                mRRCC.RCRTUSR = recibe.USUARIO;
        //                mRRCC.RAPP = "REPORTREST";
        //                mRRCC.RWINDAT = DateTime.Now.ToString("yyyy-MM-dd-HH.mm.ss.000000");
        //                mRRCC.RFECREP = DateTime.Now.ToString("yyyy-MM-dd-HH.mm.ss.000000");
        //                mRRCC.RCODUBI = "000";
        //                mRRCC.RSTSE = "1";
        //                res = dRRCC.Add(mRRCC);
        //                if (res == "OK")
        //                {
        //                    mRHCC.HFECREP = DateTime.Now.ToString("yyyy-MM-dd-HH.mm.ss.000000");
        //                    if (recibe.REPORTE.Length > 50)
        //                    {
        //                        mRHCC.HREPORT = recibe.REPORTE.Substring(0, 50);
        //                    }
        //                    else
        //                    {
        //                        mRHCC.HREPORT = recibe.REPORTE;
        //                    }
        //                    mRHCC.HCONSO = recibe.CONSO.Trim();
        //                    res = dRHCC.Update(mRHCC);
        //                }
        //            }
        //            else
        //            {
        //                res = "El Consolidado no está entre los estados 50 y 59";
        //            }
        //        }
        //        else
        //        {
        //            res = "No existe Consolidado";
        //        }
        //    }
        //    return Json(new { Respuesta = res });
        //}

        public IHttpActionResult PostReportar(FiltroConso recibe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string res = "";

            res = dRCAU.ExisteUSR(recibe.USUARIO, recibe.PASSWORD);
            if (res == "OK")
            {               
                mRRCC.RCONSO = recibe.CONSO.Trim();
                mRRCC.RPLANI = recibe.PLANILLA;
                mRRCC.RPLACA = recibe.PLACA;
                mRRCC.RFECREP = recibe.FECREP;
                mRRCC.RUBIC = recibe.CODDANE;                
                mRRCC.RTIPNOV = recibe.CODNOVEDAD;
                mRRCC.RREPORT = recibe.REPORTE;
                mRRCC.RCRTUSR = recibe.USUARIO;                
                mRRCC.RWINDAT = DateTime.Now.ToString("yyyy-MM-dd-HH.mm.ss.000000");                
                res = dRRCC.AddReporte(mRRCC);                
            }
            return Json(new { Respuesta = res });
        }
    }
}
