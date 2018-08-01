using CLCommom;
using Dapper;
using IBM.Data.DB2.iSeries;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLDB2
{
    public class RFWRKGRLRepository : IRFWRKGRLRepository
    {
        private IDbConnection db = new iDB2Connection(ConfigurationManager.ConnectionStrings["ConexionDB2"].ConnectionString);

        public string Add(RFWRKGRL rfwrkgrl)
        {
            string res;
            StringBuilder query = new StringBuilder();
            query.Append(" INSERT INTO RFWRKGRL");
            query.Append(" (");
            if (rfwrkgrl.WPLANI != null)
            {
                query.Append(" WPLANI,");
            }
            query.Append(" WDESC, WFTS01, WTXT1)");
            query.Append(" VALUES (");
            if (rfwrkgrl.WPLANI != null)
            {
                query.Append(rfwrkgrl.WPLANI + ",");
            }
            query.Append(" '" + rfwrkgrl.WDESC + "',");
            query.Append(" '" + rfwrkgrl.WFTS01 + "', ' ')");

            try
            {
                db.Execute(query.ToString());
                res = "OK";
                //query.Clear();
                //query.Append("SELECT IDENTITY_VAL_LOCAL() FROM SYSIBM.SYSDUMMY1");
                //var id = db.Query<int>(query.ToString()).Single();
                //rmailx.LID = id;
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
    }
}
