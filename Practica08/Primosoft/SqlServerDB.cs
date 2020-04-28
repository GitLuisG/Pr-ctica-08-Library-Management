using System;
using System.Data;
using System.Data.SqlClient;

namespace Primosoft.DbUtils
{

    /// <summary>
    /// Class that implements functions to work with SQL Server.
    /// </summary>
    public class SqlServerDB : IDataBase
    {

        /// <summary>
        /// Tests the connection to the database. If an error happens, it returns 
        /// the description of the error; else it returns null.
        /// </summary>
        /// <returns>Description of the error. Null when no error.</returns>
        public string TestConnection()
        {
            string errorMessage = null;
            var connection = new SqlConnection(ConnectionString);
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            finally
            {
                try
                {
                    connection.Close();
                }
                catch { }
                connection.Dispose();
            }
            return errorMessage;
        }

        /// <summary>
        /// Initializes a new instance of the SQLServerDB class when given a string that contains the connection string.
        /// </summary>
        public SqlServerDB(string connectionString)
        {
            _connectionString = connectionString;
            _connection = new SqlConnection(connectionString);
            _command = new SqlCommand();
            _command.Connection = _connection;
        }

        /// <summary>
        /// Gets the string used to open a database.
        /// </summary>
        public string ConnectionString
        {
            get { return _connectionString; }
        }

        /// <summary>
        /// Gets a value that indicates whether the actual command is transactional.
        /// </summary>
        public bool Transactional
        {
            get { return _transactional; }
        }

        /// <summary>
        /// Gets the System.Data.SqlClient.SqlConnection. 
        /// </summary>
        public IDbConnection Connection
        {
            get { return _connection; }
        }

        /// <summary>
        /// Gets the System.Data.SqlClient.SqlCommand used to execute the operations with the database.
        /// </summary>
        public IDbCommand Command
        {
            get { return _command; }
        }

        /// <summary>
        /// Gets the System.Data.SqlClient.SqlTransaction
        /// </summary>
        public IDbTransaction Transaction
        {
            get { return _transaction; }
        }
        
