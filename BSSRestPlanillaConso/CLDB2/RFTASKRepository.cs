using ADODB;
using CLCommom;
using Dapper;
using IBM.Data.DB2.iSeries;
using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace CLDB2
{
    public class RFTASKRepository : IRFTASKRepository
    {
        private IDbConnection db = new iDB2Connection(ConfigurationManager.ConnectionStrings["ConexionDB2"].ConnectionString);

        public string Add(RFTASK rftask)
        {
            string res;
            StringBuilder query = new StringBuilder();
            query.Append(" INSERT INTO RFTASK");
            query.Append(" (TCATEG, TSUBCAT, TDESC, TNUMTAREA,");
            query.Append(" TFPROXIMA, TTIPO, TPRM, TKEY, TKEYWORD, TCRTUSR, TPGM)");
            query.Append(" VALUES('RECC', 'SMSSALIDA', '',");
            query.Append(" " + CalcConsec("RFTASKBATCH", 7) + ", NOW(),");
            query.Append(" 'Rutina', '" + rftask.TPRM + "', '" + rftask.TKEY + "',");
            query.Append(" '" + rftask.TKEYWORD + "', '" + rftask.TCRTUSR + "',");
            query.Append(" 'E:\\Despachos_DB2\\rutina.bat')");

            try
            {
                db.Execute(query.ToString());
                res = "OK";
            }
            catch (iDB2SQLErrorException ex)
            {
                res = ex.Message;
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        public string AddAviso(RFTASK rftask)
        {
            string res;
            StringBuilder query = new StringBuilder();
            query.Append(" INSERT INTO RFTASK");
            query.Append(" (TCATEG, TSUBCAT, TDESC, TNUMTAREA,");
            query.Append(" TFPROXIMA, TFRECUENCIA, TTIPO, THLD, TKEY, TKEYWORD,");
            query.Append(" STS1, TCRTUSR, TPGM ) ");
            
            query.Append(" VALUES('AVISODESPACHO', ' ', 'Aviso de Despacho EDI',");
            query.Append(" " + CalcConsec("RFTASKBATCH", 7) + ", NOW(),");
            query.Append(" '0', 'Rutina', '1',");
            query.Append(" '" + rftask.TKEY + "', '" + rftask.TKEYWORD + "',");
            query.Append(" '0', 'API', 'E:\\Despachos_DB\\rutina.bat'");

            try
            {
                db.Execute(query.ToString());
                res = "OK";
            }
            catch (iDB2SQLErrorException ex)
            {
                res = ex.Message;
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        public string AddEndTraffic(RFTASK rftask)
        {
            string res;
            StringBuilder query = new StringBuilder();
            query.Append(" INSERT INTO RFTASK");
            query.Append(" (TAPP, TCATEG, TKEYWORD,");
            query.Append(" TKEY, TTIPO, TPRM, TPGM)");
            query.Append(" VALUES('API', 'FINTRANSITO',");
            query.AppendFormat(" '{0}',", rftask.TKEYWORD);
            query.AppendFormat(" '{0}',", rftask.TKEY);
            query.Append(" 'Rutina',");
            query.AppendFormat(" '|CONSO={0}',", rftask.TPRM);
            query.AppendFormat(" '{0}')", rftask.TPGM);

            try
            {
                db.Execute(query.ToString());
                //res = "OK";
                query.Clear();
                query.Append("SELECT IDENTITY_VAL_LOCAL() FROM SYSIBM.SYSDUMMY1");
                res = db.Query<string>(query.ToString()).Single().ToString();
            }
            catch (iDB2SQLErrorException ex)
            {
                res = "ERROR: " + ex.Message;
            }
            catch (Exception ex)
            {
                res = "ERROR: " + ex.Message;
            }
            return res;
        }

        public String CalcConsec(string tabla, int longitud)
        {
            String consecutivo = "0";
            String consecutivoAct = "0";
            StringBuilder query = new StringBuilder();
            try
            {

                db.Open();
                IDbTransaction idbt = db.BeginTransaction(IsolationLevel.RepeatableRead);

                query.Clear();

                query.Append(" SELECT DIGITS(DEC(DEC(TRIM(CCDESC), " + longitud + ", 0) + 1, " + longitud + ", 0))");
                query.Append(" FROM ZCC");
                query.Append(" WHERE CCTABL = 'SECUENCE' AND CCCODE = '" + tabla + "'");

                IDbCommand cmm = new iDB2Command(query.ToString(), db, idbt);
                IDataReader dr = cmm.ExecuteReader();
                dr.Read();
                consecutivo = dr.GetString(0);
                dr.Close();

                query.Clear();

                if (consecutivo != "0")
                {
                    consecutivoAct = consecutivo;
                    if (consecutivo == "99")
                    {
                        consecutivoAct = "00";
                    }
                    query.Append(" UPDATE ZCC SET");
                    query.Append(" CCDESC = '" + consecutivoAct + "'");
                    query.Append(" WHERE CCTABL = 'SECUENCE' AND CCCODE = '" + tabla + "'");

                    cmm.CommandText = query.ToString();
                    int r = cmm.ExecuteNonQuery();

                }
                idbt.Commit();
                db.Close();
            }
            catch (iDB2SQLErrorException ex)
            {
                db.Close();
                string err = ex.Message;

            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }
            return consecutivo;
        }

        public RFTASK FindTask(string Id)
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT STS1, TMSG, TNUMREG,");
            query.Append(" TJOB400, TJOBD, TJOBQ, TAMBIENTE, TPRM");
            query.Append(" FROM RFTASK");
            query.AppendFormat(" WHERE TNUMREG = '{0}'", Id);
            return db.Query<RFTASK>(query.ToString()).SingleOrDefault();
        }

        /// <summary>
        /// JvaClassPath()
        /// </summary>
        /// <param name="jar"></param>
        /// <param name="Libreria"></param>
        /// <returns></returns>
        public string JvaClassPath(string jar, string Libreria)
        {
            var res = " CLASSPATH('/{LIB}/:/{LIB}/" + jar + ".jar:";

            StringBuilder query = new StringBuilder();
            query.Append(" SELECT CCNOT1");
            query.Append(" FROM RFPARAM");
            query.Append(" WHERE CCTABL = 'LXLONG'");
            query.Append(" AND CCCODE = 'CLASSPATH'");
            query.Append(" ORDER BY CCCODEN");
            var lmRFPARAM = db.Query<RFPARAM>(query.ToString()).ToList();
            if (lmRFPARAM != null)
            {
                if (lmRFPARAM.Count > 0)
                {
                    foreach (var item in lmRFPARAM)
                    {
                        res = res + item.CCNOT1;
                    }
                    res = res + "')";
                }
            }

            return res.Replace("{LIB}", Libreria);
        }

        public string CallPgmJva(RFTASK datos)
        {
            StringBuilder sCmd = new StringBuilder();
            StringBuilder sSbm = new StringBuilder();
            string resultado = "";

            sCmd.Append("RUNJVA CLASS(someteTarea)");
            sCmd.AppendFormat(" PARM( '-lib' '{0}' '-id' '{1}' )", datos.TAMBIENTE, datos.TNUMREG);
            sCmd.AppendFormat(" {0}", JvaClassPath("RJTAREA", datos.TAMBIENTE));

            sSbm.AppendFormat("SBMJOB CMD( {0} )", sCmd.ToString());
            sSbm.AppendFormat(" JOB( {0} )", datos.TJOB400);
            sSbm.AppendFormat(" JOBD( {0} )", datos.TJOBD);
            sSbm.AppendFormat(" JOBQ( {0} )", datos.TJOBQ);

            try
            {                
                resultado = CallPgm(sSbm.ToString());                             
            }
            catch (iDB2SQLErrorException ex)
            {
                resultado = "ERROR: " + ex.Message;
                if (ex.InnerException != null)
                {
                    resultado = resultado + " - " + ex.InnerException;
                }
                if (ex.StackTrace != null)
                {
                    resultado = resultado + " - " + ex.StackTrace;
                }
            }
            catch (iDB2Exception ex)
            {
                resultado = "ERROR: " + ex.Message;
                if (ex.InnerException != null)
                {
                    resultado = resultado + " - " + ex.InnerException;
                }
                if (ex.StackTrace != null)
                {
                    resultado = resultado + " - " + ex.StackTrace;
                }
            }
            catch (Exception ex)
            {
                resultado = "ERROR: " + ex.Message;
                if (ex.InnerException != null)
                {
                    resultado = resultado + " - " + ex.InnerException;
                }
                if (ex.StackTrace != null)
                {
                    resultado = resultado + " - " + ex.StackTrace;
                }
            }
            finally
            {
                if (resultado.Contains("ERROR"))
                {
                    resultado = resultado + " | " + sSbm.ToString();
                    var nRFLOG = new RFLOG()
                    {
                        OPERACION = "SOMETER400",
                        EVENTO = "ERROR",
                        TXTSQL = resultado.Replace("'", "''"),
                        ALERT = "0"
                    };
                    IRFLOGRepository dRFLOG = new RFLOGRepository();
                    resultado = dRFLOG.Add(nRFLOG);
                }
            }
            return resultado;
        }
                
        static string CallPgm(String cmdtext)
        {

            string rc = "OK";            
            var cnn = new Connection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["ConexionDB2ADO"].ConnectionString                
            };
            cnn.Open();

            String pgmParm = "CALL QSYS.QCMDEXC('"
            + cmdtext.Replace("'", "''")
            + "', "
            + cmdtext.Length.ToString("0000000000")
            + ".00000)";

            //pgmParm = "CALL QSYS.QCMDEXC('SBMJOB CMD( RUNJVA CLASS(someteTarea) PARM( ''-lib'' ''DESLX834F'' ''-id'' ''338176'' )  CLASSPATH(''/DESLX834F/:/DESLX834F/RJTAREA.jar:/BSS/lib/commons-cli-1.2.jar:/LXCONNECTOR_192.168.10.1-ERPLX834EC/LxBean.jar:/LXCONNECTOR_192.168.10.1-ERPLX834EC/LXCRuntime.jar:/LXCONNECTOR_192.168.10.1-ERPLX834EC/LXCPI.jar:/LXCONNECTOR_192.168.10.1-ERPLX834EC/lib/jt400.jar:/LXCONNECTOR_192.168.10.1-ERPLX834EC/lib/mailapi.jar:/LXCONNECTOR_192.168.10.1-ERPLX834EC/lib/smtp.jar'') ) JOB( TESORERIA  ) JOBD( ERPLX834EC/BPCS ) JOBQ( QBATCH )', 0000000517.00000)";
            Command cmd = new Command
            {
                ActiveConnection = cnn,
                CommandType = CommandTypeEnum.adCmdText,
                CommandText = pgmParm
            };
            
            try
            {
                object rs;
                cmd.Execute(out rs);                
                cnn.Close();
            }
            catch (iDB2CommErrorException ex)
            {
                rc = "ERROR: " + ex.Message;
                if (ex.InnerException != null)
                {
                    rc = rc + " - " + ex.InnerException;
                }
                if (ex.StackTrace != null)
                {
                    rc = rc + " - " + ex.StackTrace;
                }
            }
            catch (iDB2ExitProgramErrorException ex)
            {
                rc = "ERROR: " + ex.Message;
                if (ex.InnerException != null)
                {
                    rc = rc + " - " + ex.InnerException;
                }
                if (ex.StackTrace != null)
                {
                    rc = rc + " - " + ex.StackTrace;
                }
            }
            catch (iDB2DCFunctionErrorException ex)
            {
                rc = "ERROR: " + ex.Message;
                if (ex.InnerException != null)
                {
                    rc = rc + " - " + ex.InnerException;
                }
                if (ex.StackTrace != null)
                {
                    rc = rc + " - " + ex.StackTrace;
                }
            }
            catch (iDB2Exception ex)
            {
                rc = "ERROR: " + ex.Message;
                if (ex.InnerException != null)
                {
                    rc = rc + " - " + ex.InnerException;
                }
                if (ex.StackTrace != null)
                {
                    rc = rc + " - " + ex.StackTrace;
                }
            }
            catch (Exception ex)
            {
                rc = "ERROR: " + ex.Message;
                if (ex.InnerException != null)
                {
                    rc = rc + " - " + ex.InnerException;
                }
                if (ex.StackTrace != null)
                {
                    rc = rc + " - " + ex.StackTrace;
                }
            }
            finally{
                if (rc.Contains("ERROR"))
                {
                    rc = rc + " | " + cnn.ConnectionString ;
                    rc = rc + " | " + cnn.State;
                    rc = rc + " | " + pgmParm;
                }
            }
            return rc;
        }                
    }
}
