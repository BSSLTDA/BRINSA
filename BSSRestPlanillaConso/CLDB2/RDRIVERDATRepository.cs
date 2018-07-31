using CLCommom;
using Dapper;
using IBM.Data.DB2.iSeries;
using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace CLDB2
{
    public class RDRIVERDATRepository : IRDRIVERDATRepository
    {
        private IDbConnection db = new iDB2Connection(ConfigurationManager.ConnectionStrings["ConexionDB2"].ConnectionString);
        public string Add(string CONSO)
        {
            string res;
            StringBuilder query = new StringBuilder();
            query.Append(" INSERT INTO RDRIVERDAT");
            query.Append(" (CCEDULA, CTIPODATO, CCRTUSR, CCONSO)");

            query.Append(" SELECT HCEDUL, 'CELULAR', 'SMS02',");
            query.Append(" '" + CONSO + "'");
            query.Append(" FROM RHCC");
            query.Append(" WHERE HCONSO = '" + CONSO + "'");

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

        public RDRIVERDAT GetInfo(string CONSO)
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT CNUMREG, HCEDUL, HTRAUSR");
            query.Append(" FROM RDRIVERDAT");
            query.Append(" INNER JOIN RHCC ON HCONSO = '" + CONSO + "'");
            query.Append(" AND CCEDULA = HCEDUL");
            query.Append(" WHERE CTIPODATO = 'CELULAR'");            
            return db.Query<RDRIVERDAT>(query.ToString()).SingleOrDefault();
        }
    }
}
