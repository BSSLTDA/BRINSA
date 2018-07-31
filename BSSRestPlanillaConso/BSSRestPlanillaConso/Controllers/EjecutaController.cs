using BSSRestPlanillaConso.Models;
using CLCommom;
using CLDB2;
using System.Collections.Generic;
using System.Web.Http;

namespace BSSRestPlanillaConso.Controllers
{
    public class EjecutaController : ApiController
    {
        IRECCRepository dRECC = new RECCRepository();
        IRDCCRepository dRDCC = new RDCCRepository();
        IRRCCRepository dRRCC = new RRCCRepository();
        IRRCCTRepository dRRCCT = new RRCCTRepository();
        IRHCCRepository dRHCC = new RHCCRepository();
        IRFLOGRepository dRFLOG = new RFLOGRepository();
        IRFWRKGRLRepository dRFWRKGRL = new RFWRKGRLRepository();
        IRDRIVERDATRepository dRDRIVERDAT = new RDRIVERDATRepository();
        IRFTASKRepository dRFTASK = new RFTASKRepository();
        RECC mRECC = new RECC();
        RDCC mRDCC = new RDCC();
        RRCC mRRCC = new RRCC();
        RRCCT mRRCCT = new RRCCT();
        RHCC mRHCC = new RHCC();
        RFLOG mRFLOG = new RFLOG();
        RDRIVERDAT mRDRIVERDAT = new RDRIVERDAT();
        RFWRKGRL mRFWRKGRL = new RFWRKGRL();
        List<RDCC> lmRDCC = new List<RDCC>();
        List<RECC> lmRECC = new List<RECC>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fExe"></param>
        /// <returns></returns>
        public IHttpActionResult PostEjecuta(FiltroEjecuta fExe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string res = "";

            switch (fExe.que)
            {
                case "PlanillaOK":
                    res = Planilla(fExe.xplanilla);
                    break;
                case "chkTruePlani":
                    res = ChkTruePlanilla(fExe.xplanilla, fExe.psender, fExe.phora, fExe.rega);
                    break;
                case "bErrSMS":
                    res = BErrSMSChk(fExe.phora, fExe.preporte, fExe.psender, fExe.ok);
                    break;
                case "Consolidado":
                    res = Conso(fExe.xplanilla, fExe.phora, fExe.ok, fExe.preporte);
                    break;
                case "SMSRecibido":
                    res = Recib(fExe.xplanilla);
                    break;
                case "Bateria":
                    res = EstadoBateria(fExe.xplanilla);
                    break;
            }

            return Json(new { Resultado = res });
        }

        private string Recib(string Recibido)
        {
            string result = "";
            mRFLOG.OPERACION = "Mensaje Recibido";
            mRFLOG.EVENTO = Recibido.Replace("'", "''");
            mRFLOG.ALERT = "1";
            result = dRFLOG.Add(mRFLOG);
            return result;
        }

