using System;

namespace API.Library.APIWrappers
{
    /// <summary>
    ///     Interface for wrapping System.Console
    /// </summary>
    public interface IConsole
    {
        /// <summary>
        ///     Writes to the Console
        /// </summary>
        /// <param name="message">Message to write</param>
        void Write(string message);

        /// <summary>
        ///     Writes a line to the Console
        /// </summary>
        /// <param name="message">Message to write</param>
        void WriteLine(string message);

        /// <summary>
        ///     Writes to the Console.Error (standard error)
        /// </summary>
        /// <param name="message">Message to write</param>
        void ErrorWrite(string message);

        /// <summary>
        ///     Writes a line to the Console.Error (standard error)
        /// </summary>
        /// <param name="message">Message to write</param>
        void ErrorWriteLine(string message);
    }

    /// <summary>
    ///     Wraps the DateTime structure
    /// </summary>
    public interface IDateTime
    {
        /// <summary>
        ///     Gets the DateTime as of Now
        /// </summary>
        /// <returns>A DateTime object containing the date and time of Now</returns>
        DateTime Now();
    }

    /// <summary>
    ///     Wraps the System.Uri class
    /// </summary>
    public interface IUri
    {
        /// <summary>
        ///     Creates a Uri based on the specified Uri string
        /// </summary>
        /// <param name="uriString">The Uri string</param>
        /// <returns>A DateTime object containing the date and time of Now</returns>
        Uri GetUri(string uriString);
    }

    /// <summary>
    ///     Class for wrapping System.Console
    /// </summary>
    public class SystemConsole : IConsole
    {
        /// <summary>
        ///     Writes to the Console
        /// </summary>
        /// <param name="message">Message to write</param>
        public void Write(string message)
        {
            Console.Write(message);
        }

        /// <summary>
        ///     Writes a line to the Console
        /// </summary>
        /// <param name="message">Message to write</param>
        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }

        /// <summary>
        ///     Writes to the Console.Error (standard error)
        /// </summary>
        /// <param name="message">Message to write</param>
        public void ErrorWrite(string message)
        {
            Console.Error.Write(message);
        }

        /// <summary>
        ///     Writes a line to the Console.Error (standard error)
        /// </summary>
        /// <param name="message">Message to write</param>
        public void ErrorWriteLine(string message)
        {
            Console.Error.WriteLine(message);
        }
    }

    /// <summary>
    ///     Wraps the System.DateTime structure
    /// </summary>
    public class SystemDateTime : IDateTime
    {
        /// <summary>
        ///     Gets the DateTime as of Now
        /// </summary>
        /// <returns>A DateTime object containing the date and time of Now</returns>
        public DateTime Now()
        {
            return DateTime.Now;
        }
    }

    /// <summary>
    ///     Wraps the System.URI class
    /// </summary>
    public class SystemUri : IUri
    {
        /// <summary>
        ///     Creates a Uri based on the specified Uri string
        /// </summary>
        /// <param name="uriString">The Uri string</param>
        /// <returns>A DateTime object containing the date and time of Now</returns>
        public Uri GetUri(string uriString)
        {
            return new Uri(uriString);
        }
    }
}