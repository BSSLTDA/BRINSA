using CLCommon;
using Dapper;
using IBM.Data.DB2.iSeries;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace CLDB2
{
    public class RCORepository : IRCORepository
    {
        private IDbConnection db = new iDB2Connection(ConfigurationManager.ConnectionStrings["ConexionDB2"].ToString());

        public RCO GetCompany()
        {
            StringBuilder query = new StringBuilder();            
            query.Append("SELECT TRIM(COEXID) COEXID, TRIM(CVATNM) CVATNM, TRIM(COAUTN) COAUTN, TRIM(COAUTB) COAUTB,");
            query.Append(" TRIM(CMPNAM) CMPNAM, TRIM(CMPAD1) CMPAD1, COSTE, TRIM(COADR3) COADR3, TRIM(CMPOST) CMPOST,");
            query.Append(" TRIM(COCRCC) COCRCC, TRIM(COCRNO) COCRNO, TRIM(COADR4) COADR4");
            query.Append(" FROM RCO");
            query.Append(" WHERE CMPNY = '10'");

            return db.Query<RCO>(query.ToString()).SingleOrDefault();
        }
    }
}
