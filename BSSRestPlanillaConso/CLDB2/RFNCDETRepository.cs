using CLCommom;
using Dapper;
using IBM.Data.DB2.iSeries;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace CLDB2
{
    public class RFNCDETRepository : IRFNCDETRepository
    {
        private IDbConnection db = new iDB2Connection(ConfigurationManager.ConnectionStrings["ConexionDB2"].ToString());
        public List<RFNCDET> Find(string Prefijo, string Nota)
        {
            StringBuilder query = new StringBuilder();

            query.Append(" SELECT D.DLINEA, TRIM(D.DCODPRO) DCODPRO, TRIM(D.DDESPRO) DDESPRO,");
            query.Append(" DECIMAL(D.DVALUNI, 14, 4) DVALUNI, DECIMAL(D.DFVALTOT, 14, 4) DFVALTOT,");
            query.Append(" DECIMAL(D.DCANTID, 14, 4) DCANTID, DECIMAL(D.DVALTOT, 14, 4) DVALTOT,");
            query.Append(" IFNULL(TRIM(U.CCCODE), 'NO ENCONTRO ' || D.DUNIMED) AS DUNIMED, TRIM(DFPORIMP) DFPORIMP, DECIMAL(DVALIMP, 14, 4) DVALIMP");
            query.Append(" FROM RFNCDET D");
            query.Append(" LEFT JOIN RZCCL01 U ON U.CCTABL = 'FETABL12' AND U.CCSDSC = D.DUNIMED");
            query.AppendFormat(" WHERE DPREFIJ = '{0}' AND DNOTA = {1}", Prefijo, Nota);
            
            return db.Query<RFNCDET>(query.ToString()).ToList();
        }
    }
}
