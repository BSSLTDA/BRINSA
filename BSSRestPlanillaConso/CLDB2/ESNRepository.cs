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
    public class ESNRepository : IESNRepository
    {
        private IDbConnection db = new iDB2Connection(ConfigurationManager.ConnectionStrings["ConexionDB2"].ToString());

        public string AddNotas(string Prefijo, string Factura)
        {
            string res = "";
            StringBuilder query = new StringBuilder();
            query.Append(" INSERT INTO ESNL01(");
            query.Append(" SNID, SNTYPE, SNCUST, SNSEQ, SNDESC, SNPRT");
            query.Append(" SNPIC, SNINV, SNSTMT, SNENDT, SNENTM, SNENUS)");
            query.Append(" SELECT DISTINCT 'SN', 'O', DPEDIDO, CCCODEN, CCDESC,");
            query.Append(" 'N', 'N', 'Y', 'N', ( YEAR( NOW() ) * 10000 + MONTH( NOW()  ) * 100 + DAY( NOW() ) ),");
            query.Append(" ( HOUR( NOW() ) * 10000 + MINUTE( NOW() ) * 100 + SECOND( NOW() ) ), 'SISTEMA'");
            query.Append(" FROM RFFACDET F");
            query.Append(" INNER JOIN RIIMC P ON F.DCODPRO = P.RCPROD");
            query.Append(" LEFT JOIN ESNL01 N ON F.DPEDIDO = N.SNCUST AND N.SNTYPE = 'O',");
            query.Append(" RFPARAM L");
            query.Append(" WHERE P.RCDIVI = 'QUIMICOS' AND");
            query.Append(" P.RCLINE = 'CLORO' AND");
            query.Append(" P.RCSEGM IN ('CONTENEDOR', 'CILINDRO') AND");
            query.AppendFormat(" F.DPREFIJ = '{0}' AND", Prefijo);
            query.AppendFormat(" F.DFACTUR = {0} AND", Factura);
            query.Append(" L.CCTABL = 'FACCILINDROS' AND");
            query.Append(" L.CCCODE = 'NOTA' AND N.SNCUST IS NULL");

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

        public List<ESN> GetNotas(string Prefijo, string Factura)
        {
            StringBuilder query = new StringBuilder();

            query.Append(" SELECT DEC( 1, 1, 0) AS CAT, TRIM(SNSEQ) SNSEQ, TRIM(SNDESC) SNDESC");
            query.Append(" FROM ESNL01");
            query.Append(" INNER JOIN SIL ON SNTYPE = 'L' AND SNCUST = ILORD AND SnSHIP = ILSEQ");
            query.AppendFormat(" WHERE ILDPFX = '{0}' AND ILDOCN = {1}", Prefijo, Factura);
            query.Append(" AND SNINV = 'Y'");

            query.Append(" UNION ");

            query.Append(" SELECT DEC( 2, 1, 0) AS CAT, TRIM(SNSEQ) SNSEQ, TRIM(SNDESC) SNDESC");
            query.Append(" FROM ESNL01");
            query.Append(" INNER JOIN SIH ON SNTYPE = 'O' AND SNCUST = SIORD AND SNSHIP = 0");
            query.AppendFormat(" WHERE IHDPFX = '{0}' AND IHDOCN = {1}", Prefijo, Factura);
            query.Append(" AND SNINV = 'Y'");

            query.Append(" UNION ");

            query.Append(" SELECT DEC( 3, 1, 0) AS CAT, TRIM(SNSEQ) SNSEQ, TRIM(SNDESC) SNDESC");
            query.Append(" FROM ESNL01");
            query.Append(" INNER JOIN RFFACCAB ON SNTYPE = 'C' AND SNCUST = FCLIENT AND SNSHIP = 0");
            query.AppendFormat(" WHERE FPREFIJ = '{0}' AND FFACTUR = {1}", Prefijo, Factura);
            query.Append(" AND SNINV = 'Y'");

            query.Append(" UNION ");

            query.Append(" SELECT DEC( 4, 1, 0) AS CAT, TRIM(SNSEQ) SNSEQ, TRIM(SNDESC) SNDESC");
            query.Append(" FROM ESNL01");
            query.Append(" INNER JOIN RFFACCAB ON SNTYPE = 'C' AND SNCUST = FCLIENT AND SNSHIP = FPTOEN");
            query.AppendFormat(" WHERE FPREFIJ = '{0}' AND FFACTUR = {1}", Prefijo, Factura);
            query.Append(" AND SNINV = 'Y'");

            return db.Query<ESN>(query.ToString()).ToList();
        }
    }
}
