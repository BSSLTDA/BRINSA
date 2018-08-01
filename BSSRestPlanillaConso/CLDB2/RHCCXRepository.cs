using CLCommom;
using Dapper;
using IBM.Data.DB2.iSeries;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace CLDB2
{
    public class RHCCXRepository : IRHCCXRepository
    {
        private IDbConnection db = new iDB2Connection(ConfigurationManager.ConnectionStrings["ConexionDB2"].ConnectionString);

        public RHCCX GetGPS(string conso)
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT");
            query.Append(" IFNULL(XCREF01, 'ND') AS CELULAR,");
            query.Append(" IFNULL(XCGPSPAG, 'ND' ) AS GPSPAG,");
            query.Append(" IFNULL(XCGPSUSR, 'ND' ) AS GPSUSR,");
            query.Append(" IFNULL(XCGPSPASS, 'ND' ) AS GPSPASS");
            query.Append(" FROM RHCCX");
            query.Append(" WHERE XCCONSO = '" + conso + "'");
            return db.Query<RHCCX>(query.ToString()).SingleOrDefault();
        }
    }
}
