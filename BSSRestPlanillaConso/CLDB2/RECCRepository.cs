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
    public class RECCRepository : IRECCRepository
    {
        private IDbConnection db = new iDB2Connection(ConfigurationManager.ConnectionStrings["ConexionDB2"].ConnectionString);

        public string Add(string DPLANI)
        {
            string res;
            StringBuilder query = new StringBuilder();
            query.Append(" INSERT INTO RECC");
            query.Append(" (ECONSO, EPEDID, EPLANI, EORDENT,");
            query.Append(" EFREQCIT, EIDIVI, ECUST, ESHIP, EPESOD, ECANTD)");

            query.Append(" SELECT DCONSO, DPEDID, DPLANI, MAX(DORDENT),");
            query.Append(" MAX(DFEC02), DIDIVI, DCUST, DSHIP, SUM(DPESO), SUM(DCANT)");
            query.Append(" FROM RDCC");
            query.Append(" WHERE DPLANI = " + DPLANI);
            query.Append(" GROUP BY DCONSO, DPEDID, DPLANI, DIDIVI, DCUST, DSHIP");

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

        public void Delete(string Conso)
        {
            string res;
            StringBuilder query = new StringBuilder();
            query.Append(" DELETE FROM RECC");
            query.Append(" WHERE ECONSO = '" + Conso + "' AND EPLANI = 0");

            try
            {
                db.Execute(query.ToString(), Conso);
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
        }

        public List<RECC> GetData(string Conso)
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT EPLANI");
            query.Append(" FROM RECC");
            query.Append(" LEFT OUTER JOIN RDCC ON EID = DNUMENT AND EPLANI = DPLANI");
            query.Append(" WHERE ECONSO = '" + Conso + "' AND DPLANI IS NULL");
            return db.Query<RECC>(query.ToString()).ToList();
        }

        public List<RECC> GetData2(RECC recc)
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT EID");
            query.Append(" FROM RECC");
            query.Append(" WHERE ECONSO = '" + recc.ECONSO + "'");
            query.Append(" AND EPEDID = " + recc.EPEDID);
            query.Append(" AND EIDIVI = '" + recc.EIDIVI + "'");
            query.Append(" AND ECUST = " + recc.ECUST);
            query.Append(" AND ESHIP = " + recc.ESHIP);
            query.Append(" AND EPLANI = 0");
            return db.Query<RECC>(query.ToString()).ToList();
        }

        public List<RECC> GetData3(string Conso)
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT *");
            query.Append(" FROM RECC");
            query.Append(" WHERE ECONSO = '" + Conso + "'");
            query.Append(" AND ESTS < 50 AND EIDIVI <> 'QUIMICOS'");           
            return db.Query<RECC>(query.ToString()).ToList();
        }

        public RECC GetPlani(string Planilla)
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT EPLANI");
            query.Append(" FROM RECC");
            query.Append(" WHERE EPLANI = " + Planilla);
            return db.Query<RECC>(query.ToString()).SingleOrDefault();
        }

        public string UpdateConso(string Conso)
        {
            string res;
            StringBuilder query = new StringBuilder();
            query.Append(" UPDATE RECC");
            query.Append(" SET");
            query.Append(" EPLANI = 0");
            query.Append(" WHERE ECONSO = '" + Conso + "'");

            try
            {
                db.Execute(query.ToString(), Conso);
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

        public string UpdateConso2(string Conso)
        {
            string res;
            StringBuilder query = new StringBuilder();
            query.Append(" UPDATE RECC");
            query.Append(" SET");
            query.Append(" ESTS = 50");
            query.Append(" WHERE ECONSO = '" + Conso + "' AND YEAR(EFINENT) >= 2014");
            query.Append(" AND ESTS < 50 AND EIDIVI <> 'QUIMICOS'");

            try
            {
                db.Execute(query.ToString(), Conso);
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

        public string UpdatePlani(string EPLANI, string EID)
        {
            string res;
            StringBuilder query = new StringBuilder();
            query.Append(" UPDATE RECC");
            query.Append(" SET");
            query.Append(" EPLANI = " + EPLANI);
            query.Append(" WHERE EID = " + EID);

            try
            {
                db.Execute(query.ToString());
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

        public string UpdateESTS(string EFINENT, string EPLANI)
        {
            string res;
            StringBuilder query = new StringBuilder();
            query.Append(" UPDATE RECC");
            query.Append(" SET");
            query.Append(" ESTS = 50, EFINENT = '" + EFINENT + "',");
            query.Append(" ESMS = 1");
            query.Append(" WHERE EPLANI = " + EPLANI + " AND ESTS <= 50");

            try
            {
                db.Execute(query.ToString());
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

        public string UpdateESTS2(string EFINENT, string EPLANI, string ECELOK)
        {
            string res;
            StringBuilder query = new StringBuilder();
            query.Append(" UPDATE RECC");
            query.Append(" SET");
            query.Append(" ESTS = 50, EFINENT = '" + EFINENT + "',");
            query.Append(" ESMS = 1");
            if (ECELOK != "")
            {
                query.Append(" , ECELOK = " + ECELOK);
            }
            query.Append(" WHERE EPLANI = " + EPLANI + " AND ESTS <= 50");

            try
            {
                db.Execute(query.ToString());
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

        public string UpdateSelect(string CONSO)
        {
            string res;
            StringBuilder query = new StringBuilder();
            query.Append(" UPDATE RECC E");
            query.Append(" SET");
            query.Append(" (EHNAME, ENSHIP, ESPOST, ESHDEP) =");
            query.Append(" (SELECT TCUSTNAME, TNAME, TPOST, TSTE FROM RVESTTOT00 WHERE E.ECUST = TCUST AND E.ESHIP = TSHIP)");
            query.Append(" WHERE  ECONSO='" + CONSO + "'");

            try
            {
                db.Execute(query.ToString());
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
