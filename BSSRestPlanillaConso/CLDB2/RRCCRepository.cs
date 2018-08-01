using CLCommom;
using Dapper;
using IBM.Data.DB2.iSeries;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace CLDB2
{
    public class RRCCRepository : IRRCCRepository
    {
        private IDbConnection db = new iDB2Connection(ConfigurationManager.ConnectionStrings["ConexionDB2"].ConnectionString);
        public string Add(RRCC rrcc)
        {
            string res;
            StringBuilder query = new StringBuilder();
            query.Append(" INSERT INTO RRCC");
            query.Append(" (RTREP, RCONSO, RSTSC, RREPORT,");
            query.Append(" RCRTUSR, RCRTDAT, RFECREP, RAPP)");

            query.Append(" SELECT 4, HCONSO, HSTS, '" + rrcc.RREPORT + "',");
            query.Append(" 'SMS02', '" + rrcc.RCRTDAT + "', '" + rrcc.RCRTDAT + "',");
            query.Append(" '" + rrcc.RAPP + "'");
            query.Append(" FROM RHCC");
            query.Append(" WHERE HCONSO = '" + rrcc.HCONSO + "'");

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

        public string AddRPLANI(RRCC rrcc)
        {
            string res;
            StringBuilder query = new StringBuilder();
            query.Append(" INSERT INTO RRCC");
            query.Append(" (RTREP, RPLANI, RCONSO, RSTSC, RSTSE,");
            query.Append(" RFECREP, RREPORT, RCRTUSR, RAPP)");

            query.Append(" VALUES (30, " + rrcc.RPLANI + ", '" + rrcc.HCONSO + "', " + rrcc.RSTSC + ",");
            query.Append(" 50, '" + rrcc.RFECREP + "', '" + rrcc.RREPORT + "',");
            query.Append(" '" + rrcc.RCRTUSR + "', 'SMS')");

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

        public string AddERRPLA(RRCC rrcc)
        {
            string res;
            StringBuilder query = new StringBuilder();
            query.Append(" INSERT INTO RRCC");
            query.Append(" (RTREP, RCONSO, RSTSE, RFECREP, RREPORT,");
            query.Append(" RCRTUSR, RAPP)");

            query.Append(" VALUES (30, 'ERRPLA', 50, '" + rrcc.RFECREP + "',");
            query.Append(" '" + rrcc.RREPORT + "',");
            query.Append(" '" + rrcc.RCRTUSR + "', 'SMS')");

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

        public List<RRCC> FindConso(string Conso, string rapp)
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT RCONSO");
            query.Append(" FROM RRCC");            
            query.Append(" WHERE RCONSO = '" + Conso + "' AND RAPP = '" + rapp + "'");            
            return db.Query<RRCC>(query.ToString()).ToList();
        }

        public string AddReporte(RRCC rrcc)
        {

            string res;
            StringBuilder query = new StringBuilder();
            query.Append(" INSERT INTO RRCC");
            query.Append(" (RTREP, RCONSO, RPLANI, RPLACA, RFECREP,");
            query.Append(" RUBIC, RTIPORI, RTIPNOV, RREPORT,");
            query.Append(" RCRTUSR, RAPP, RWINDAT)");
            query.AppendFormat(" VALUES (45, '{0}',", rrcc.RCONSO);
            query.AppendFormat(" '{0}', '{1}',", rrcc.RPLANI, rrcc.RPLACA);
            query.AppendFormat(" '{0}', '{1}',", rrcc.RFECREP, rrcc.RUBIC);
            query.AppendFormat(" '3', '{0}',", rrcc.RTIPNOV);
            query.AppendFormat(" '{0}', '{1}',", rrcc.RREPORT, rrcc.RCRTUSR);
            query.AppendFormat(" 'APIBRINSA', '{0}')", rrcc.RWINDAT);

            try
            {
                db.Execute(query.ToString());
                query.Clear();
                query.Append("SELECT IDENTITY_VAL_LOCAL() FROM SYSIBM.SYSDUMMY1");
                res = db.Query<int>(query.ToString()).Single().ToString();                               
            }
            catch (iDB2SQLErrorException ex)
            {
                res = "ERROR: " + ex.Message;
            }
            catch (Exception ex)
            {
                res = "ERROR: " + ex.Message;
            }
            return res;
        }

        public RRCC FindId(string Id)
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT RCONSO, RPLANI, TRIM(RMANIF) RMANIF, RPLACA, RFECREP, TRIM(RTIPNOV) RTIPNOV, RREPORT");
            query.Append(" FROM RRCC");
            query.AppendFormat(" WHERE RID = {0}", Id);
            return db.Query<RRCC>(query.ToString()).SingleOrDefault();
        }
    }
}
