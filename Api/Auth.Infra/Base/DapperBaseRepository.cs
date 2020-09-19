using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Auth.Infra.Base
{
    public class DapperBaseRepository
    {
        private SqlConnection Create()
        {   
            return new SqlConnection("");
        }

        public int Execute(string query)
        {
            int ret;
            using (var cn = Create())
            {
                cn.Open();
                ret = cn.Execute(query);
                cn.Close();
                cn.Dispose();
            }
            return ret;
        }

        public IEnumerable<T> Find<T>(string query)
        {
            IEnumerable<T> items;
            using (var cn = Create())
            {
                cn.Open();
                items = cn.Query<T>(query);
                cn.Close();
                cn.Dispose();
            }

            return items;
        }
    }
}