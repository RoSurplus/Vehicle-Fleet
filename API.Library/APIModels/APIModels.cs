namespace API.Library.APIModels
{
    /// <summary>
    ///     Model that represents a HW_Message response
    /// </summary>
    public class HW_Message
    {
        /// <summary>
        ///     Gets or sets the data
        /// </summary>
        public string Data { get; set; }
    }

    /// <summary>
    ///     Model for error responses
    /// </summary>
    public class ErrorResponseContent
    {
        /// <summary>
        /// Gets or sets the error code used by marketing
        /// </summary>
        /// <value>
        /// The error code for looking up content managed by marketing
        /// </value>
        public string ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the exception message
        /// </summary>
        /// <value>
        /// The exception message
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the exception type
        /// </summary>
        /// <value>
        /// The exception type
        /// </value>
        public string ExceptionType { get; set; }

        /// <summary>
        /// Gets or sets the full exception message, inner exception messages, and stack trace
        /// </summary>
        /// <value>
        /// The stack trace
        /// </value>
        public string FullException { get; set; }

        /// <summary>
        /// Gets or sets the error severity
        /// </summary>
        /// <value>
        /// The error severity
        /// </value>
        public string Severity { get; set; }
    }
}