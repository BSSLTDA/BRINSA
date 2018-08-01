using CLCommon;
using Dapper;
using IBM.Data.DB2.iSeries;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace CLDB2
{
    public class ZCCRepository : IZCCRepository
    {
        private IDbConnection db = new iDB2Connection(ConfigurationManager.ConnectionStrings["ConexionDB2"].ToString());
        public List<ZCC> GetTablesRut()
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT TRIM(CCCODE) CCCODE");
            query.Append(" FROM ZCC");            
            query.Append(" WHERE CCTABL IN ('FERUT53', 'FERUT54')");

            return db.Query<ZCC>(query.ToString()).ToList();
        }
    }
}
