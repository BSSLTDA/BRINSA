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
    public class RFCTLKEYRepository : IRFCTLKEYRepository
    {
        private IDbConnection db = new iDB2Connection(ConfigurationManager.ConnectionStrings["ConexionDB2"].ConnectionString);
        public string Add(RFCTLKEY datos)
        {
            string res = "";
            StringBuilder query = new StringBuilder();
            query.Append(" INSERT INTO RFCTLKEY(");
            query.Append(" KCATEG, KLLAVE)");            
            query.Append(" VALUES (");
            query.AppendFormat(" '{0}', '{1}')", datos.KCATEG, datos.KLLAVE);
            
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