        private string Conso(string consoli, string hora, string entrada, string planta)
        {
            string result = "";
            bool sale = false;
            bool entra = false;
            var Prove = "";
            mRHCC = null;
            mRHCC = new RHCC()
            {
                HCONSO = consoli,
                HPLTA = planta
            };
            mRHCC = dRHCC.GetConsoSalida(mRHCC);
            if (mRHCC != null)
            {
                Prove = mRHCC.HPROVE;
                entrada = "False";
                if (mRHCC.HSTS >= 30 && mRHCC.HSTS <= 50)
                {
                    sale = true;
                    entrada = "False";
                }
                else
                {
                    mRFLOG.OPERACION = "Consulta RHCC Estado";
                    mRFLOG.EVENTO = "Consolidado " + consoli + " con estado " + mRHCC.HSTS;
                    mRFLOG.ALERT = "0";
                    mRFLOG.USUARIO = "";
                    result = dRFLOG.Add(mRFLOG);
                    if (result == "OK")
                    {
                        result = "RHCC Estado no esta entre 30 y 50";
                    }
                }
                if (mRHCC.HSTS > 50)
                {
                    sale = false;
                    entrada = "False";
                }

                if (entrada == "OK")
                {
                    mRRCC.RREPORT = "Llegada a Planta";
                    mRRCC.RCRTDAT = hora;
                    mRRCC.RAPP = "SMSENTRADA";
                    mRRCC.HCONSO = consoli;
                    result = dRRCC.Add(mRRCC);
                    if (result == "OK")
                    {
                        mRRCCT.RFEC17 = hora;
                        mRRCCT.RCONSO = consoli;
                        result = dRRCCT.Update(mRRCCT);
                        if (result == "OK" && entra)
                        {
                            mRHCC.HESTAD = "17";
                            mRHCC.HLLEGA = hora;
                            mRHCC.HCONSO = consoli;
                            mRHCC.HSTS = 20;
                            result = dRHCC.Update(mRHCC);
                        }
                    }
                }
                else
                {
                    var condespacho = dRDCC.GetDespacho(consoli);
                    if (condespacho.Count != 0)
                    {
                        var existRRCC = dRRCC.FindConso(consoli, "SMSSALIDA");
                        if (existRRCC.Count == 0)
                        {
                            if (sale)
                            {
                                mRHCC = null;
                                mRHCC = new RHCC
                                {
                                    HESTAD = "5",
                                    HSALE = hora,
                                    HCONSO = consoli,
                                    HSTS = 50
                                };
                                result = dRHCC.Update(mRHCC);
                            }
                            else
                            {
                                result = "OK";
                            }
                            if (result == "OK")
                            {
                                mRRCC.RREPORT = "Salida de Planta";
                                mRRCC.RCRTDAT = hora;
                                mRRCC.RAPP = "SMSSALIDA";
                                mRRCC.HCONSO = consoli;
                                result = dRRCC.Add(mRRCC);
                                if (result == "OK")
                                {
                                    mRDRIVERDAT = dRDRIVERDAT.GetInfo(consoli);
                                    if (mRDRIVERDAT == null)
                                    {
                                        result = dRDRIVERDAT.Add(consoli);
                                    }
                                    if (result == "OK")
                                    {
                                        mRRCCT = null;
                                        mRRCCT = new RRCCT();
                                        mRRCCT.RFEC50 = hora;
                                        mRRCCT.RCONSO = consoli;
                                        result = dRRCCT.Update(mRRCCT);
                                    }
                                }
                            }
                            var nRFTASK = new RFTASK()
                            {
                                TPRM = "|CONSO=" + consoli,
                                TKEY = consoli,
                                TKEYWORD = consoli,
                                TCRTUSR = "BSS"
                            };
                            result = dRFTASK.Add(nRFTASK);
                        }
                        else
                        {
                            result = "Ya se realizó la salida";
                        }
                    }
                    else
                    {
                        mRFLOG.OPERACION = "Consulta Despacho";
                        mRFLOG.EVENTO = "Consolidado " + consoli + " sin despacho";
                        mRFLOG.ALERT = "1";
                        result = dRFLOG.Add(mRFLOG);
                    }
                }
                if (Prove == "5115")
                {
                    var nTra = new FiltroTrafico()
                    {
                        ACCION = "nuevotrafico",
                        USUARIO = "BSS2",
                        PASSWORD = "PROCESOS",
                        CONSO = consoli
                    };
                    var t = new TraficoController();
                    t.Reporte(nTra);                    
                }
            }
            else
            {
                mRFLOG.OPERACION = "Consulta Consolidado";
                mRFLOG.EVENTO = "No Existe Consolidado " + consoli;
                mRFLOG.ALERT = "1";
                result = dRFLOG.Add(mRFLOG);
            }

            return result;
        }

        private string BErrSMSChk(string pHora, string pReporte, string pSender, string chk)
        {
            string result = "";

            if (chk == "OK")
            {
                mRRCC.RFECREP = pHora;
                mRRCC.RREPORT = pReporte;
                mRRCC.RCRTUSR = pSender;
                result = dRRCC.AddERRPLA(mRRCC);
            }
            else
            {
                mRFWRKGRL.WFTS01 = pHora;
                mRFWRKGRL.WDESC = "Movil";
                mRFWRKGRL.WTXT1 = pReporte.Substring(0, 250);
                result = dRFWRKGRL.Add(mRFWRKGRL);
            }

            return result;
        }

