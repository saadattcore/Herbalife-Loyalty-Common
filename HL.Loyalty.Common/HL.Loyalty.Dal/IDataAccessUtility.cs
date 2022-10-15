using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL.Loyalty.Utilities
{
    public interface IDataAccessUtility
    {
        bool ExecuteNonQuery(string sproc, SqlParameter[] sqlParameters);

        SqlCommand GetReaderCommand(string sproc, SqlParameter[] sqlParameters);

        object ExecuteScalar(string sproc, SqlParameter[] sqlParameters);

        DataSet Execute(string sproc, SqlParameter[] sqlParameters);

        T GetReaderValue<T>(IDataRecord reader, string columnName, T defaultValue);
    }
}
