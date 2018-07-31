using BSSRestPlanillaConso.Models;
using CLCommom;
using CLDB2;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;

namespace BSSRestPlanillaConso.Controllers
{
    public class TraficoController : ApiController
    {
        IRCAURepository dRCAU = new RCAURepository();
        IRHCCRepository dRHCC = new RHCCRepository();
        IRDCCRepository dRDCC = new RDCCRepository();
        IRRCCRepository dRRCC = new RRCCRepository();
        IRFLOGRepository dRFLOG = new RFLOGRepository();
        IRFTASKRepository dRFTASK = new RFTASKRepository();

        RHCC mRHCC = new RHCC();
        RRCC mRRCC = new RRCC();

        List<RDCC> lmRDCC = new List<RDCC>();
        List<RHCCEnTransito> lmRHCCEnTransito = new List<RHCCEnTransito>();

        /// <summary>
        /// API central de Reporte
        /// </summary>        
        /// <param name="FT">       
        /// </param>
        /// <returns></returns>
        public IHttpActionResult Reporte(FiltroTrafico FT)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var res = "";
            StringBuilder txt = new StringBuilder();
            try
            {
                txt.Append("{\"ACCION\":\"" + FT.ACCION + "\",");
                txt.Append(" \"USUARIO\":\"" + FT.USUARIO + "\",");
                txt.Append(" \"PASSWORD\":\"" + FT.PASSWORD + "\",");
                txt.Append(" \"CONSO\":\"" + FT.CONSO + "\",");
                txt.Append(" \"REPORTE\":\"" + FT.REPORTE + "\",");
                txt.Append(" \"PLANILLA\":\"" + FT.PLANILLA + "\",");
                txt.Append(" \"PLACA\":\"" + FT.PLACA + "\",");
                txt.Append(" \"FECREP\":\"" + FT.FECREP.ToString("yyyy-MM-dd HH:mm:ss") + "\",");
                txt.Append(" \"CODDANE\":\"" + FT.CODDANE + "\",");
                txt.Append(" \"CODNOVEDAD\":\"" + FT.CODNOVEDAD + "\",");
                txt.Append(" \"NUMERO\":\"" + FT.NUMERO + "\"}");

                var nRFLOG = new RFLOG()
                {
                    OPERACION = "CapturaJson",
                    EVENTO = "Trafico",
                    TXTSQL = txt.ToString().Replace("'", "''"),
                    ALERT = "0"
                };
                res = dRFLOG.Add(nRFLOG);

                res = dRCAU.ExisteUSR(FT.USUARIO, FT.PASSWORD);
                if (res == "OK")
                {
                    switch (FT.ACCION.ToUpper())
                    {
                        case "NUEVOTRAFICO":
                            res = NewTraffic(FT.CONSO);
                            break;
                        case "ENTRANSITO":
                            var resu = InTransit();
                            return Json(resu);
                        case "REPORTAR":
                            var resul = "";
                            var nRRCC = new RRCC()
                            {
                                RCONSO = FT.CONSO.Trim(),
                                RPLANI = (FT.PLANILLA == "") ? "0" : FT.PLANILLA,
                                RPLACA = FT.PLACA,
                                RFECREP = FT.FECREP.ToString("yyyy-MM-dd-HH.mm.ss.000000"),
                                RUBIC = FT.CODDANE,
                                RTIPNOV = FT.CODNOVEDAD,
                                RREPORT = FT.REPORTE,
                                RCRTUSR = FT.USUARIO,
                                RWINDAT = DateTime.Now.ToString("yyyy-MM-dd-HH.mm.ss.000000")
                            };
                            resul = dRRCC.AddReporte(nRRCC);
                            if (!resul.Contains("ERROR"))
                            {
                                res = "OK";
                            }
                            else
                            {
                                res = resul;
                            }
                            if (res == "OK")
                            {
                                var rep = "";
                                if (FT.REPORTE.Length > 50)
                                {
                                    rep = FT.REPORTE.Substring(0, 50);
                                }
                                else
                                {
                                    rep = FT.REPORTE;
                                }
                                var uRHCC = new RHCC()
                                {
                                    RFECREP = FT.FECREP.ToString("yyyy-MM-dd-HH.mm.ss.000000"),
                                    RREPORT = rep,
                                    HCONSO = FT.CONSO.Trim()
                                };
                                res = dRHCC.UpdateHREPORT(uRHCC);
                                if (res == "OK")
                                {
                                    res = resul;
                                }
                            }
                            break;
                        case "FINALIZARTRAFICO":
                            res = EndTraffic(FT);
                            break;
                        case "NUEVOSEGUIMIENTO":
                            res = NewFllowUp(FT.NUMERO);
                            break;
                        case "NUEVOTRAFICOXML":
                            res = GeneraXMLNew2(FT.CONSO);
                            break;
                        case "NUEVOSEGUIMIENTOXML":
                            res = GeneraXMLFollow2(FT.NUMERO);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                res = "ERROR: " + ex.Message;
                if (ex.InnerException != null)
                {
                    res = res + " - " + ex.InnerException;
                }
                if (ex.StackTrace != null)
                {
                    res = res + " - " + ex.StackTrace;
                }
                res = res + txt.ToString();
            }
            finally
            {
                if (res.Contains("ERROR"))
                {
                    var nRFLOG = new RFLOG()
                    {
                        OPERACION = "TRAFICO",
                        EVENTO = "ERROR",
                        TXTSQL = res.Replace("'", "''"),
                        ALERT = "0"
                    };
                    dRFLOG.Add(nRFLOG);
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

                //var xml = GeneraXMLNew(mRHCC.HPLACA, mRHCC.HMANIF, mRHCC.HCEDUL, mRHCC.HCHOFE, mRHCC.HCELU, int.Parse(mRHCC.Origen.Trim()), Destinos, "", DateTime.Now.ToString("yyyy-MM-dd-HH:mm"), DateTime.Now.ToString("HH:mm:ss"), mRHCC.XCGPSPAG, mRHCC.XCGPSUSR, mRHCC.XCGPSPASS, "brinsa", "brinsa2017", "", "", "", "", Sts, int.Parse(consoli), Planillas);
                var hh = new com.nygsoft.huella.Servicioingresonuevotrafico();
                var respuesta = hh.nuevoTrafico(mRHCC.HPLACA, mRHCC.HMANIF, mRHCC.HCEDUL, mRHCC.HCHOFE, mRHCC.HCELU, int.Parse((mRHCC.Origen == "") ? "0" : mRHCC.Origen.Trim()), Destinos, "", DateTime.Now, DateTime.Now, mRHCC.XCGPSPAG, mRHCC.XCGPSUSR, mRHCC.XCGPSPASS, "brinsa", "brinsa2017", "", "", "", "", Sts, int.Parse(consoli), Planillas);

                resu = respuesta[0].mensaje;
            }

            return resu;
        }

        private string EndTraffic(FiltroTrafico Fil)
        {
            var resu = "";

            mRHCC = dRHCC.GetConsoTrafico(Fil.CONSO);
            if (mRHCC != null)
            {
                //var xml = GeneraXMLEnd(mRHCC.HPLACA, mRHCC.HMANIF, "brinsa", "brinsa2017", int.Parse(consoli));
                var hh = new com.nygsoft.huella.Servicioingresonuevotrafico();
                var respuesta = hh.finalizarTrafico(mRHCC.HPLACA, mRHCC.HMANIF, "brinsa", "brinsa2017", int.Parse(Fil.CONSO));
                resu = respuesta[0].mensaje;
                var nRFTASK = new RFTASK()
                {
                    TKEY = Fil.CONSO,
                    TKEYWORD = Fil.CONSO,
                    TPRM = Fil.CONSO,
                    TPGM = resu
                };
                resu = dRFTASK.AddEndTraffic(nRFTASK);
                if (!resu.Contains("ERROR"))
                {
                    try
                    {
                        var filtro = new FiltroServicios()
                        {
                            USUARIO = Fil.USUARIO,
                            PASSWORD = Fil.PASSWORD,
                            ACCION = "RUTINA",
                            IDTAREA = resu
                        };
                        var s = new ServiciosController();
                        var res = s.Rutinas(filtro);
                        var res2 = ((System.Web.Http.Results.JsonResult<RespuestaRutina>)res).Content;
                        resu = res2.Mensaje;
                    }
                    catch (Exception ex)
                    {
                        var nRFLOG = new RFLOG()
                        {
                            OPERACION = "ENDTRAFFIC",
                            EVENTO = "ERROR",
                            TXTSQL = ex.Message.Replace("'", "''"),
                            ALERT = "0"
                        };
                        resu = dRFLOG.Add(nRFLOG);
                        resu = "ERROR Tarea: " + resu + ex.Message;
                        if (ex.InnerException != null)
                        {
                            resu = resu + " - " + ex.InnerException;
                        }
                        if (ex.StackTrace != null)
                        {
                            resu = resu + " - " + ex.StackTrace;
                        }
                    }
                }
            }

            return resu;
        }

        private string NewFllowUp(string RID)
        {
            var resu = "";

            mRRCC = dRRCC.FindId(RID);
            if (mRRCC != null)
            {
                //var xml = GeneraXMLFollow(mRRCC.RPLACA, mRRCC.RMANIF, mRRCC.RREPORT, DateTime.Parse(mRRCC.RFECREP).ToString("yyyy-MM-dd-HH:mm"), DateTime.Parse(mRRCC.RFECREP).ToString("HH:mm:ss"), mRRCC.RTIPNOV, "24", "brinsa", "brinsa2017", mRRCC.RCONSO, mRRCC.RPLANI);
                var hh = new com.nygsoft.huella.Servicioingresonuevotrafico();
                var respuesta = hh.nuevoSeguimiento(mRRCC.RPLACA, mRRCC.RMANIF, mRRCC.RREPORT, DateTime.Parse(mRRCC.RFECREP), DateTime.Parse(mRRCC.RFECREP), int.Parse(mRRCC.RTIPNOV), 24, "brinsa", "brinsa2017", int.Parse(mRRCC.RCONSO), mRRCC.RPLANI);

                resu = respuesta[0].mensaje;
            }

            return resu;
        }

        private List<RHCCEnTransito> InTransit()
        {
            lmRHCCEnTransito = dRHCC.GetEnTransito("5115");
            if (lmRHCCEnTransito == null)
            {
                return null;
            }
            return lmRHCCEnTransito;
        }

        private string GeneraXMLNew2(string consoli)
        {
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

                StringBuilder xmml = new StringBuilder();
                xmml.Append("<soapenv:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:web='http://localhost:1001/webservice/'>");
                xmml.Append("    <soapenv:Header />");
                xmml.Append("    <soapenv:Body>");
                xmml.Append("        <web:nuevoTrafico soapenv:encodingStyle='http://schemas.xmlsoap.org/soap/encoding/'>");
                xmml.AppendFormat("            <placa xsi:type='xsd:string'>{0}</placa>", mRHCC.HPLACA);
                xmml.AppendFormat("            <manifiesto xsi:type='xsd:string'>{0}</manifiesto>", mRHCC.HMANIF);
                xmml.AppendFormat("            <ccConductor xsi:type='xsd:string'>{0}</ccConductor>", mRHCC.HCEDUL);
                xmml.AppendFormat("            <NombreConductor xsi:type='xsd:string'>{0}</NombreConductor>", mRHCC.HCHOFE);
                xmml.AppendFormat("            <celular xsi:type='xsd:string'>{0}</celular>", mRHCC.HCELU);
                xmml.AppendFormat("            <origen xsi:type='xsd:int'>{0}</origen>", int.Parse((mRHCC.Origen == "") ? "0" : mRHCC.Origen.Trim()));
                xmml.AppendFormat("            <destino xsi:type='xsd:string'>{0}</destino>", Destinos);
                xmml.AppendFormat("            <observacion xsi:type='xsd:string'>{0}</observacion>", "");
                xmml.AppendFormat("            <fechaInicio xsi:type='xsd:date'>{0}</fechaInicio>", DateTime.Now.ToString("yyyy-MM-dd-HH:mm"));
                xmml.AppendFormat("            <horaInicio xsi:type='xsd:time'>{0}</horaInicio>", DateTime.Now.ToString("HH:mm:ss"));
                xmml.AppendFormat("            <UrlGps xsi:type='xsd:string'>{0}</UrlGps>", mRHCC.XCGPSPAG);
                xmml.AppendFormat("            <usuarioGps xsi:type='xsd:string'>{0}</usuarioGps>", mRHCC.XCGPSUSR);
                xmml.AppendFormat("            <ContrasenaGps xsi:type='xsd:string'>{0}</ContrasenaGps>", mRHCC.XCGPSPASS);
                xmml.AppendFormat("            <Usuario xsi:type='xsd:string'>{0}</Usuario>", "brinsa");
                xmml.AppendFormat("            <Clave xsi:type='xsd:string'>{0}</Clave>", "brinsa2017");
                xmml.AppendFormat("            <item1 xsi:type='xsd:string'>{0}</item1>", "");
                xmml.AppendFormat("            <item2 xsi:type='xsd:string'>{0}</item2>", "");
                xmml.AppendFormat("            <item3 xsi:type='xsd:string'>{0}</item3>", "");
                xmml.AppendFormat("            <generadorCarga xsi:type='xsd:string'>{0}</generadorCarga>", "");
                xmml.AppendFormat("            <estado xsi:type='xsd:int'>{0}</estado>", Sts);
                xmml.AppendFormat("            <consolidado xsi:type='xsd:int'>{0}</consolidado>", int.Parse(consoli));
                xmml.AppendFormat("            <planilla xsi:type='xsd:string'>{0}</planilla>", Planillas);
                xmml.Append("        </web:nuevoTrafico>");
                xmml.Append("    </soapenv:Body>");
                xmml.Append("</soapenv:Envelope>");

                return xmml.ToString();
            }
            else
            {
                return null;
            }
        }

        private string GeneraXMLFollow2(string RID)
        {
            mRRCC = dRRCC.FindId(RID);
            if (mRRCC != null)
            {
                StringBuilder xml = new StringBuilder();
                xml.Append("<soapenv:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:web='http://localhost:1001/webservice/'>");
                xml.Append("    <soapenv:Header />");
                xml.Append("    <soapenv:Body>");
                xml.Append("        <web:nuevoseguimiento soapenv:encodingStyle='http://schemas.xmlsoap.org/soap/encoding/'>");
                xml.AppendFormat("            <placa xsi:type='xsd:string'>{0}</placa>", mRRCC.RPLACA);
                xml.AppendFormat("            <manifiesto xsi:type='xsd:string'>{0}</manifiesto>", mRRCC.RMANIF);
                xml.AppendFormat("            <mensaje xsi:type='xsd:string'>{0}</mensaje>", mRRCC.RREPORT);
                xml.AppendFormat("            <fechaMensaje xsi:type='xsd:date'>{0}</fechaMensaje>", DateTime.Parse(mRRCC.RFECREP).ToString("yyyy-MM-dd-HH:mm"));
                xml.AppendFormat("            <horaMensaje xsi:type='xsd:time'>{0}</horaMensaje>", DateTime.Parse(mRRCC.RFECREP).ToString("HH:mm:ss"));
                xml.AppendFormat("            <novedad xsi:type='xsd:int'>{0}</novedad>", mRRCC.RTIPNOV);
                xml.AppendFormat("            <estadoActual xsi:type='xsd:int'>{0}</estadoActual>", "24");
                xml.AppendFormat("            <Usuario xsi:type='xsd:string'>{0}</Usuario>", "brinsa");
                xml.AppendFormat("            <Clave xsi:type='xsd:string'>{0}</Clave>", "brinsa2017");
                xml.AppendFormat("            <Consolidado xsi:type='xsd:int'>{0}</Consolidado>", mRRCC.RCONSO);
                xml.AppendFormat("            <Planilla xsi:type='xsd:string'>{0}</Planilla>", mRRCC.RPLANI);
                xml.Append("        </web:nuevoseguimiento>");
                xml.Append("    </soapenv:Body>");
                xml.Append("</soapenv:Envelope>");
                return xml.ToString();
            }
            else
            {
                return null;
            }
        }

        private XDocument GeneraXMLNew(
            string placa,
                    string manifiesto,
                    string ccConductor,
                    string NombreConductor,
                    string celular,
                    int origen,
                    string destino,
                    string observacion,
                    string fechaInicio,
                    string horaInicio,
                    string UrlGps,
                    string usuarioGps,
                    string ContrasenaGps,
                    string Usuario,
                    string Clave,
                    string item1,
                    string item2,
                    string item3,
                    string generadorCarga,
                    int estado,
                    int consolidado,
                    string planilla)
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<soapenv:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:web='http://localhost:1001/webservice/'>");
            xml.Append("    <soapenv:Header />");
            xml.Append("    <soapenv:Body>");
            xml.Append("        <web:nuevoTrafico soapenv:encodingStyle='http://schemas.xmlsoap.org/soap/encoding/'>");
            xml.AppendFormat("            <placa xsi:type='xsd:string'>{0}</placa>", placa);
            xml.AppendFormat("            <manifiesto xsi:type='xsd:string'>{0}</manifiesto>", manifiesto);
            xml.AppendFormat("            <ccConductor xsi:type='xsd:string'>{0}</ccConductor>", ccConductor);
            xml.AppendFormat("            <NombreConductor xsi:type='xsd:string'>{0}</NombreConductor>", NombreConductor);
            xml.AppendFormat("            <celular xsi:type='xsd:string'>{0}</celular>", celular);
            xml.AppendFormat("            <origen xsi:type='xsd:int'>{0}</origen>", origen);
            xml.AppendFormat("            <destino xsi:type='xsd:string'>{0}</destino>", destino);
            xml.AppendFormat("            <observacion xsi:type='xsd:string'>{0}</observacion>", observacion);
            xml.AppendFormat("            <fechaInicio xsi:type='xsd:date'>{0}</fechaInicio>", fechaInicio);
            xml.AppendFormat("            <horaInicio xsi:type='xsd:time'>{0}</horaInicio>", horaInicio);
            xml.AppendFormat("            <UrlGps xsi:type='xsd:string'>{0}</UrlGps>", UrlGps);
            xml.AppendFormat("            <usuarioGps xsi:type='xsd:string'>{0}</usuarioGps>", usuarioGps);
            xml.AppendFormat("            <ContrasenaGps xsi:type='xsd:string'>{0}</ContrasenaGps>", ContrasenaGps);
            xml.AppendFormat("            <Usuario xsi:type='xsd:string'>{0}</Usuario>", Usuario);
            xml.AppendFormat("            <Clave xsi:type='xsd:string'>{0}</Clave>", Clave);
            xml.AppendFormat("            <item1 xsi:type='xsd:string'>{0}</item1>", item1);
            xml.AppendFormat("            <item2 xsi:type='xsd:string'>{0}</item2>", item2);
            xml.AppendFormat("            <item3 xsi:type='xsd:string'>{0}</item3>", item3);
            xml.AppendFormat("            <generadorCarga xsi:type='xsd:string'>{0}</generadorCarga>", generadorCarga);
            xml.AppendFormat("            <estado xsi:type='xsd:int'>{0}</estado>", estado);
            xml.AppendFormat("            <consolidado xsi:type='xsd:int'>{0}</consolidado>", consolidado);
            xml.AppendFormat("            <planilla xsi:type='xsd:string'>{0}</planilla>", planilla);
            xml.Append("        </web:nuevoTrafico>");
            xml.Append("    </soapenv:Body>");
            xml.Append("</soapenv:Envelope>");

            return XDocument.Parse(xml.ToString());
        }

