using CLCommon;
using Dapper;
using IBM.Data.DB2.iSeries;
using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace CLDB2
{
    public class RFFACCABRepository : IRFFACCABRepository
    {
        private IDbConnection db = new iDB2Connection(ConfigurationManager.ConnectionStrings["ConexionDB2"].ToString());

        public RFFACCAB Find(string Prefijo, string Factura)
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT TRIM(H.FPREFIJ) FPREFIJ, H.FFACTUR, TRIM(H.FORDCOM) FORDCOM, TRIM(H.FMONEDA) FMONEDA,");
            query.Append(" YEAR(H.FFECFAC) || '-' || DIGITS(DECIMAL(MONTH(H.FFECFAC), 2, 0)) || '-' || DIGITS(DECIMAL(DAY(H.FFECFAC), 2, 0)) AS FFECFAC, ");
            query.Append(" YEAR(H.FFECVEN) || '-' || DIGITS(DECIMAL(MONTH(H.FFECVEN), 2, 0)) || '-' || DIGITS(DECIMAL(DAY(H.FFECVEN), 2, 0)) AS FFECVEN, ");
            query.Append(" H.FPEDIDO, H.FPLANIL, H.FCONSOL, H.FBODEGA, H.FTERMIN, H.FCLIENT, TRIM(H.FNOMCLI) FNOMCLI,");
            query.Append(" TRIM(C.CPHON) CPHON, TRIM(C.CMAD6) CMAD6, TRIM(C.CCON) CCON,");
            query.Append(" TRIM(H.FDIRCLI1) FDIRCLI1, TRIM(H.FDIRCLI3) FDIRCLI3, H.FDEPCLI, TRIM(IFNULL(P.CCCODE, 'NO ENCONTRO ' || H.FPAICLI)) FPAICLI, TRIM(H.FNIT) FNIT, TRIM(H.FNOMPEN) FNOMPEN,");
            query.Append(" TRIM(H.FDIRPEN1) FDIRPEN1, TRIM(H.FDIRPEN3) FDIRPEN3, H.FDEPPEN, TRIM(IFNULL(E.CCCODE, 'NO ENCONTRO ' || H.FPAIPEN)) FPAIPEN, DECIMAL(H.FSUBTOT, 14, 4) FSUBTOT, DECIMAL(H.FTOTFAC, 14, 4) FTOTFAC, H.FACRTDAT,");
            query.AppendFormat(" (SELECT COUNT(*) FROM RFFACDET WHERE DPREFIJ = '{0}' AND DFACTUR = {1}) AS TotLineas,", Prefijo, Factura);
            query.Append(" TRIM(C.CTAX) CTAX, DECIMAL(H.FIMPUES,14, 4) FIMPUES,");
            query.Append(" TRIM(IFNULL(A.SUFD05, 'SIN RSU')) SUFD05, TRIM(IFNULL(A.SUFD06, 'SIN RSU')) SUFD06, IFNULL(A.SUFD13, 'SIN RSU') SUFD13, IFNULL(A.SUFD14, 'SIN RSU') SUFD14, TRIM(IFNULL(A.SUFD17, 'SIN RSU')) SUFD17, TRIM(IFNULL(A.SUFD19, 'SIN RSU')) SUFD19");
            query.Append(" FROM RFFACCAB H");
            query.Append(" INNER JOIN RCM C ON H.FCLIENT = C.CCUST");
            query.Append(" LEFT JOIN ZCC P ON H.FPAICLI = P.CCSDSC AND P.CCTABL = 'FETABL01'");
            query.Append(" LEFT JOIN ZCC E ON H.FPAIPEN = E.CCSDSC AND E.CCTABL = 'FETABL01'");
            query.Append(" LEFT JOIN RSUL01 A ON H.FCLIENT = A.SUCUST AND A.SUSEQN = 1");
            query.AppendFormat(" WHERE H.FPREFIJ = '{0}' AND H.FFACTUR = {1}", Prefijo, Factura);

            return db.Query<RFFACCAB>(query.ToString()).SingleOrDefault();
        }

        public RFFACCAB GetCCNUM(string Prefijo, string Factura)
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT CCNUM");
            query.Append(" FROM RFFACCAB");
            query.Append(" INNER JOIN RFPARAM ON CCTABL = 'EDIAVISODESP' AND CCCODEN = FCLIENT");
            query.Append(" AND CCCODEN2 = FPTOEN");
            query.AppendFormat(" WHERE FPREFIJ = '{0}' AND FFACTUR = {1}", Prefijo, Factura);

            return db.Query<RFFACCAB>(query.ToString()).SingleOrDefault();
        }

        public RFFACCAB GetCCNUMCode(string Prefijo, string Factura)
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT CCNUM");
            query.Append(" FROM RFFACCAB");
            query.Append(" INNER JOIN RFPARAM ON CCTABL = 'EDIAVISODESP' AND CCCODEN = FCLIENT");
            query.AppendFormat(" WHERE FPREFIJ = '{0}' AND FFACTUR = {1}", Prefijo, Factura);
            query.Append(" AND CCCODEN2 = 0");

            return db.Query<RFFACCAB>(query.ToString()).SingleOrDefault();
        }

        public RFFACCAB GetFlag3(string Prefijo, string Factura)
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT FAFLAG03 ");
            query.Append(" FROM RFFACCAB");            
            query.AppendFormat(" WHERE FPREFIJ = '{0}' AND FFACTUR = {1}", Prefijo, Factura);

            return db.Query<RFFACCAB>(query.ToString()).SingleOrDefault();
        }

        public string UpdFacturaIdNme(string Prefijo, string Factura, string Id, string Nme)
        {
            string res = "";            
            StringBuilder query = new StringBuilder();
            query.Append(" UPDATE RFFACCAB");
            query.Append(" SET");
            query.AppendFormat(" FAREF01 = '{0}',", Id);
            query.AppendFormat(" FAREF02 = '{0}'", Nme);
            query.AppendFormat(" WHERE FPREFIJ = '{0}' AND FFACTUR = {1}", Prefijo, Factura);

            try
            {
                db.Execute(query.ToString());
                res = "OK";
            }
            catch (iDB2SQLErrorException ex)
            {
                res = "ERROR: " + ex.ToString() + " " + query.ToString();
            }
            catch (iDB2Exception ex)
            {
                res = "ERROR: " + ex.ToString() + " " + query.ToString();
            }
            catch (Exception ex)
            {
                res = "ERROR: " + ex.ToString() + " " + query.ToString();
            }
            return res;            
        }

        public string UpdFacturaResPDF(string Prefijo, string Factura, string Resultado)
        {
            string res = "";
            StringBuilder query = new StringBuilder();
            query.Append(" UPDATE RFFACCAB");
            query.Append(" SET");
            query.AppendFormat(" FAFLAG04 = '{0}'", Resultado);
            query.AppendFormat(" WHERE FPREFIJ = '{0}' AND FFACTUR = {1}", Prefijo, Factura);

            try
            {
                db.Execute(query.ToString());
                res = "OK";
            }
            catch (iDB2SQLErrorException ex)
            {
                res = "ERROR: " + ex.ToString() + " " + query.ToString();
            }
            catch (iDB2Exception ex)
            {
                res = "ERROR: " + ex.ToString() + " " + query.ToString();
            }
            catch (Exception ex)
            {
                res = "ERROR: " + ex.ToString() + " " + query.ToString();
            }
            return res;
        }

        public string UpdFlag3(string Prefijo, string Factura)
        {
            string res = "";
            StringBuilder query = new StringBuilder();
            query.Append(" UPDATE RFFACCAB");
            query.Append(" SET");
            query.Append(" FAFLAG03 = 1");
            query.AppendFormat(" WHERE FPREFIJ = '{0}' AND FFACTUR = {1}", Prefijo, Factura);

            try
            {
                db.Execute(query.ToString());
                res = "OK";
            }
            catch (iDB2SQLErrorException ex)
            {
                res = "ERROR: " + ex.ToString() + " " + query.ToString();
            }
            catch (iDB2Exception ex)
            {
                res = "ERROR: " + ex.ToString() + " " + query.ToString();
            }
            catch (Exception ex)
            {
                res = "ERROR: " + ex.ToString() + " " + query.ToString();
            }
            return res;
        }
    }
}
