using System;
using System.Data;

namespace Primosoft.DbUtils
{

    public interface IDataBase : IDisposable
    {
        string ConnectionString { get; }

        bool Transactional { get; }

        IDbConnection Connection { get; }

        IDbCommand Command { get; }

        IDbTransaction Transaction { get; }

        string TestConnection();

        void OpenConnection();

        void SetCommand(string commandText, CommandType commandType = CommandType.StoredProcedure);

        void AddParameter(string parameterName, object value = null);

        int ExecuteNonQuery();

        IDataReader ExecuteReader();

        object ExecuteScalar();

        DataTable ExecuteQuery(string tableName = "Query");

        void BeginTransaction();

        void BeginTransaction(string transactionName);

        void SaveTransaction(string savePointName);

        void CommitTransaction();

        void RollbackTransaction();

        void RollbackTransaction(string savePointName);

        void CleanCommand();

        void CloseConnection();
        
    }

}
