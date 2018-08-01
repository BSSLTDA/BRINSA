using CLCommom;
using Dapper;
using IBM.Data.DB2.iSeries;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace CLDB2
{
    public class RHCCRepository : IRHCCRepository
    {
        private IDbConnection db = new iDB2Connection(ConfigurationManager.ConnectionStrings["ConexionDB2"].ConnectionString);
        
        public RHCC GetConsoSalida(RHCC rhcc)
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT HCONSO, HSTS, HPROVE");
            query.Append(" FROM RHCC");
            query.Append(" WHERE HCONSO = '" + rhcc.HCONSO + "'");
            query.Append(" AND HSTS > 30");
            if (rhcc.HPLTA != null)
            {
                query.Append(" AND HPLTA = '" + rhcc.HPLTA + "'");
            }
            return db.Query<RHCC>(query.ToString()).SingleOrDefault();
        }

        public string Update(RHCC rhcc)
        {
            string res;
            StringBuilder query = new StringBuilder();
            query.Append(" UPDATE RHCC");
            query.Append(" SET");
            query.Append(" HESTAD = " + rhcc.HESTAD + ", HFESTAD = NOW(),");
            if (rhcc.HLLEGA != null)
            {
                query.Append(" HLLEGA = '" + rhcc.HLLEGA + "'");
            }
            if (rhcc.HSALE != null)
            {
                query.Append(" HSALE = '" + rhcc.HSALE + "'");
            }
            query.Append(" WHERE HCONSO = '" + rhcc.HCONSO + "' AND HSTS <= " + rhcc.HSTS);

            try
            {
                db.Execute(query.ToString(), rhcc);
                res = "OK";
            }
            catch (iDB2SQLErrorException ex)
            {
                res = ex.Message;
            }
            catch (iDB2Exception ex)
            {
                res = ex.Message;
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        public string UpdateHREPORT(RHCC rhcc)
        {
            string res;
            StringBuilder query = new StringBuilder();
            query.Append(" UPDATE RHCC");
            query.Append(" SET");
            query.Append(" HREPORT = '" + rhcc.RREPORT + "', HFECREP = '" + rhcc.RFECREP + "'");            
            query.Append(" WHERE HCONSO = '" + rhcc.HCONSO + "'");

            try
            {
                db.Execute(query.ToString(), rhcc);
                res = "OK";
            }
            catch (iDB2SQLErrorException ex)
            {
                res = ex.Message;
            }
            catch (iDB2Exception ex)
            {
                res = ex.Message;
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        public string UpdateHESTAD(RHCC rhcc)
        {
            string res;
            StringBuilder query = new StringBuilder();
            query.Append(" UPDATE RHCC");
            query.Append(" SET");
            query.Append(" HESTAD = 6, HFESTAD = '" + rhcc.HFESTAD + "'");
            query.Append(" WHERE HCONSO = '" + rhcc.HCONSO + "'");

            try
            {
                db.Execute(query.ToString(), rhcc);
                res = "OK";
            }
            catch (iDB2SQLErrorException ex)
            {
                res = ex.Message;
            }
            catch (iDB2Exception ex)
            {
                res = ex.Message;
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        public RHCC GetConsoTrafico(string Cons)
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT TRIM(C.HCONSO) HCONSO, TRIM(C.HPLACA) HPLACA,");
            query.Append(" TRIM(C.HCEDUL) HCEDUL, TRIM(C.HCHOFE) HCHOFE, TRIM(C.HCELU) HCELU,");
            query.Append(" TRIM(C.HMANIF) HMANIF, TRIM(O.WMSLOC) AS Origen,");
            query.Append(" TRIM(D.CCIUD) AS Destino, TRIM(D.CTIPO) CTIPO,");
            query.Append(" TRIM(G.XCGPSPAG) XCGPSPAG, TRIM(G.XCGPSUSR) XCGPSUSR,");
            query.Append(" TRIM(G.XCGPSPASS) XCGPSPASS, HSTS");            
            query.Append(" FROM RHCC C");
            query.Append(" INNER JOIN IWM O ON O.LWHS = C.HBOD");
            query.Append(" INNER JOIN RFDANE D ON D.CLXCIUD = C.HCIUDAD AND D.CLXDPTO = C.HDEPTO");
            query.Append(" INNER JOIN RHCCX G ON G.XCCONSO = C.HCONSO");
            query.AppendFormat(" WHERE C.HCONSO = '{0}'", Cons);
            query.Append(" AND D.CTIPO = 'CM'");            
            return db.Query<RHCC>(query.ToString()).SingleOrDefault();
        }

        public List<RHCCEnTransito> GetEnTransito(string hprove)
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT  C.HCONSO Consolidado, C.HPLACA Placa");
            query.Append(" FROM RHCC C");
            query.Append(" INNER JOIN RHCCX X ON C.HCONSO = X.XCCONSO AND IFNULL(X.XCSTS09, 0) = 0");
            query.AppendFormat(" WHERE C.HPROVE = {0}", hprove);
            query.Append(" AND HSTS BETWEEN 50 AND 59");
            query.Append(" ORDER BY C.HCONSO");
            

            //query.Append("             SELECT                          ");
            //query.Append("     HCONSO  AS Consolidado                  ");
            //query.Append("   , HESTAD  AS Estado                       ");
            //query.Append("   , TRIM(CCSDSC)  AS EstadoDescripcion      ");
            //query.Append("   , HPLACA  AS Placa                        ");
            //query.Append("   , TRIM(HREMOLQ) AS Remolque               ");
            //query.Append("   , HPROVE  AS Transp                       ");
            //query.Append("   , TRIM(HNPROVE) AS Transportador          ");
            //query.Append("   , HCEDUL  AS Cedula_Conductor             ");
            //query.Append("   , TRIM(HCHOFE)  AS Conductor              ");
            //query.Append("   , TRIM(HCELU)   AS Celular                ");
            //query.Append("   , TRIM(HMANIF)  AS Manifiesto_del_Transp  ");
            //query.Append("   , TRIM(HREPORT) AS Ultimo_Reporte         ");
            //query.Append("   , HFECREP AS FReporte                     ");
            //query.Append("   , TRIM(HCIUDAD) AS Ciudad                 ");
            //query.Append("   , HDEPTO  AS Depto                        ");
            //query.Append("   , TRIM(HBOD)    AS Bodega                 ");
            //query.Append("   , TRIM(LPOAS)   AS Origen                 ");
            //query.Append("   , HPER    AS Periodo                      ");
            //query.Append("   , HSALE   AS FechaSalida                  ");
            //query.Append("   , HSTS    AS Estado                       ");
            //query.Append("   , DATE(HFECREP)AS FechaReporte            ");
            //query.Append("   , TRIM(HREF06)  AS TipoDeCarga            ");
            //query.Append(" FROM                                        ");
            //query.Append("     (RHCC                                   ");
            //query.Append("     INNER JOIN                              ");
            //query.Append("         IWM                                 ");
            //query.Append("         ON                                  ");
            //query.Append("             RHCC.HBOD = IWM.LWHS)           ");
            //query.Append("     INNER JOIN                              ");
            //query.Append("         VSPESTAD                            ");
            //query.Append("         ON                                  ");
            //query.Append("             RHCC.HSTS = VSPESTAD.VESTAD     ");
            //query.Append(" WHERE                                       ");
            //query.Append("     RHCC.HPROVE IN (" + hprove + ")");
            //query.Append("     AND                                     ");
            //query.Append("     (                                       ");
            //query.Append("         HSTS BETWEEN 50 AND 59              ");            
            //query.Append("     )                                       ");

            return db.Query<RHCCEnTransito>(query.ToString()).ToList();
        }

        public RHCC GetCONSO(string conso)
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT HCONSO, HSTS");
            query.Append(" FROM RHCC");
            query.Append(" WHERE HCONSO = '" + conso + "'");
            return db.Query<RHCC>(query.ToString()).SingleOrDefault();
        }
    }
}