        /// <summary>
        /// Opens a database connection.
        /// </summary>
        /// <exception cref="DBException">
        /// Could not open database connection.
        /// </exception>
        public void OpenConnection()
        {
            if (_connection.State == ConnectionState.Open) return;
            try
            {
                _connection.Open();
            }
            catch (Exception ex)
            {
                throw new DBException("Could not open database connection. " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Sets the command or statment to execute.
        /// </summary>
        /// <param name="commandText">The command text to execute.</param>
        /// <param name="commandType">The command type: text (sql injection), stored procedure. See System.Data.CommandType.</param>
        public void SetCommand(string commandText, CommandType commandType = CommandType.StoredProcedure)
        {
            OpenConnection();
            CleanCommand();
            _command.CommandText = commandText;
            _command.CommandType = commandType;
        }

        /// <summary>
        /// Adds a parameter and its value to the commant to execute (input parameter only).
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="value">An object that is the value of the parameter. null = DBNull.Value.</param>
        public void AddParameter(string parameterName, object value = null)
        {
            if (value != null)
                _command.Parameters.AddWithValue(parameterName, value);
            else
                _command.Parameters.AddWithValue(parameterName, DBNull.Value);
        }

        /// <summary>
        /// Execute a statement against the connection and returns the number of rows affected.
        /// </summary>
        /// <returns>The number of rows affected.</returns>
        /// <exception cref="DBException"></exception>
        public int ExecuteNonQuery()
        {
            var rowsAffected = 0;
            try
            {
                rowsAffected = _command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new DBException(ex.Message, ex);
            }
            return rowsAffected;
        }

        /// <summary>
        /// Executes a query and sets the DataReader R property for a forward only read. 
        /// </summary>
        /// <exception cref="DBException"></exception>
        public IDataReader ExecuteReader()
        {
            SqlDataReader r = null;
            try
            {
                r = _command.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw new DBException(ex.Message, ex);
            }
            return r;
        }

        /// <summary>
        /// Executes the command and return the first value.
        /// </summary>
        /// <returns>First value of the command.</returns>
        /// <exception cref="DBException"></exception>
        public object ExecuteScalar()
        {
            object val = null;
            try
            {
                val = _command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new DBException(ex.Message, ex);
            }
            return val;
        }

        /// <summary>
        /// Executes the actual command/statement and returns a DataTable
        /// with the result of the query.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <returns>A DataTable of the result of the query.</returns>
        /// <exception cref="DBException"></exception>
        public DataTable ExecuteQuery(string tableName = "Query")
        {
            var da = new SqlDataAdapter(_command);
            var dataTable = new DataTable(tableName);
            try
            {
                da.Fill(dataTable);
            }
            catch (Exception ex)
            {
                throw new DBException(ex.Message, ex);
            }
            finally
            {
                da.Dispose();
            }
            return dataTable;
        }

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <exception cref="DBException">
        /// Could not begin the transaction.
        /// </exception>
        public void BeginTransaction()
        {
            try
            {
                _transaction = _connection.BeginTransaction();
                _command.Transaction = _transaction;
                _transactional = true;
            }
            catch (Exception ex)
            {
                throw new DBException("Could not begin the transaction. " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <param name="transactionName">The name of the transaction.</param>
        /// <exception cref="DBException">
        /// Could not begin the transaction.
        /// </exception>
        public void BeginTransaction(string transactionName)
        {
            try
            {
                _transaction = _connection.BeginTransaction(transactionName);
                _command.Transaction = _transaction;
                _transactional = true;
            }
            catch (Exception ex)
            {
                throw new DBException("Could not begin the transaction. " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Creates a savepoint in the transaction that can be used to roll back a part of the transaction, and specifies the savepoint name.
        /// </summary>
        /// <param name="savePointName">The name of the savepoint.</param>
        /// <exception cref="DBException"></exception>
        public void SaveTransaction(string savePointName)
        {
            try
            {
                if (_transactional && _transaction != null)
                    _transaction.Save(savePointName);
                else
                    throw new DBException("There aren't transactions to save.");
            }
            catch (Exception ex)
            {
                throw new DBException("Could not save the transaction. " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Commits the database transaction.
        /// </summary>
        /// <exception cref="DBException">
        /// Could not commit the transaction. There aren't transactions to commit. |
        /// Could not commit the transaction. 
        /// </exception>
        public void CommitTransaction()
        {
            try
            {
                if (_transactional && _transaction != null)
                    _transaction.Commit();
                else
                    throw new DBException("There aren't transactions to commit.");
            }
            catch (Exception ex)
            {
                throw new DBException("Could not commit the transaction. " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Rolls back a transaction from a pending state. 
        /// </summary>
        /// <remarks>
        /// This method does not throw exceptions.
        /// </remarks>
        public void RollbackTransaction()
        {
            if (!_transactional || _transaction == null) return;
            try
            {
                _transaction.Rollback();
            }
            catch { }
        }

        /// <summary>
        /// Rolls back a transaction from a pending state. 
        /// </summary>
        /// <param name="savePointName">The savepoint to which to roll back.</param>
        /// <remarks>
        /// This method does not throw exceptions.
        /// </remarks>
        public void RollbackTransaction(string savePointName)
        {
            if (!_transactional || _transaction == null) return;
            try
            {
                _transaction.Rollback(savePointName);                
            }
            catch { }
        }

        /// <summary>
        /// Cleans the resources for the next execution or statament to execute.
        /// </summary>
        public void CleanCommand()
        {
            _command.CommandText = null;
            _command.Parameters.Clear();
        }

        /// <summary>
        /// Closes the connection to the database.
        /// </summary>
        /// <remarks>
        /// This method does not throw exceptions.
        /// </remarks>
        public void CloseConnection()
        {
            CleanCommand();
            if (Connection == null || Connection.State == ConnectionState.Closed) return;
            try
            {
                _connection.Close();
            }
            catch { }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private string _connectionString;

        private bool _transactional;

        private SqlConnection _connection;

        private SqlCommand _command;

        private SqlTransaction _transaction;
        
        private bool _disposedValue;

        protected void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _connectionString = null;
                    CloseConnection();
                    if (Command != null)
                    {
                        _command.Dispose();
                        _command = null;
                    }
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }
                    if (_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null;
                    }
                }
            }
            _disposedValue = true;
        }

    }

}
