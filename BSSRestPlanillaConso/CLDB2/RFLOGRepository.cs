using CLCommom;
using Dapper;
using IBM.Data.DB2.iSeries;
using System;
using System.Configuration;
using System.Data;
using System.Text;

namespace CLDB2
{
    public class RFLOGRepository : IRFLOGRepository
    {
        private IDbConnection db = new iDB2Connection(ConfigurationManager.ConnectionStrings["ConexionDB2"].ConnectionString);
        public string Add(RFLOG rflog)
        {
            string res;
            StringBuilder query = new StringBuilder();
            query.Append(" INSERT INTO RFLOG");
            query.Append(" (USUARIO, OPERACION, PROGRAMA, EVENTO, TXTSQL, ALERT)");

            query.Append(" VALUES ('REST', '" + rflog.OPERACION + "', 'BSSRestPlanillaCONSO',");
            query.Append(" '" + rflog.EVENTO + "',");
            if (rflog.TXTSQL != null)
            {
                query.Append(" '" + rflog.TXTSQL + "',");
            }else
            {
                query.Append(" '',");
            }            
            query.Append(" " + rflog.ALERT + ")");

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
