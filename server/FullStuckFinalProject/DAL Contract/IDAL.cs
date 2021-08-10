using System;
using System.Data;

namespace DALContract
{
    public interface IDAL
    {
        void ExecuteNonQuery(string spName, params IParameter[] parameters);
        DataSet ExecuteQuery(string spName, params IParameter[] parameters);
        IParameter CreateParameter(string paramName, object value);

    }
}
