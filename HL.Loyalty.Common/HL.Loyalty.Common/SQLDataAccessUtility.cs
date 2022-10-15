using System;
using System.Data;
using System.Data.SqlClient;

namespace HL.Loyalty.Common
{
    public class SqlDataAccessUtility: IDataAccessUtility
    {
        private readonly string _connectionString;

        public SqlDataAccessUtility(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));
            _connectionString = connectionString;
        }

        public bool ExecuteNonQuery(string sproc, SqlParameter[] sqlParameters)
        {
            var command = new SqlCommand();
            try
            {
                command.Connection = new SqlConnection(_connectionString);
                command.CommandText = sproc;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddRange(sqlParameters);
                command.Connection.Open();
                var result = command.ExecuteNonQuery();
                return result > 0;
            }
            finally
            {
                if (null != command.Connection && ConnectionState.Closed != command.Connection.State)
                {
                    command.Connection.Close();
                }
            }
        }

        public SqlCommand GetReaderCommand(string sproc, SqlParameter[] sqlParameters)
        {
            var command = new SqlCommand();
            command.Connection = new SqlConnection(_connectionString);
            command.CommandText = sproc;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(sqlParameters);
            command.Connection.Open();
            return command;
        }

        public object ExecuteScalar(string sproc, SqlParameter[] sqlParameters)
        {
            var command = new SqlCommand();
            try
            {
                command.Connection = new SqlConnection(_connectionString);
                command.CommandText = sproc;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddRange(sqlParameters);
                command.Connection.Open();
                var scalarValue = command.ExecuteScalar();
                return scalarValue;
            }
            catch (Exception ex)
            {
                //Log error
            }
            finally
            {
                if (null != command.Connection && ConnectionState.Closed != command.Connection.State)
                {
                    command.Connection.Close();
                }
            }
            return null;
        }

        public DataSet Execute(string sproc, SqlParameter[] sqlParameters)
        {
            var dbConnect = new SqlConnection(_connectionString);
            SqlDataAdapter da;

            try
            {
                da = new SqlDataAdapter(sproc, dbConnect);
                da.SelectCommand.Parameters.AddRange(sqlParameters);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                var ds = new DataSet();
                dbConnect.Open();
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                //Log error
            }

            return null;
        }

        public T GetReaderValue<T>(IDataRecord reader, string columnName, T defaultValue)
        {
            return (DBNull.Value != reader[columnName])
                       ? (T)Convert.ChangeType(reader[columnName], typeof(T))
                       : defaultValue;
        }
    }
}
