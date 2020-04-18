using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace foodComparisonServer.Services.SQLGateway.v1
{
    public class SQLGateway
    {
        #region GLOBAL VARIABLES
        private IConfiguration Configuration;
        private IHostingEnvironment HostingEnvironment;
        public string _connString;
        public SqlDataAdapter da;
        public DataSet ds;
        private string ConnectionStringLocalDB;
        #endregion

        #region CONSTRUCTOR
        public SQLGateway(IHostingEnvironment _HostingEnvironment, IConfiguration _Configuration)
        {
            HostingEnvironment = _HostingEnvironment;
            Configuration = _Configuration;
            this.ConnectionStringLocalDB = Configuration["ConnnectonStrings:LocalConnectionDB"];                       
        }
        #endregion

        private SqlConnection GetConnection(string _ConnectionType)
        {
            if (_ConnectionType == "ldb")
            {
                return new SqlConnection(this.ConnectionStringLocalDB);
            }
            else
            {
                return new SqlConnection(this.ConnectionStringLocalDB);
            }
        }

        private void CloseConnection(SqlConnection _Connection)
        {
            if (_Connection.State == ConnectionState.Open)
            {
                _Connection.Close();
                _Connection.Dispose();
            }
        }

        #region DB CALLS
        public DataTable bindDataTableParam(String _StoredProcedureName, SqlParameter[] _ParameterList = null, bool _IsUID = false)
        {
            string DBType = string.Empty;
            if (_IsUID)
            {
                DBType = "uid";
            }

            //USING BLOCK WILL AUTOMATICALLY CLOSE & DISPOSE THE CONNECTION[EVEN IF EXCEPTION OCCURS]
            using (SqlConnection Connection = GetConnection("ldb"))
            {
                try
                {
                    // OPEN THE SQLCONNECTION.
                    if (Connection.State == ConnectionState.Closed)
                    {
                        Connection.Open();
                    }
                    //USING BLOCK WILL AUTOMATICALLY CLOSE & DISPOSE THE COMMAND [EVEN IF EXCEPTION OCCURS]
                    using (SqlCommand LocalDbCommand = new SqlCommand(_StoredProcedureName, Connection))
                    {
                        using (DataSet LocalDS = new DataSet())
                        {
                            LocalDbCommand.CommandType = CommandType.StoredProcedure;
                            if (_ParameterList != null)
                            {
                                for (int i = 0; i <= _ParameterList.Length - 1; i++)
                                {
                                    LocalDbCommand.Parameters.Add(_ParameterList[i]);
                                }
                            }
                            using (DataAdapter LocalDA = new SqlDataAdapter(LocalDbCommand))
                            {
                                LocalDA.Fill(LocalDS);
                                if (LocalDS.Tables.Count > 0)
                                {
                                    return LocalDS.Tables[0];
                                }
                                else
                                {
                                    return null;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    CloseConnection(Connection);
                    throw ex;
                }
                finally
                {
                    CloseConnection(Connection);
                }
            }
        }
        #endregion
    }
}
