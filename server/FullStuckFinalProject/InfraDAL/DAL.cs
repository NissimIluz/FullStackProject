using DALContract;
using System;
using System.Data;
using System.Data.SqlClient;
using Config;
using Microsoft.Extensions.Options;
using InfraAttributes;

namespace InfraDAL
{
    [Register(typeof(IDAL))]
    [Policy(Policy.Singleton)]
    public class DAL : IDAL
    {
        string _connetionString;
        public DAL(IOptions<ConfigOptions> settings)
        {
            _connetionString = settings.Value.MyConn;
        }
        private SqlCommand getCommand(string spName, params IParameter[] parameters)
        {
            var con = new SqlConnection(_connetionString);
            con.Open();
            SqlCommand commandSP = new SqlCommand();
            commandSP.CommandText = spName;
            commandSP.CommandType = CommandType.StoredProcedure;
            foreach (var parameter in parameters)
            {
                var paramAdapter = parameter as SqlParameterAdapter;
                commandSP.Parameters.Add(paramAdapter.Parameter);
            }

            commandSP.Connection = con;
            return commandSP;
        }

        public IParameter CreateParameter(string paramName, object value)
        {
            var retval = new SqlParameterAdapter();
            retval.Parameter = new SqlParameter(paramName, value);
            return retval as IParameter;
        }

        public DataSet ExecuteQuery(string spName, params IParameter[] parameters)
        {
            DataSet dataSet = new DataSet();
            var commandSP = getCommand(spName, parameters);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(commandSP);
            dataAdapter.Fill(dataSet);
            commandSP.Connection.Close();
            return dataSet;
        }
        public void ExecuteNonQuery(string spName, params IParameter[] parameters)
        {
            var commandSP = getCommand(spName, parameters);
            commandSP.ExecuteNonQuery();
            commandSP.Connection.Close();
        }
    }
}
