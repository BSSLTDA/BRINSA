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
    public class RCAURepository : IRCAURepository
    {
        private IDbConnection db = new iDB2Connection(ConfigurationManager.ConnectionStrings["ConexionDB2"].ConnectionString);

        public RCAU DatosUSR(string user, string pass)
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT UQRY");
            query.Append(" FROM RCAU");
            query.Append(" WHERE UUSR = '" + user + "' AND UPASS = '" + pass + "'");
            return db.Query<RCAU>(query.ToString()).SingleOrDefault();
        }

        public string ExisteUSR(string user, string pass)
        {
            string uusr;
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT UUSR");
            query.Append(" FROM RCAU");
            query.Append(" WHERE UUSR = '" + user + "' AND UPASS = '" + pass + "'");
            try
            {
                uusr = db.Query<string>(query.ToString()).SingleOrDefault();
                if (uusr != null)
                {
                    return "OK";
                }
                else
                {
                    return "No existe Usuario o clave incorrecta";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
