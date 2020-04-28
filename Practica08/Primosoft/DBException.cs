using System;

namespace Primosoft.DbUtils
{

    /// <summary>
    /// The exception that is thrown when the database returns a warning or error.
    /// </summary>
    public class DBException : System.Data.Common.DbException
    {

        /// <summary>
        /// Initializes a new instance of the DBException class.
        /// </summary>
        public DBException() : base() { }
        
        /// <summary>
        /// Initializes a new instance of the DBException class with the specified error message.
        /// </summary>
        /// <param name="message">The message to display for this exception.</param>
        public DBException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the DBException class with the specified error message 
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message to display for this exception.</param>
        /// <param name="innerException">The inner exception reference.</param>
        public DBException(string message, Exception innerException)
            : base(message, innerException) { }

    }

}