        private string ChkTruePlanilla(string xPlanilla, string pSender, string pHora, string regA)
        {
            string result = "";
            string ECELOK = "";

            mRDCC = dRDCC.GetInfo(xPlanilla);
            if (mRDCC != null)
            {
                mRRCC.RPLANI = xPlanilla;
                mRRCC.HCONSO = mRDCC.DCONSO;
                mRRCC.RSTSC = mRDCC.HSTS;
                mRRCC.RFECREP = pHora;
                mRRCC.RREPORT = xPlanilla + " Fin de Entrega";
                mRRCC.RCRTUSR = pSender;
                result = dRRCC.AddRPLANI(mRRCC);
                if (result == "OK")
                {
                    mRHCC = null;
                    mRHCC = new RHCC();
                    mRHCC.RREPORT = xPlanilla + " Fin de Entrega";
                    mRHCC.RFECREP = pHora;
                    mRHCC.HCONSO = mRDCC.DCONSO;
                    result = dRHCC.UpdateHREPORT(mRHCC);
                    if (result == "OK")
                    {
                        if (pSender.Trim() == mRDCC.HCELU.Replace(" ", "").Replace("-", ""))
                        {
                            ECELOK = "1";
                        }
                        result = dRECC.UpdateESTS2(pHora, xPlanilla, ECELOK);
                        if (result == "OK")
                        {
                            if (regA == "0" || regA == "" || regA == null)
                            {
                                ActualizaRECC(mRDCC.DCONSO);
                            }
                            result = dRECC.UpdateConso2(mRDCC.DCONSO);
                            if (result == "OK")
                            {
                                lmRECC = dRECC.GetData3(mRDCC.DCONSO);
                                if (lmRECC == null || lmRECC.Count == 0)
                                {
                                    mRHCC = null;
                                    mRHCC = new RHCC();
                                    mRHCC.HFESTAD = pHora;
                                    mRHCC.HCONSO = mRDCC.DCONSO;
                                    result = dRHCC.UpdateHESTAD(mRHCC);
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }
        
        private string Planilla(string plani)
        {
            string result = "";
            int output;

            if (int.TryParse(plani, out output))
            {
                mRECC = dRECC.GetPlani(plani);
                if (mRECC != null)
                {
                    result = "OK";
                }
                else
                {
                    mRDCC = dRDCC.GetInfo(plani);
                    if (mRDCC != null)
                    {
                        ActualizaRECC(mRDCC.DCONSO);
                        mRECC = dRECC.GetPlani(plani);
                        if (mRECC != null)
                        {
                            result = "OK";
                        }
                    }
                }
            }
            return result;
        }

        private void ActualizaRECC(string Conso)
        {
            string res = "";
            bool bCompleto;

            lmRDCC = dRDCC.Get(Conso);
            if (lmRDCC != null)
            {
                if (lmRDCC.Count > 0)
                {
                    for (int i = 0; i <= lmRDCC.Count; i++)
                    {
                        mRDCC.DPLANI = lmRDCC[i].LLLOAD;
                        mRDCC.DPEDID = lmRDCC[i].DPEDID;
                        mRDCC.DLINEA = lmRDCC[i].DLINEA;
                        res = dRDCC.Update(mRDCC);
                    }
                }
            }
            lmRDCC = dRDCC.Getdata(Conso);
            if (lmRDCC != null)
            {
                if (lmRDCC.Count > 0)
                {
                    for (int i = 0; i <= lmRDCC.Count; i++)
                    {
                        mRDCC.DLDPFX = lmRDCC[i].ILDPFX;
                        mRDCC.DLDOCN = lmRDCC[i].ILDOCN;
                        mRDCC.DPEDID = lmRDCC[i].DPEDID;
                        mRDCC.DLINEA = lmRDCC[i].DLINEA;
                        res = dRDCC.Updatedata(mRDCC);
                    }
                }
            }
            bCompleto = true;
            lmRDCC = dRDCC.GetDPLANI(Conso);
            if (lmRDCC != null)
            {
                if (lmRDCC.Count > 0)
                {
                    bCompleto = false;
                }
            }

            lmRECC = dRECC.GetData(Conso);
            if (lmRECC != null)
            {
                if (lmRECC.Count > 0)
                {
                    bCompleto = false;
                }
            }

            if (bCompleto)
            {
                dRECC.Delete(Conso);
                return;
            }

            dRDCC.UpdateConso(Conso);
            dRECC.UpdateConso(Conso);

            lmRDCC = dRDCC.Getdata2(Conso);
            if (lmRDCC != null)
            {
                if (lmRDCC.Count > 0)
                {
                    for (int i = 0; i < lmRDCC.Count; i++)
                    {
                        mRECC.ECONSO = Conso;
                        mRECC.EPEDID = lmRDCC[i].DPEDID;
                        mRECC.EIDIVI = lmRDCC[i].DIDIVI.Trim();
                        mRECC.ECUST = lmRDCC[i].DCUST;
                        mRECC.ESHIP = lmRDCC[i].DSHIP;

                        lmRECC = dRECC.GetData2(mRECC);
                        if (lmRECC != null)
                        {
                            dRECC.UpdatePlani(lmRDCC[i].DPLANI, lmRECC[0].EID);
                        }
                        else
                        {
                            dRECC.Add(lmRDCC[i].DPLANI);
                        }
                    }
                }
            }

            dRECC.UpdateSelect(Conso);
            dRECC.Delete(Conso);
            dRDCC.UpdateDNUMENT(Conso);
        }

        private string EstadoBateria(string estado)
        {
            string result = "";

            return result;
        }
    }
}
