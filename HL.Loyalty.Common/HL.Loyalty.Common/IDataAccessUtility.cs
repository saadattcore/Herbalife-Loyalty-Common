using System.Data;
using System.Data.SqlClient;

namespace HL.Loyalty.Common
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
