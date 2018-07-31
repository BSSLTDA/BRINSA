using CLCommom;
using Dapper;
using IBM.Data.DB2.iSeries;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System;

namespace CLDB2
{
    public class RDCCRepository : IRDCCRepository
    {
        private IDbConnection db = new iDB2Connection(ConfigurationManager.ConnectionStrings["ConexionDB2"].ConnectionString);

        public RDCC GetInfo(string DPLANI)
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT DCONSO, HSTS, HCELU");
            query.Append(" FROM RDCC");
            query.Append(" INNER JOIN RHCC ON DCONSO = HCONSO");
            query.Append(" WHERE DPLANI = " + DPLANI);
            query.Append(" GROUP BY DCONSO, HSTS, HCELU");
            return db.Query<RDCC>(query.ToString()).SingleOrDefault();
        }

        public List<RDCC> Get(string DCONSO)
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT DPEDID, DLINEA, LLLOAD, DPLANI");
            query.Append(" FROM RDCC");
            query.Append(" INNER JOIN LLL ON DPEDID = LLORDN AND DLINEA = LLOLIN");
            query.Append(" WHERE DCONSO = '" + DCONSO + "'");
            query.Append(" AND DPLANI <> LLLOAD");
            return db.Query<RDCC>(query.ToString()).ToList();
        }

        public List<RDCC> Getdata(string DCONSO)
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT DPEDID, DLINEA, ILDPFX, ILDOCN, DLDPFX, DLDOCN");
            query.Append(" FROM RDCC");
            query.Append(" INNER JOIN SIL ON DPEDID = ILORD AND DLINEA=ILSEQ");
            query.Append(" WHERE DCONSO = '" + DCONSO + "'");
            query.Append(" AND DLDOCN <> ILDOCN");
            return db.Query<RDCC>(query.ToString()).ToList();
        }

        public List<RDCC> Getdata2(string DCONSO)
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT DPEDID, DPLANI, MAX(DORDENT) AS DORDENT, DIDIVI, DCUST, DSHIP");
            query.Append(" FROM RDCC");
            query.Append(" WHERE DCONSO = '" + DCONSO + "' AND DPLANI > 0");
            query.Append(" GROUP BY DPEDID, DPLANI, DIDIVI, DCUST, DSHIP");
            query.Append(" ORDER BY DPEDID, DPLANI");
            return db.Query<RDCC>(query.ToString()).ToList();
        }

        public string Update(RDCC rdcc)
        {
            string res;
            StringBuilder query = new StringBuilder();
            query.Append(" UPDATE RDCC");
            query.Append(" SET");
            query.Append(" DPLANI = " + rdcc.DPLANI);
            query.Append(" WHERE DPEDID = " + rdcc.DPEDID + " AND DLINEA = " + rdcc.DLINEA);

            try
            {
                db.Execute(query.ToString(), rdcc);
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

        public string Updatedata(RDCC rdcc)
        {
            string res;
            StringBuilder query = new StringBuilder();
            query.Append(" UPDATE RDCC");
            query.Append(" SET");
            query.Append(" DLDPFX = '" + rdcc.DLDPFX + "', DLDOCN = " + rdcc.DLDOCN);
            query.Append(" WHERE DPEDID = " + rdcc.DPEDID + " AND DLINEA = " + rdcc.DLINEA);

            try
            {
                db.Execute(query.ToString(), rdcc);
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

        public string UpdateConso(string Conso)
        {
            string res;
            StringBuilder query = new StringBuilder();
            query.Append(" UPDATE RDCC");
            query.Append(" SET");
            query.Append(" DNUMENT = 0");
            query.Append(" WHERE DCONSO = '" + Conso + "'");

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

        public string UpdateDNUMENT(string Conso)
        {
            string res;
            StringBuilder query = new StringBuilder();
            query.Append(" UPDATE RDCC D");
            query.Append(" SET");
            query.Append(" (DNUMENT) = (");
            query.Append(" SELECT EID");
            query.Append(" FROM RECC");
            query.Append(" WHERE D.DCONSO = ECONSO AND D.DPEDID = EPEDID");
            query.Append(" AND D.DIDIVI = EIDIVI AND D.DCUST = ECUST");
            query.Append(" AND D.DSHIP = ESHIP AND D.DPLANI = EPLANI)");
            query.Append(" WHERE DCONSO = '" + Conso + "' AND DPLANI > 0");

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

        public List<RDCC> GetDPLANI(string DCONSO)
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT DPLANI");
            query.Append(" FROM RDCC");
            query.Append(" LEFT OUTER JOIN RECC ON DNUMENT = EID AND DPLANI = EPLANI");
            query.Append(" WHERE DCONSO = '" + DCONSO + "'");
            query.Append(" AND EID IS NULL");
            return db.Query<RDCC>(query.ToString()).ToList();
        }

        public List<RDCC> GetDespacho(string DCONSO)
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT DISTINCT");
            query.Append(" d.DCONSO");
            query.Append(" FROM RDCC d");
            query.Append(" INNER JOIN ECL e ON d.DPEDID = e.LORD AND d.DLINEA = e.LLINE");
            query.Append(" WHERE d.DCONSO = '" + DCONSO + "'");
            query.Append(" AND e.LQSHP > 0");
            return db.Query<RDCC>(query.ToString()).ToList();
        }

        public List<RDCC> GetDestino(string DCONSO)
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT DISTINCT D.CCIUD AS Destino");
            query.Append(" FROM RDCC P");
            query.Append(" INNER JOIN RFDANE D ON D.CLXCIUD = P.DSPOST AND D.CLXDPTO = P.DSHDEP");
            query.AppendFormat(" WHERE P.DCONSO = '{0}'", DCONSO);
            query.Append(" AND D.CTIPO = 'CM'");
            return db.Query<RDCC>(query.ToString()).ToList();
        }

        public List<RDCC> GetPlanilla(string DCONSO)
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT DISTINCT DPLANI");
            query.Append(" FROM RDCC");
            query.AppendFormat(" WHERE DCONSO = '{0}'", DCONSO);
            query.Append(" AND DPLANI > 0");
            //query.Append(" ORDER BY DORDENT");
            return db.Query<RDCC>(query.ToString()).ToList();
        }
    }
}
