using BSSRestPlanillaConso.Models;
using CLCommom;
using CLDB2;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace BSSRestPlanillaConso.Controllers
{
    public class apiBRINSAController : ApiController
    {
        RHCC mRHCC = new RHCC();
        List<RDCC> lmRDCC = new List<RDCC>();
        IRHCCRepository dRHCC = new RHCCRepository();
        IRDCCRepository dRDCC = new RDCCRepository();
        IRCAURepository dRCAU = new RCAURepository();
        //RCAU mRCAU = new RCAU();

        public IHttpActionResult PostApiBrinsa(string Conso, string Accion)
        {
            var res = "";

            switch (Accion.ToUpper())
            {
                case "NUEVOTRAFICO":
                    res = NewTraffic(Conso);
                    break;
            }

            return Json(new { Resultado = res });
        }

        /// <summary>
        /// Reenvio Transito segun accion
        /// </summary>
        /// <param name="FRT">
        /// {"USUARIO": "USR", "PASSWORD": "PSS", "CONSO": "999999", "ACCION": "NUEVOTRAFICO"}
        /// </param>
        /// <returns>IHttpActionResult</returns>
        public IHttpActionResult ReenvioTransito(FiltroReenvioTransito FRT)
        {
            var res = "";

            res = dRCAU.ExisteUSR(FRT.USUARIO, FRT.PASSWORD);
            if (res == "OK")
            {
                switch (FRT.ACCION.ToUpper())
                {
                    case "NUEVOTRAFICO":
                        res = NewTraffic(FRT.CONSO);
                        break;
                }
            }
            
            return Json(new { Resultado = res });
        }

        private string NewTraffic(string consoli)
        {
            var resu = "";

            mRHCC = dRHCC.GetConsoTrafico(consoli);
            if (mRHCC != null)
            {
                var Sts = 0;
                var Destinos = "";
                var Planillas = "";
                if (mRHCC.HSTS < 50)
                {
                    Sts = 54;
                }
                else
                {
                    Sts = 24;
                }

                lmRDCC = dRDCC.GetDestino(consoli);
                if (lmRDCC != null)
                {
                    if (lmRDCC.Count > 0)
                    {
                        Destinos = "";
                        foreach (var item in lmRDCC)
                        {
                            if (Destinos == "")
                            {
                                Destinos = item.Destino.Trim();
                            }
                            else
                            {
                                Destinos = Destinos + "," + item.Destino.Trim();
                            }
                        }
                    }
                }

                lmRDCC = dRDCC.GetPlanilla(consoli);
                if (lmRDCC != null)
                {
                    if (lmRDCC.Count > 0)
                    {
                        Planillas = "";
                        foreach (var item in lmRDCC)
                        {
                            if (Planillas == "")
                            {
                                Planillas = item.DPLANI.Trim();
                            }
                            else
                            {
                                Planillas = Planillas + "," + item.DPLANI.Trim();
                            }
                        }
                    }
                }

                var hh = new com.nygsoft.huella.Servicioingresonuevotrafico();
                var respuesta = hh.nuevoTrafico(mRHCC.HPLACA, mRHCC.HMANIF, mRHCC.HCEDUL, mRHCC.HCHOFE, mRHCC.HCELU, int.Parse(mRHCC.Origen.Trim()), Destinos, "", DateTime.Now, DateTime.Now, mRHCC.XCGPSPAG, mRHCC.XCGPSUSR, mRHCC.XCGPSPASS, "brinsa", "brinsa2017", "", "", "", "", Sts, int.Parse(consoli), Planillas);
                
                resu = respuesta[0].mensaje;
            }

            return resu;
        }

    }
}
