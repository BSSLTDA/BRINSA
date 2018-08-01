using BSSRestPlanillaConso.Models;
using CLCommom;
using CLCommon;
using CLDB2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web.Http;
using System.Xml;
using System.Xml.Serialization;

namespace BSSRestPlanillaConso.Controllers
{
    /// <summary>
    /// Factura Electrónica
    /// </summary>
    public class FacturaElectronicaController : ApiController
    {
        IRFLOGRepository dRFLOG = new RFLOGRepository();
        IRFFACDETRepository dRFFACDET = new RFFACDETRepository();
        IRFFACCABRepository dRFFACCAB = new RFFACCABRepository();
        IRCORepository dRCO = new RCORepository();
        IRFPARAMRepository dRFPARAM = new RFPARAMRepository();
        IZCCRepository dZCC = new ZCCRepository();
        IESNRepository dESN = new ESNRepository();
        IRFCTLKEYRepository dRFCTLKEY = new RFCTLKEYRepository();
        IRFTASKRepository dRFTASK = new RFTASKRepository();
        List<RFFACDET> lmRFFACDET = new List<RFFACDET>();
        List<RFPARAM> lmRFPARAM = new List<RFPARAM>();
        List<ZCC> lmZCC = new List<ZCC>();
        List<ESN> lmESN = new List<ESN>();
        RCO mRCO = new RCO();
        RFFACCAB mRFFACCAB = new RFFACCAB();
        RFPARAM mRFPARAM = new RFPARAM();
        private LogFile logProceso = new LogFile();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FFE"></param>
        /// <returns></returns>
        public IHttpActionResult FV(FiltroFE FFE)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string res = "", Cargue = "";
            StringBuilder txt = new StringBuilder();
            try
            {
                txt.Append("{\"ACCION\":\"" + FFE.ACCION + "\",");
                txt.Append(" \"PREFIJO\":\"" + FFE.PREFIJO + "\",");
                txt.Append(" \"FACTURA\":\"" + FFE.FACTURA + "\",");
                txt.Append(" \"ID\":\"" + FFE.ID + "\",");
                txt.Append(" \"USUARIO\":\"" + FFE.USUARIO + "\",");
                txt.Append(" \"PASSWORD\":\"" + FFE.PASSWORD + "\"}");

                var nRFLOG = new RFLOG()
                {
                    OPERACION = "CapturaJson",
                    EVENTO = "FE",
                    TXTSQL = txt.ToString().Replace("'", "''"),
                    ALERT = "0"
                };
                res = dRFLOG.Add(nRFLOG);

                if (res == "OK")
                {
                    switch (FFE.ACCION.ToUpper())
                    {
                        case "CARGAR":
                            res = CargarFV(FFE.PREFIJO, FFE.FACTURA);
                            break;
                        case "ESTADO":
                            res = EstadoFV(FFE.ID, FFE.PREFIJO, FFE.FACTURA);
                            break;
                        case "DESCARGAR":
                            res = DescargarFV(FFE.PREFIJO, FFE.FACTURA);
                            break;
                    }
                }
            }
            catch (WebException ex)
            {
                WebResponse errRsp = ex.Response;
                using (StreamReader rdr = new StreamReader(errRsp.GetResponseStream()))
                {
                    Cargue = "ERROR: " + rdr.ReadToEnd();
                    Console.WriteLine(rdr.ReadToEnd());
                }
                res = "ERROR " + Cargue.Substring((Cargue.IndexOf("<errorMessage>") + 14), (Cargue.IndexOf("</errorMessage>") - Cargue.IndexOf("<errorMessage>")) - 14);
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
                        OPERACION = "FE",
                        EVENTO = "ERROR",
                        TXTSQL = res.Replace("'", "''"),
                        ALERT = "0"
                    };
                    dRFLOG.Add(nRFLOG);
                }
            }

            return Json(new { Resultado = res });
        }

        private string CargarFV(string Prefijo, string Factura)
        {
            string NombreXml = "", res = "", xml = "", xmlResp = "";
            Prefijo = Prefijo ?? "A";
            Factura = Factura ?? "1023711";

            dESN.AddNotas(Prefijo, Factura);

            lmESN = dESN.GetNotas(Prefijo, Factura);
            mRFFACCAB = dRFFACCAB.Find(Prefijo, Factura);
            lmRFFACDET = dRFFACDET.Find(Prefijo, Factura);

            mRCO = dRCO.GetCompany();
            lmRFPARAM = dRFPARAM.GetParams();
            NombreXml = Prefijo + Factura;
            mRFPARAM = lmRFPARAM.Where(m => m.CCCODE == "PREFIJO" && m.CCCODE2 == Prefijo).SingleOrDefault();
            if (mRFPARAM != null)
            {
                if (mRFPARAM.CCCODEN != "0")
                {
                    NombreXml = mRFPARAM.CCALTC + (double.Parse(mRFPARAM.CCCODEN) + double.Parse(Factura));
                }
            }

            FACTURA mFACTURA = new FACTURA
            {
                LMdlENC = CabeceraENC(mRFFACCAB),
                LMdlEMI = CabeceraEMI(mRFFACCAB),
                LMdlADQ = CabeceraADQ(mRFFACCAB),
                LMdlTOT = CabeceraTOT(mRFFACCAB),
                LMdlTIM = CabeceraTIM(mRFFACCAB),
                LMdlTDC = CabeceraTDC(mRFFACCAB),
                LMdlDRF = DetalleDRF(),
                LMdlNOT = DetalleNOT(),
                LMdlORC = CabeceraORC(mRFFACCAB),
                LMdlREF = CabeceraREF(mRFFACCAB),
                LMdlIEN = CabeceraIEN(mRFFACCAB),
                LMdlITE = DetalleITE(mRFFACCAB, lmRFFACDET)
            };

            XmlSerializer xs = new XmlSerializer(typeof(FACTURA));
            using (var sww = new StringWriter())
            {
                using (XmlWriter xwriter = XmlWriter.Create(sww))
                {
                    xs.Serialize(xwriter, mFACTURA);
                    xml = sww.ToString();
                }
            }

            //TextWriter writer = new StreamWriter("C:\\Copia_Servidor\\Proyectos\\BRINSA\\BssFacturaElectronicaService\\" + NombreXml + ".xml");
            TextWriter writer = new StreamWriter("E:\\Sitios\\XMLFE\\" + NombreXml + ".xml");
            xs.Serialize(writer, mFACTURA);
            writer.Close();

            string Cargue = "";
            string url = lmRFPARAM.Where(m => m.CCCODE == "WSDL").SingleOrDefault().CCNOT1 + lmRFPARAM.Where(m => m.CCCODE == "WSDL").SingleOrDefault().CCNOT2;
            string credid = lmRFPARAM.Where(m => m.CCCODE == "WSDLUSER").SingleOrDefault().CCNOT1;
            string credpassword = Sha256(lmRFPARAM.Where(m => m.CCCODE == "WSDLPASS").SingleOrDefault().CCNOT1);

            StringBuilder rawSOAP;
            using (var wb = new WebClient())
            {
                wb.Credentials = new NetworkCredential(credid, credpassword);

                for (var i = 0; i < 4; i++)
                {
                    try
                    {
                        rawSOAP = new StringBuilder();
                        rawSOAP.Append(BuildSoapHeader(credid, credpassword));
                        rawSOAP.Append(@"<soapenv:Body><inv:UploadRequest>");
                        rawSOAP.AppendFormat(@"<fileName>{0}.xml</fileName>", NombreXml);
                        rawSOAP.AppendFormat(@"<fileData>{0}</fileData>", Convert.ToBase64String(Encoding.UTF8.GetBytes(xml)));
                        rawSOAP.AppendFormat(@"<companyId>{0}</companyId>", lmRFPARAM.Where(m => m.CCCODE == "COMPANYID").SingleOrDefault().CCNOT1);
                        rawSOAP.AppendFormat(@"<accountId>{0}</accountId>", lmRFPARAM.Where(m => m.CCCODE == "ACCOUNTID").SingleOrDefault().CCNOT1);
                        rawSOAP.Append(@"</inv:UploadRequest></soapenv:Body></soapenv:Envelope>");
                        Cargue = wb.UploadString(url, "POST", rawSOAP.ToString());
                        Thread.Sleep(5000);
                    }
                    catch (WebException ex)
                    {
                        WebResponse errRsp = ex.Response;
                        using (StreamReader rdr = new StreamReader(errRsp.GetResponseStream()))
                        {
                            Cargue = "ERROR: " + rdr.ReadToEnd();
                            Console.WriteLine(rdr.ReadToEnd());
                        }
                        res = Cargue.Substring((Cargue.IndexOf("<errorMessage>") + 14), (Cargue.IndexOf("</errorMessage>") - Cargue.IndexOf("<errorMessage>")) - 14);
                    }
                    catch (Exception ex)
                    {
                        Cargue = "ERROR: " + ex.ToString();
                        res = Cargue;
                    }
                    if (Cargue.Contains("<transactionId>"))
                    {
                        break;
                    }
                }
                res = dRFFACCAB.UpdFacturaIdNme(Prefijo, Factura, Cargue.Substring((Cargue.IndexOf("<transactionId>") + 15), (Cargue.IndexOf("</transactionId>") - Cargue.IndexOf("<transactionId>")) - 15), NombreXml);
                if (res == "OK")
                {
                    Thread.Sleep(5000);
                    res = EstadoFV(Cargue.Substring((Cargue.IndexOf("<transactionId>") + 15), (Cargue.IndexOf("</transactionId>") - Cargue.IndexOf("<transactionId>")) - 15), Prefijo, Factura);
                    res = DescargarFV(Prefijo, Factura);
                }

                if (Cargue.Contains("ERROR"))
                {
                    var nRFLOG = new RFLOG()
                    {
                        OPERACION = "FE",
                        EVENTO = "ERROR",
                        TXTSQL = Cargue.Replace("'", "''"),
                        ALERT = "0"
                    };
                    dRFLOG.Add(nRFLOG);
                }

                xmlResp = Cargue.Substring((Cargue.IndexOf("<soap:Envelope")), (Cargue.IndexOf("</soap:Envelope>") - Cargue.IndexOf("<soap:Envelope")) + 16);
                //logProceso.CapturaLog(xmlResp, NombreXml + "_resp", "C:\\Copia_Servidor\\Proyectos\\BRINSA\\BssFacturaElectronicaService", "xml");
                logProceso.CapturaLog(xmlResp, NombreXml + "_resp", "E:\\Sitios\\XMLFE", "xml");
            }
            return res;
        }

        private string EstadoFV(string TransId, string Prefijo, string Factura)
        {
            lmRFPARAM = dRFPARAM.GetParams();
            string Estado = "", res = "", xmlResp = "", NombreXml = "";
            string url = lmRFPARAM.Where(m => m.CCCODE == "WSDL").SingleOrDefault().CCNOT1 + lmRFPARAM.Where(m => m.CCCODE == "WSDL").SingleOrDefault().CCNOT2;
            string credid = lmRFPARAM.Where(m => m.CCCODE == "WSDLUSER").SingleOrDefault().CCNOT1;
            string credpassword = Sha256(lmRFPARAM.Where(m => m.CCCODE == "WSDLPASS").SingleOrDefault().CCNOT1);

            NombreXml = Prefijo + Factura;
            mRFPARAM = lmRFPARAM.Where(m => m.CCCODE == "PREFIJO" && m.CCCODE2 == Prefijo).SingleOrDefault();
            if (mRFPARAM != null)
            {
                if (mRFPARAM.CCCODEN != "0")
                {
                    NombreXml = mRFPARAM.CCALTC + (double.Parse(mRFPARAM.CCCODEN) + double.Parse(Factura));
                }
            }

            StringBuilder rawSOAP;
            using (var wb = new WebClient())
            {
                wb.Credentials = new NetworkCredential(credid, credpassword);
                for (var i = 0; i < 4; i++)
                {
                    try
                    {
                        rawSOAP = new StringBuilder();
                        rawSOAP.Append(BuildSoapHeader(credid, credpassword));
                        rawSOAP.Append(@"<soapenv:Body><inv:DocumentStatusRequest>");
                        rawSOAP.AppendFormat(@"<transactionId>{0}</transactionId>", TransId);
                        rawSOAP.AppendFormat(@"<companyId>{0}</companyId>", lmRFPARAM.Where(m => m.CCCODE == "COMPANYID").SingleOrDefault().CCNOT1);
                        rawSOAP.AppendFormat(@"<accountId>{0}</accountId>", lmRFPARAM.Where(m => m.CCCODE == "ACCOUNTID").SingleOrDefault().CCNOT1);
                        rawSOAP.Append(@"</inv:DocumentStatusRequest></soapenv:Body></soapenv:Envelope>");
                        Estado = wb.UploadString(url, "POST", rawSOAP.ToString());
                        res = "OK";
                        Thread.Sleep(5000);
                    }
                    catch (WebException ex)
                    {
                        WebResponse errRsp = ex.Response;
                        using (StreamReader rdr = new StreamReader(errRsp.GetResponseStream()))
                        {
                            Estado = "ERROR: " + rdr.ReadToEnd();
                            Console.WriteLine(rdr.ReadToEnd());
                        }
                        res = Estado.Substring((Estado.IndexOf("<errorMessage>") + 14), (Estado.IndexOf("</errorMessage>") - Estado.IndexOf("<errorMessage>")) - 14);
                    }
                    catch (Exception ex)
                    {
                        Estado = "ERROR: " + ex.ToString();
                        res = Estado;
                    }
                    if (res == "OK")
                    {
                        break;
                    }
                }

                if (Estado.Contains("ERROR"))
                {
                    var nRFLOG = new RFLOG()
                    {
                        OPERACION = "FE",
                        EVENTO = "ERROR",
                        TXTSQL = Estado.Replace("'", "''"),
                        ALERT = "0"
                    };
                    dRFLOG.Add(nRFLOG);
                }

                xmlResp = Estado.Substring((Estado.IndexOf("<soap:Envelope")), (Estado.IndexOf("</soap:Envelope>") - Estado.IndexOf("<soap:Envelope")) + 16);
                //logProceso.CapturaLog(xmlResp, NombreXml + "_resp", "C:\\Copia_Servidor\\Proyectos\\BRINSA\\BssFacturaElectronicaService", "xml");
                logProceso.CapturaLog(xmlResp, NombreXml + "_sts", "E:\\Sitios\\XMLFE", "xml");
            }
            return res;
        }

        private string DescargarFV(string Prefijo, string Factura)
        {
            lmRFPARAM = dRFPARAM.GetParams();
            string NombreXml = "", pref = "", res = "", xmlResp = "";
            string descarga = "";
            string url = lmRFPARAM.Where(m => m.CCCODE == "WSDL").SingleOrDefault().CCNOT1 + lmRFPARAM.Where(m => m.CCCODE == "WSDL").SingleOrDefault().CCNOT2;
            string credid = lmRFPARAM.Where(m => m.CCCODE == "WSDLUSER").SingleOrDefault().CCNOT1;
            string credpassword = Sha256(lmRFPARAM.Where(m => m.CCCODE == "WSDLPASS").SingleOrDefault().CCNOT1);

            Prefijo = Prefijo ?? "A";
            Factura = Factura ?? "1023711";

            NombreXml = Factura;
            pref = Prefijo;
            mRFPARAM = lmRFPARAM.Where(m => m.CCCODE == "PREFIJO" && m.CCCODE2 == Prefijo).SingleOrDefault();
            if (mRFPARAM != null)
            {
                if (mRFPARAM.CCCODEN != "0")
                {
                    pref = mRFPARAM.CCALTC;
                    NombreXml = (double.Parse(mRFPARAM.CCCODEN) + double.Parse(Factura)).ToString();
                }
            }

            StringBuilder rawSOAP;

            using (var wb = new WebClient())
            {
                wb.Credentials = new NetworkCredential(credid, credpassword);

                for (var i = 0; i < 4; i++)
                {
                    try
                    {
                        rawSOAP = new StringBuilder();
                        rawSOAP.Append(BuildSoapHeader(credid, credpassword));
                        rawSOAP.Append(@"<soapenv:Body><inv:DownloadRequest>");
                        rawSOAP.Append(@"<documentType>FV</documentType>");
                        rawSOAP.AppendFormat(@"<documentNumber>{0}</documentNumber>", pref + NombreXml);
                        rawSOAP.AppendFormat(@"<documentPrefix>{0}</documentPrefix>", pref);
                        rawSOAP.Append(@"<resourceType>PDF</resourceType>");
                        rawSOAP.AppendFormat(@"<companyId>{0}</companyId>", lmRFPARAM.Where(m => m.CCCODE == "COMPANYID").SingleOrDefault().CCNOT1);
                        rawSOAP.AppendFormat(@"<accountId>{0}</accountId>", lmRFPARAM.Where(m => m.CCCODE == "ACCOUNTID").SingleOrDefault().CCNOT1);
                        rawSOAP.Append(@"</inv:DownloadRequest></soapenv:Body></soapenv:Envelope>");
                        descarga = wb.UploadString(url, "POST", rawSOAP.ToString());                        
                        var pdfbytes = Convert.FromBase64String(descarga.Substring((descarga.IndexOf("<downloadData>") + 14), (descarga.IndexOf("</downloadData>") - descarga.IndexOf("<downloadData>")) - 14));
                        //File.WriteAllBytes(@"C:\\Copia_Servidor\\Proyectos\\BRINSA\\BssFacturaElectronicaService\" + pref + NombreXml + ".pdf", pdfbytes);
                        File.WriteAllBytes(@"E:\\Despachos_DB2\\ImagenesTransportes\\pdf\\FacturaPDF\\" + pref + NombreXml + ".pdf", pdfbytes);
                        res = "OK";
                        Thread.Sleep(15000);
                    }
                    catch (WebException ex)
                    {
                        WebResponse errRsp = ex.Response;
                        using (StreamReader rdr = new StreamReader(errRsp.GetResponseStream()))
                        {
                            descarga = "ERROR: " + rdr.ReadToEnd();
                            Console.WriteLine(rdr.ReadToEnd());
                        }
                        res = descarga.Substring((descarga.IndexOf("<errorMessage>") + 14), (descarga.IndexOf("</errorMessage>") - descarga.IndexOf("<errorMessage>")) - 14);
                    }
                    catch (Exception ex)
                    {
                        descarga = "ERROR: " + ex.ToString();
                        res = descarga;
                    }
                    if (res == "OK")
                    {
                        break;
                    }
                }

                AvisoDespacho(Prefijo, Factura);

                if (descarga.Contains("ERROR"))
                {
                    var nRFLOG = new RFLOG()
                    {
                        OPERACION = "FE",
                        EVENTO = "ERROR DESCARGA",
                        TXTSQL = descarga.Replace("'", "''"),
                        ALERT = "0"
                    };
                    dRFLOG.Add(nRFLOG);
                }
                xmlResp = descarga.Substring((descarga.IndexOf("<soap:Envelope")), (descarga.IndexOf("</soap:Envelope>") - descarga.IndexOf("<soap:Envelope")) + 16);
                //logProceso.CapturaLog(xmlResp, pref + NombreXml + "_resp", "C:\\Copia_Servidor\\Proyectos\\BRINSA\\BssFacturaElectronicaService", "xml");
                logProceso.CapturaLog(xmlResp, pref + NombreXml + "_rdesc", "E:\\Sitios\\XMLFE", "xml");
                //File.Copy("E:\\Despachos_DB2\\ImagenesTransportes\\pdf\\FacturaPDF2\\" + Prefijo + Factura.PadLeft(8, '0') + "_O.pdf", "E:\\Despachos_DB2\\ImagenesTransportes\\pdf\\FacturaPDF\\" + pref + NombreXml + "_O.pdf", true);

                if (res == "OK")
                {
                    dRFFACCAB.UpdFacturaResPDF(Prefijo, Factura, "1");
                }
                else
                {
                    dRFFACCAB.UpdFacturaResPDF(Prefijo, Factura, "-1");
                }
            }
            return res;
        }

        private void AvisoDespacho(string Prefijo, string Factura)
        {
            bool bGenera = true;
            if (!CtlProcesoUnico("FACTURAS", Prefijo + Factura + ".AVISODESPACHO"))
            {
                return;
            }

            RFFACCAB ADRFFACCAB = dRFFACCAB.GetFlag3(Prefijo, Factura);
            if (ADRFFACCAB != null)
            {
                if (ADRFFACCAB.FAFLAG03 != "0")
                {
                    bGenera = false;
                }
            }

            if (!bGenera)
            {
                return;
            }
            bGenera = false;
            ADRFFACCAB = dRFFACCAB.GetCCNUM(Prefijo, Factura);
            if (ADRFFACCAB != null)
            {
                bGenera = true;
            }
            else
            {
                ADRFFACCAB = dRFFACCAB.GetCCNUMCode(Prefijo, Factura);
                if (ADRFFACCAB != null)
                {
                    bGenera = true;
                }
            }
            if (!bGenera)
            {
                return;
            }
            dRFFACCAB.UpdFlag3(Prefijo, Factura);

            RFTASK mRFTASK = new RFTASK()
            {
                TKEY = Prefijo + Factura,
                TKEYWORD = Prefijo + Factura
            };
            dRFTASK.AddAviso(mRFTASK);
        }

        private bool CtlProcesoUnico(string Categ, string Llave)
        {
            bool continua = true;
            string res = "";
            RFCTLKEY mRFCTLKEY = new RFCTLKEY()
            {
                KCATEG = Categ,
                KLLAVE = Llave
            };

            res = dRFCTLKEY.Add(mRFCTLKEY);
            if (res.Contains("ERROR"))
            {
                continua = false;
            }
            return continua;
        }

        static string Sha256(string randomString)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

        /// <summary>
        /// Genera token Nonce
        /// </summary>
        /// <returns></returns>
        protected string GetNonce()
        {
            string phrase = Guid.NewGuid().ToString();
            return phrase;
        }

        private string BuildSoapHeader(string credid, string credpassword)
        {
            var nonce = GetNonce();
            string nonceToSend = Convert.ToBase64String(Encoding.UTF8.GetBytes(nonce));
            string utc = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            StringBuilder rawSOAP = new StringBuilder();
            rawSOAP.Append(@"<soapenv:Envelope xmlns:inv=""http://invoice.carvajal.com/invoiceService/"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:wsse=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"" xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"">");
            rawSOAP.Append(@"<soapenv:Header>");
            rawSOAP.Append(@"<wsse:Security>");
            rawSOAP.Append(@"<wsse:UsernameToken wsu:Id=""UsernameToken-757E1C333DD11835B515295837182745"">");
            rawSOAP.Append(@"<wsse:Username>" + credid + "</wsse:Username>");
            rawSOAP.Append(@"<wsse:Password Type=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText"">" + credpassword + "</wsse:Password>");
            rawSOAP.Append(@"<wsse:Nonce EncodingType=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary"">" + nonceToSend + "</wsse:Nonce>");
            rawSOAP.Append(@"<wsu:Created>" + utc + "</wsu:Created>");
            rawSOAP.Append(@"</wsse:UsernameToken>");
            rawSOAP.Append(@"</wsse:Security>");
            rawSOAP.Append(@"</soapenv:Header>");
            return rawSOAP.ToString();
        }

        private List<ENC> CabeceraENC(RFFACCAB model)
        {
            ENC mENC = new ENC();
            List<ENC> lmENC = new List<ENC>();
            string NombreXml = "";

            mENC.ENC_1 = "INVOIC";
            mENC.ENC_4 = lmRFPARAM.Where(m => m.CCCODE == "ENC_4").SingleOrDefault().CCDESC;
            mENC.ENC_5 = lmRFPARAM.Where(m => m.CCCODE == "ENC_5").SingleOrDefault().CCDESC;

            NombreXml = model.FPREFIJ + model.FFACTUR;
            mRFPARAM = lmRFPARAM.Where(m => m.CCCODE == "PREFIJO" && m.CCCODE2 == model.FPREFIJ).SingleOrDefault();
            if (mRFPARAM != null)
            {
                if (mRFPARAM.CCCODEN != "0")
                {
                    NombreXml = mRFPARAM.CCALTC + (double.Parse(mRFPARAM.CCCODEN) + double.Parse(model.FFACTUR));
                }
            }
            mENC.ENC_6 = NombreXml;

            mENC.ENC_7 = model.FFECFAC;
            mENC.ENC_8 = model.FACRTDAT.ToString("HH:mm:ss");
            mENC.ENC_9 = (model.FORDCOM.StartsWith("EXP")) ? "2" : "1";
            mENC.ENC_10 = model.FMONEDA;
            mENC.ENC_13 = model.FCLIENT;
            mENC.ENC_15 = model.TotLineas;
            mENC.ENC_16 = model.FFECVEN;

            lmENC.Add(mENC);

            return lmENC;
        }

        private List<EMI> CabeceraEMI(RFFACCAB model)
        {
            EMI mEMI = new EMI();
            List<EMI> lmEMI = new List<EMI>();

            ICC mICC = new ICC();
            List<ICC> lmICC = new List<ICC>();

            TAC mTAC;
            List<TAC> lmTAC = new List<TAC>();

            mEMI.EMI_1 = mRCO.COEXID;
            mEMI.EMI_2 = mRCO.CVATNM;
            mEMI.EMI_3 = mRCO.COAUTN;
            mEMI.EMI_4 = mRCO.COAUTB;
            mEMI.EMI_6 = mRCO.CMPNAM;
            mEMI.EMI_10 = mRCO.CMPAD1;
            mEMI.EMI_11 = mRCO.COSTE;
            mEMI.EMI_12 = mRCO.COADR3;
            mEMI.EMI_13 = mRCO.CMPOST;
            mEMI.EMI_15 = mRCO.COCRCC;
            mEMI.EMI_18 = mRCO.CMPAD1;

            mICC.ICC_1 = mRCO.COCRNO;
            mICC.ICC_3 = mRCO.CMPOST;
            mICC.ICC_5 = mRCO.COSTE;
            mICC.ICC_6 = mRCO.CMPAD1;
            mICC.ICC_7 = mRCO.COCRCC;
            mICC.ICC_8 = mRCO.COADR4;
            lmICC.Add(mICC);
            mEMI.LMdlICC = lmICC;

            lmZCC = dZCC.GetTablesRut();
            foreach (ZCC item in lmZCC)
            {
                mTAC = new TAC()
                {
                    TAC_1 = item.CCCODE
                };
                lmTAC.Add(mTAC);
            }
            mEMI.LMdlTAC = lmTAC;

            lmEMI.Add(mEMI);

            return lmEMI;
        }

        private List<ADQ> CabeceraADQ(RFFACCAB model)
        {
            ADQ mADQ = new ADQ();
            List<ADQ> lmADQ = new List<ADQ>();
            ICR mICR = new ICR();
            List<ICR> lmICR = new List<ICR>();
            CDA mCDA = new CDA();
            List<CDA> lmCDA = new List<CDA>();
            
            mADQ.ADQ_1 = model.SUFD13;
            mADQ.ADQ_2 = (model.SUFD19 != "") ? model.SUFD19 : model.FNIT;
            mADQ.ADQ_3 = model.SUFD17;
            mADQ.ADQ_4 = model.SUFD14;
            mADQ.ADQ_5 = model.FCLIENT;
            mADQ.ADQ_6 = (model.SUFD13 != "2") ? model.FNOMCLI : "";
            mADQ.ADQ_8 = (model.SUFD13 == "2") ? model.SUFD06 : "";
            mADQ.ADQ_9 = (model.SUFD13 == "2") ? model.SUFD05 : "";
            mADQ.ADQ_10 = model.FDIRCLI1;
            mADQ.ADQ_11 = model.FDEPCLI;
            mADQ.ADQ_13 = model.FDIRCLI3;
            mADQ.ADQ_15 = model.FPAICLI;
            mADQ.ADQ_18 = model.FDIRCLI1;

            mICR.ICR_3 = model.FDIRCLI3;
            mICR.ICR_5 = model.FDEPCLI;
            mICR.ICR_6 = model.FDIRCLI1;
            mICR.ICR_7 = model.FPAICLI;
            lmICR.Add(mICR);
            mADQ.LMdlICR = lmICR;

            mCDA.CDA_1 = "1";
            mCDA.CDA_2 = model.CCON;
            mCDA.CDA_3 = model.CPHON;
            mCDA.CDA_4 = model.CMAD6;
            lmCDA.Add(mCDA);
            mADQ.LMdlCDA = lmCDA;

            lmADQ.Add(mADQ);

            return lmADQ;
        }

        private List<TOT> CabeceraTOT(RFFACCAB model)
        {
            TOT mTOT = new TOT();
            List<TOT> lmTOT = new List<TOT>();

            mTOT.TOT_1 = model.FSUBTOT;
            mTOT.TOT_2 = model.FMONEDA;
            mTOT.TOT_3 = model.FSUBTOT;
            mTOT.TOT_4 = model.FMONEDA;
            mTOT.TOT_5 = model.FTOTFAC;
            mTOT.TOT_6 = model.FMONEDA;
            lmTOT.Add(mTOT);

            return lmTOT;
        }

        private List<TIM> CabeceraTIM(RFFACCAB model)
        {
            TIM mTIM = new TIM();
            List<TIM> lmTIM = new List<TIM>();

           
                if (model.FIMPUES != 0)
                {
                    mTIM.TIM_1 = "false";
                    mTIM.TIM_2 = model.FIMPUES;
                    mTIM.TIM_3 = model.FMONEDA;
                    lmTIM.Add(mTIM);
                }               
            

            return lmTIM;
        }

        private List<TDC> CabeceraTDC(RFFACCAB model)
        {
            List<TDC> lmTDC = new List<TDC>();
            TDC mTDC = new TDC
            {
                TDC_1 = model.FMONEDA,
                TDC_2 = model.FMONEDA
            };
            lmTDC.Add(mTDC);

            return lmTDC;
        }

        private List<ORC> CabeceraORC(RFFACCAB model)
        {
            ORC mORC = new ORC();
            List<ORC> lmORC = new List<ORC>();

            mORC.ORC_1 = model.FORDCOM;
            lmORC.Add(mORC);

            return lmORC;
        }

        private List<REF> CabeceraREF(RFFACCAB model)
        {
            REF mREF;
            List<REF> lmREF = new List<REF>();

            mREF = new REF()
            {
                REF_1 = "RF1",
                REF_2 = model.FPEDIDO
            };
            lmREF.Add(mREF);

            mREF = new REF()
            {
                REF_1 = "RF2",
                REF_2 = model.FPLANIL
            };
            lmREF.Add(mREF);

            return lmREF;
        }

        private List<IEN> CabeceraIEN(RFFACCAB model)
        {
            IEN mIEN = new IEN();
            List<IEN> lmIEN = new List<IEN>();

            mIEN.IEN_1 = model.FDIRPEN1;
            mIEN.IEN_2 = model.FDEPPEN;
            mIEN.IEN_4 = model.FDIRPEN3;
            mIEN.IEN_6 = model.FPAIPEN;
            mIEN.IEN_7 = model.FNOMPEN;
            lmIEN.Add(mIEN);

            return lmIEN;
        }

        private List<ITE> DetalleITE(RFFACCAB modelC, List<RFFACDET> modelD)
        {
            ITE mITE = new ITE();
            List<ITE> lmITE = new List<ITE>();

            TII mTII = new TII();
            List<TII> lmTII = new List<TII>();

            IIM mIIM = new IIM();
            List<IIM> lmIIM = new List<IIM>();

            foreach (RFFACDET Det in modelD)
            {
                mITE = new ITE
                {
                    ITE_1 = Det.DLINEA,
                    ITE_2 = (Det.DVALUNI == 0) ? "true" : "false",
                    ITE_3 = Det.DCANTID,
                    ITE_4 = Det.DUNIMED,//
                    ITE_5 = Det.DVALTOT,
                    ITE_6 = modelC.FMONEDA,
                    ITE_7 = Det.DVALUNI,
                    ITE_8 = modelC.FMONEDA,
                    ITE_18 = Det.DCODPRO,
                    ITE_10 = Det.DUNIMED,
                    ITE_11 = Det.DDESPRO,
                    ITE_19 = Det.DVALTOT,
                    ITE_20 = modelC.FMONEDA
                };

                lmIIM.Clear();
                lmTII.Clear();

                mIIM = new IIM()
                {
                    IIM_1 = "01",
                    IIM_2 = (Det.DFVALTOT * decimal.Parse(Det.DFPORIMP)),
                    IIM_3 = modelC.FMONEDA,
                    IIM_4 = Det.DFVALTOT,
                    IIM_5 = modelC.FMONEDA,
                    IIM_6 = Det.DFPORIMP
                };
                lmIIM.Add(mIIM);

                mTII = new TII()
                {
                    TII_1 = (Det.DFVALTOT * decimal.Parse(Det.DFPORIMP)),
                    TII_2 = modelC.FMONEDA,
                    TII_3 = "false",
                    LMdlIIM = lmIIM
                };

                lmTII.Add(mTII);

                mITE.LMdlTII = lmTII;

                lmITE.Add(mITE);
            }

            return lmITE;
        }

        private List<NOT> DetalleNOT()
        {
            List<NOT> lmNOT = new List<NOT>();

            foreach (RFPARAM item in lmRFPARAM.Where(m => m.CCSDSC == "NOTAS" && m.CCCODE == "FV").ToList())
            {
                NOT mNOT = new NOT()
                {
                    NOT_1 = item.CCCODEN + ".-" + item.CCNOTE
                };
                lmNOT.Add(mNOT);
            }
            foreach (ESN item in lmESN)
            {
                NOT mNOT = new NOT()
                {
                    NOT_1 = "3.-" + item.CAT + item.SNSEQ + item.SNDESC
                };
                lmNOT.Add(mNOT);
            }
            return lmNOT;
        }

        private List<DRF> DetalleDRF()
        {
            List<DRF> lmDRF = new List<DRF>();
            DRF mDRF;
            if (mRFPARAM != null)
            {
                mDRF = new DRF()
                {
                    DRF_1 = mRFPARAM.CCCODE3,
                    DRF_2 = mRFPARAM.CCCODE4,
                    DRF_4 = mRFPARAM.CCALTC,
                    DRF_5 = mRFPARAM.CCCODEN,
                    DRF_6 = mRFPARAM.CCCODEN2
                };
                lmDRF.Add(mDRF);
            }

            return lmDRF;
        }

    }
}
