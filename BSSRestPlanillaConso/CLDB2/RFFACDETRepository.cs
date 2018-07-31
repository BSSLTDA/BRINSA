using CLCommon;
using Dapper;
using IBM.Data.DB2.iSeries;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace CLDB2
{
    public class RFFACDETRepository : IRFFACDETRepository
    {
        private IDbConnection db = new iDB2Connection(ConfigurationManager.ConnectionStrings["ConexionDB2"].ToString());

        public List<RFFACDET> Find(string Prefijo, string Factura)
        {
            StringBuilder query = new StringBuilder();               
            query.Append(" SELECT D.DLINEA, DECIMAL(D.DVALUNI, 14, 4) DVALUNI, DECIMAL(D.DCANTID, 14, 4) DCANTID, DECIMAL(D.DVALTOT, 14, 4) DVALTOT,");
            query.Append(" TRIM(D.DCODPRO) DCODPRO, TRIM(D.DDESPRO) DDESPRO, CASE WHEN TRIM(DFPORIMP) = '' THEN '0' ELSE TRIM(DFPORIMP) END AS DFPORIMP, DECIMAL(D.DFVALTOT, 14, 4) DFVALTOT,");
            query.Append(" IFNULL(TRIM(U.CCCODE), 'NO ENCONTRO ' || D.DUNIMED) AS DUNIMED");
            query.Append(" FROM RFFACDET D");
            query.Append(" LEFT JOIN RZCCL01 U ON U.CCTABL = 'FETABL12' AND U.CCSDSC = D.DUNIMED");
            query.AppendFormat(" WHERE DPREFIJ = '{0}' AND DFACTUR = {1}", Prefijo, Factura);

            return db.Query<RFFACDET>(query.ToString()).ToList();
        }
    }
}
