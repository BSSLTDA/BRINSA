using BSSRestPlanillaConso.Models;
using CLCommom;
using CLDB2;
using System;
using System.Diagnostics;
using System.Text;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace BSSRestPlanillaConso.Controllers
{
    public class ServiciosController : ApiController
    {
        IRCAURepository dRCAU = new RCAURepository();
        IRFLOGRepository dRFLOG = new RFLOGRepository();
        IRFPARAMRepository dRFPARAM = new RFPARAMRepository();
        IRFTASKRepository dRFTASK = new RFTASKRepository();
        RFTASK mRFTASK = new RFTASK();
        /// <summary>
        /// API central de Servicios
        /// </summary>        
        /// <param name="FS">       
        /// </param>
        /// <returns></returns>
        public IHttpActionResult Rutinas(FiltroServicios FS)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var res = "";
            StringBuilder txt = new StringBuilder();
            try
            {
                txt.Append("{\"ACCION\":\"" + FS.ACCION + "\",");
                txt.Append(" \"USUARIO\":\"" + FS.USUARIO + "\",");
                txt.Append(" \"PASSWORD\":\"" + FS.PASSWORD + "\",");
                txt.Append(" \"ESPERARRESPUESTA\":\"" + FS.ESPERARRESPUESTA + "\",");
                txt.Append(" \"IDTAREA\":\"" + FS.IDTAREA + "\",");
                txt.Append(" \"RESULTADO\":{");
                txt.Append(" \"TCATEG\":\"" + FS.RESULTADO?.TCATEG + "\",");
                txt.Append(" \"TSUBCAT\":\"" + FS.RESULTADO?.TSUBCAT + "\",");
                txt.Append(" \"TKEY\":\"" + FS.RESULTADO?.TKEY + "\",");
                txt.Append(" \"TKEYWORD\":\"" + FS.RESULTADO?.TKEYWORD + "\",");
                txt.Append(" \"TAPIRESULT\":\"" + FS.RESULTADO?.TAPIRESULT + "\"}}");
                
                var nRFLOG = new RFLOG()
                {
                    OPERACION = "CapturaJson",
                    EVENTO = "Servicios",
                    TXTSQL = txt.ToString().Replace("'", "''"),
                    ALERT = "0"
                };
                res = dRFLOG.Add(nRFLOG);

                res = dRCAU.ExisteUSR(FS.USUARIO, FS.PASSWORD);
                if (res == "OK")
                {
                    switch (FS.ACCION.ToUpper())
                    {
                        case "RUTINA":
                            var resul = Rutina(FS.IDTAREA, FS.ESPERARRESPUESTA);
                            return Json(resul);
                        case "SOMETERTRABAJO400":
                            res = Someter(FS.IDTAREA, FS.RESULTADO);
                            break;
                        case "RUTINA2.0":
                            var result = Rutina2_0(FS.IDTAREA, FS.ESPERARRESPUESTA);
                            return Json(result);                        
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
                        OPERACION = "SERVICIO",
                        EVENTO = "ERROR",
                        TXTSQL = res.Replace("'", "''"),
                        ALERT = "0"
                    };
                    res = dRFLOG.Add(nRFLOG);
                }
            }
            return Json(new { Resultado = res });
        }

        private RespuestaRutina Rutina(string Id, string Espera)
        {
            RespuestaRutina res = new RespuestaRutina();
            string exe = "", parameters = "";
            var rutaEXE = dRFPARAM.GetDirPDF("LXLONG", "RUTINAS_TM");

            exe = rutaEXE.CCNOT1.Trim();
            parameters = string.Format("{0}", Id);
            var process = Process.Start(exe, parameters);
            if (Espera != null)
            {
                if (Espera.ToUpper() == "SI")
                {
                    process.WaitForExit();
                    if (process.HasExited)
                    {
                        mRFTASK = dRFTASK.FindTask(Id);
                        if (mRFTASK != null)
                        {
                            res = new RespuestaRutina()
                            {
                                Estado = int.Parse(mRFTASK.STS1),
                                Mensaje = mRFTASK.TMSG
                            };
                        }
                        else
                        {
                            res = new RespuestaRutina()
                            {
                                Estado = 0,
                                Mensaje = "Sin Respuesta"
                            };
                        }
                    }
                }
                else
                {
                    res = new RespuestaRutina()
                    {
                        Estado = 0,
                        Mensaje = "OK"
                    };
                }
            }
            else
            {
                res = new RespuestaRutina()
                {
                    Estado = 0,
                    Mensaje = "OK"
                };
            }
            return res;
        }

        private RespuestaRutina Rutina2_0(string Id, string Espera)
        {
            RespuestaRutina res = new RespuestaRutina();
            string exe = "", parameters = "";
            var rutaEXE = dRFPARAM.GetDirPDF("LXLONG", "RUTINAS2.0");

            mRFTASK = dRFTASK.FindTask(Id);

            exe = rutaEXE.CCNOT1.Trim();
            parameters = string.Format("{0} IDTAREA={1}", mRFTASK.TPRM.Replace("|", " ").Trim(), Id);
            var process = Process.Start(exe, parameters);
            if (Espera != null)
            {
                if (Espera.ToUpper() == "SI")
                {
                    process.WaitForExit();
                    if (process.HasExited)
                    {
                        mRFTASK = dRFTASK.FindTask(Id);
                        if (mRFTASK != null)
                        {
                            res = new RespuestaRutina()
                            {
                                Estado = int.Parse(mRFTASK.STS1),
                                Mensaje = mRFTASK.TMSG
                            };
                        }
                        else
                        {
                            res = new RespuestaRutina()
                            {
                                Estado = 0,
                                Mensaje = "Sin Respuesta"
                            };
                        }
                    }
                }
                else
                {
                    res = new RespuestaRutina()
                    {
                        Estado = 0,
                        Mensaje = "OK"
                    };
                }
            }
            else
            {
                res = new RespuestaRutina()
                {
                    Estado = 0,
                    Mensaje = "OK"
                };
            }

            return res;
        }

        private string Someter(string Id, RespuestaSometer400 Resultado)
        {
            string res = "";
            if (Resultado == null)
            {
                mRFTASK = dRFTASK.FindTask(Id);
                if (mRFTASK != null)
                {
                    res = dRFTASK.CallPgmJva(mRFTASK);
                }
            }
            else
            {
                //var javaScriptSer = new JavaScriptSerializer();
                //var value = (RespuestaSometer400)javaScriptSer.Deserialize(Resultado, typeof(RespuestaSometer400));

                System.Web.HttpContext.Current.Application["APIBRINSA_" + Id] = Resultado;

                //RespuestaSometer400 s = (RespuestaSometer400)System.Web.HttpContext.Current.Application["APIBRINSA_" + Id];

                
            }
            

            return res;
        }
    }
}