        private XDocument GeneraXMLEnd(
            string placa,
                    string manifiesto,
                    string Usuario,
                    string Clave,
                    int consolidado)
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<soapenv:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:web='http://localhost:1001/webservice/'>");
            xml.Append("    <soapenv:Header />");
            xml.Append("    <soapenv:Body>");
            xml.Append("        <web:finalizarTrafico soapenv:encodingStyle='http://schemas.xmlsoap.org/soap/encoding/'>");
            xml.AppendFormat("            <placa xsi:type='xsd:string'>{0}</placa>", placa);
            xml.AppendFormat("            <manifiesto xsi:type='xsd:string'>{0}</manifiesto>", manifiesto);
            xml.AppendFormat("            <Usuario xsi:type='xsd:string'>{0}</Usuario>", Usuario);
            xml.AppendFormat("            <Clave xsi:type='xsd:string'>{0}</Clave>", Clave);
            xml.AppendFormat("            <consolidado xsi:type='xsd:int'>{0}</consolidado>", consolidado);
            xml.Append("        </web:finalizarTrafico>");
            xml.Append("    </soapenv:Body>");
            xml.Append("</soapenv:Envelope>");

            return XDocument.Parse(xml.ToString());
        }

        private XDocument GeneraXMLFollow(
            string placa,
            string manifiesto,
            string mensaje,
            string fechaMensaje,
            string horaMensaje,
            string novedad,
            string estadoActual,
            string Usuario,
            string Clave,
            string Consolidado,
            string Planilla)
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<soapenv:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:web='http://localhost:1001/webservice/'>");
            xml.Append("    <soapenv:Header />");
            xml.Append("    <soapenv:Body>");
            xml.Append("        <web:nuevoseguimiento soapenv:encodingStyle='http://schemas.xmlsoap.org/soap/encoding/'>");
            xml.AppendFormat("            <placa xsi:type='xsd:string'>{0}</placa>", placa);
            xml.AppendFormat("            <manifiesto xsi:type='xsd:string'>{0}</manifiesto>", manifiesto);
            xml.AppendFormat("            <mensaje xsi:type='xsd:string'>{0}</mensaje>", mensaje);
            xml.AppendFormat("            <fechaMensaje xsi:type='xsd:date'>{0}</fechaMensaje>", fechaMensaje);
            xml.AppendFormat("            <horaMensaje xsi:type='xsd:time'>{0}</horaMensaje>", horaMensaje);
            xml.AppendFormat("            <novedad xsi:type='xsd:int'>{0}</novedad>", novedad);
            xml.AppendFormat("            <estadoActual xsi:type='xsd:int'>{0}</estadoActual>", estadoActual);
            xml.AppendFormat("            <Usuario xsi:type='xsd:string'>{0}</Usuario>", Usuario);
            xml.AppendFormat("            <Clave xsi:type='xsd:string'>{0}</Clave>", Clave);
            xml.AppendFormat("            <Consolidado xsi:type='xsd:int'>{0}</Consolidado>", Consolidado);
            xml.AppendFormat("            <Planilla xsi:type='xsd:string'>{0}</Planilla>", Planilla);
            xml.Append("        </web:nuevoseguimiento>");
            xml.Append("    </soapenv:Body>");
            xml.Append("</soapenv:Envelope>");

            return XDocument.Parse(xml.ToString());
        }

    }
}
