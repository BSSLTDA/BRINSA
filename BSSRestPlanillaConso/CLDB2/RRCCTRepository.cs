using CLCommom;
using Dapper;
using IBM.Data.DB2.iSeries;
using System;
using System.Configuration;
using System.Data;
using System.Text;

namespace CLDB2
{
    public class RRCCTRepository : IRRCCTRepository
    {
        private IDbConnection db = new iDB2Connection(ConfigurationManager.ConnectionStrings["ConexionDB2"].ConnectionString);
        public string Update(RRCCT rrcct)
        {
            string res;
            StringBuilder query = new StringBuilder();
            query.Append(" UPDATE RRCCT");
            query.Append(" SET");
            if (rrcct.RFEC17 != null)
            {
            query.Append(" RFEC17 = '" + rrcct.RFEC17 + "'");
            }
            if (rrcct.RFEC50 != null)
            {
                query.Append(" RFEC50 = '" + rrcct.RFEC50 + "'");
            }            
            query.Append(" WHERE RCONSO = '" + rrcct.RCONSO + "'");

            try
            {
                db.Execute(query.ToString(), rrcct);
                res = "OK";
            }
            catch (iDB2SQLErrorException ex)
            {
                res = ex.Message;
            }
            catch (iDB2Exception ex)
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
