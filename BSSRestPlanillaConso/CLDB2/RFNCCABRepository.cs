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
    public class RFNCCABRepository : IRFNCCABRepository
    {
        private IDbConnection db = new iDB2Connection(ConfigurationManager.ConnectionStrings["ConexionDB2"].ToString());
        public RFNCCAB Find(string Prefijo, string Nota)
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT TRIM(H.FPREFIJ) FPREFIJ, H.FNOTA, FPEDIDO,");
            query.Append(" TRIM(H.FMONEDA) FMONEDA, FCLIENT, TRIM(H.FNOMCLI) FNOMCLI, TRIM(H.FDIRCLI1) FDIRCLI1,");
            query.Append(" YEAR(H.FFECNC) || '-' || DIGITS(DECIMAL(MONTH(H.FFECNC), 2, 0)) || '-' || DIGITS(DECIMAL(DAY(H.FFECNC), 2, 0)) AS FFECNC, ");
            query.Append(" YEAR(H.FFECVEN) || '-' || DIGITS(DECIMAL(MONTH(H.FFECVEN), 2, 0)) || '-' || DIGITS(DECIMAL(DAY(H.FFECVEN), 2, 0)) AS FFECVEN, ");
            query.Append(" TRIM(H.FDIRCLI3) FDIRCLI3, FDEPCLI, TRIM(IFNULL(P.CCCODE, 'NO ENCONTRO ' || H.FPAICLI)) FPAICLI, TRIM(H.FNIT) FNIT, TRIM(H.FNOMPEN) FNOMPEN,");
            query.Append(" TRIM(H.FDIRPEN1) FDIRPEN1, TRIM(H.FDIRPEN3) FDIRPEN3, FDEPPEN, TRIM(IFNULL(E.CCCODE, 'NO ENCONTRO ' || H.FPAIPEN)) FPAIPEN,");
            query.Append(" DECIMAL(H.FSUBTOT, 14, 4) FSUBTOT, FIMPUES, DECIMAL(H.FTOTNC, 14, 4) FTOTNC, FACRTDAT,");
            query.Append(" TRIM(C.CPHON) CPHON, TRIM(C.CMAD6) CMAD6, TRIM(C.CCON) CCON,");
            query.AppendFormat(" (SELECT COUNT(*) FROM RFNCDET WHERE DPREFIJ = '{0}' AND DNOTA = {1}) AS TotLineas,", Prefijo, Nota);
            query.Append(" TRIM(IFNULL(A.SUFD05, 'SIN RSU')) SUFD05, TRIM(IFNULL(A.SUFD06, 'SIN RSU')) SUFD06,");
            query.Append(" IFNULL(A.SUFD13, 'SIN RSU') SUFD13, IFNULL(A.SUFD14, 'SIN RSU') SUFD14, TRIM(IFNULL(A.SUFD17, 'SIN RSU')) SUFD17, TRIM(IFNULL(A.SUFD19, 'SIN RSU')) SUFD19");
            query.Append(" FROM RFNCCAB H");
            query.Append(" INNER JOIN RCM C ON H.FCLIENT = C.CCUST");
            query.Append(" LEFT JOIN ZCC P ON H.FPAICLI = P.CCSDSC AND P.CCTABL = 'FETABL01'");
            query.Append(" LEFT JOIN ZCC E ON H.FPAIPEN = E.CCSDSC AND E.CCTABL = 'FETABL01'");
            query.Append(" LEFT JOIN RSUL01 A ON H.FCLIENT = A.SUCUST AND A.SUSEQN = 1");
            query.AppendFormat(" WHERE H.FPREFIJ = '{0}' AND H.FNOTA = {1}", Prefijo, Nota);
            
            return db.Query<RFNCCAB>(query.ToString()).SingleOrDefault();
        }

        public string UpdNotaIdNme(string Prefijo, string Nota, string Id, string Nme)
        {
            string res = "";
            StringBuilder query = new StringBuilder();
            query.Append(" UPDATE RFNCCAB");
            query.Append(" SET");
            query.AppendFormat(" FAREF01 = '{0}',", Id);
            query.AppendFormat(" FAREF02 = '{0}'", Nme);
            query.AppendFormat(" WHERE FPREFIJ = '{0}' AND FNOTA = {1}", Prefijo, Nota);

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

        public string UpdNotaResPDF(string Prefijo, string Nota, string Resultado)
        {
            string res = "";
            StringBuilder query = new StringBuilder();
            query.Append(" UPDATE RFNCCAB");
            query.Append(" SET");
            query.AppendFormat(" FAFLAG04 = '{0}'", Resultado);
            query.AppendFormat(" WHERE FPREFIJ = '{0}' AND FNOTA = {1}", Prefijo, Nota);

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
