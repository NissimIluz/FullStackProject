using DALContract;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace InfraDAL
{
    class SqlParameterAdapter:IParameter
    {
        public SqlParameter Parameter { get; set; }
    }
}
