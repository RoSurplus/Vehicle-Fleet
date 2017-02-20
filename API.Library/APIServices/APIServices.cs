using System;
using System.Collections.Generic;
using System.IO;
using System.Configuration;
using API.Library.APIModels;
using API.Library.APIResources;
using API.Library.APIMappers;
using API.Library.APIWrappers;
using log4net.Config;
using log4net.Core;
using System.Web.Hosting;

namespace API.Library.APIServices
{
    /// <summary>
    ///     Service for application settings
    /// </summary>
    public interface IAppSettings
    {
        /// <summary>
        ///     Gets the string value of a configuration value
        /// </summary>
        /// <param name="name">The name</param>
        /// <returns>The application settings value</returns>
        string Get(string name);
    }

    /// <summary>
    ///     Service for application settings in a configuration file
    /// </summary>
    public class ConfigAppSettings : IAppSettings
    {
        /// <summary>
        ///     Gets the string value of a configuration value
        /// </summary>
        /// <param name="name">The name</param>
        /// <returns>The application settings value</returns>
        public string Get(string name)
        {
            return ConfigurationManager.AppSettings.Get(name);
        }
    }

    /// <summary>
    ///     Service for logging
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        ///     Write an INFO message to the log
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="otherProperties">Other properties</param>
        void Info(string message, Dictionary<string, object> otherProperties);

        /// <summary>
        ///     Write an DEBUG message to the log
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="otherProperties">Other properties</param>
        void Debug(string message, Dictionary<string, object> otherProperties);

        /// <summary>
        ///     Write an ERROR message to the log
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="otherProperties">Other properties</param>
        /// <param name="exception">Exception instance</param>
        void Error(string message, Dictionary<string, object> otherProperties, Exception exception);
    }

    /// <summary>
    ///     Data Service for manipulating data
    /// </summary>
    public interface IDataService
    {
        /// <summary>
        ///     Gets today's data
        /// </summary>
        /// <returns>A TodaysData model containing today's data</returns>
        //TodaysData GetTodaysData();
        HW_Message GetHW_Message();

        /// <summary>
        ///     Get the serialized Fleet_File
        /// </summary>
        /// <returns>A serialized Fleet_File</returns>
        HW_Message HW_Load_Fleet_File();
    }

    /// <summary>
    ///     Service for file IO
    /// </summary>
    public interface IFileIOService
    {
        /// <summary>
        ///     Reads the specified file
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <returns>The contents of the file</returns>
        string ReadFile(string filePath);

        /// <summary>
        ///     Writes the specified file
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <returns>The contents of the file</returns>
        void WriteFile(string filePath, string fileContent);
    }

    /// <summary>
    ///     Service for Hosting Environment
    /// </summary>
    public interface IHostingEnvironmentService
    {
        /// <summary>
        ///     Map's the specified path to the hosting environment's path
        /// </summary>
        /// <param name="path">The path</param>
        /// <returns>The hosting environment's path</returns>
        string MapPath(string path);
    }

    /// <summary>
    ///     Service for text file IO
    /// </summary>
    public class TextFileIOService : IFileIOService
    {
        /// <summary>
        ///     The hosting environment service
        /// </summary>
        private readonly IHostingEnvironmentService hostingEnvironmentService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TextFileIOService" /> class.
        /// </summary>
        /// <param name="hostingEnvironmentService">The injected hosting environment service</param>
        public TextFileIOService(IHostingEnvironmentService hostingEnvironmentService)
        {
            this.hostingEnvironmentService = hostingEnvironmentService;
        }

        /// <summary>
        ///     Reads the specified file
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <returns>The contents of the file</returns>
        public string ReadFile(string filePath)
        {
            string fileContents;

            // Map path to server path
            var serverPath = this.hostingEnvironmentService.MapPath(filePath);

            // Read the contents of the file
            try
            {
                using (var reader = new StreamReader(serverPath))
                {
                    fileContents = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                // Throw an IO exception
                throw new IOException(ErrorCodes.HW_MessageFileError, new IOException("There was a problem reading the data file!", ex));
            }

            return fileContents;
        }


        /// <summary>
        ///     Writes the specified file
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <returns>void</returns>
        public void WriteFile(string filePath, string fileContents)
        {
  //          string fileContents;

            // Map path to server path
            var serverPath = this.hostingEnvironmentService.MapPath(filePath);

            // Read the contents of the file
            try
            {
                using (var writer = new StreamWriter(serverPath))
                {
                    writer.Write(fileContents);
                }
            }
            catch (Exception ex)
            {
                // Throw an IO exception
                throw new IOException(ErrorCodes.HW_MessageFileError, new IOException("There was a problem reading the data file!", ex));
            }

//            return fileContents;
        }
    }

    /// <summary>
    ///     Logger class that uses the Log4Net library
    /// </summary>
    public class JsonL4NLogger : ILogger
    {
        /// <summary>
        ///     The log4net logger
        /// </summary>
        private readonly log4net.Core.ILogger log4NetLogger;

        /// <summary>
        ///     The logger name
        /// </summary>
        private string loggerName;

        /// <summary>
        ///     Initializes a new instance of the <see cref="JsonL4NLogger" /> class.
        /// </summary>
        public JsonL4NLogger()
        {
            XmlConfigurator.Configure();
            this.log4NetLogger = LoggerManager.GetLogger(this.GetType().Assembly, this.GetType().Name);
            ////this.log4NetLogger = LoggerManager.GetLogger(this.GetType().Assembly, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            this.loggerName = this.GetType().Name;
        }

        /// <summary>
        ///     Write an INFO message to the log
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="otherProperties">Other properties</param>
        public void Info(string message, Dictionary<string, object> otherProperties)
        {
            this.WriteLog(Level.Info, message, otherProperties, null);
        }

        /// <summary>
        ///     Write an DEBUG message to the log
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="otherProperties">Other properties</param>
        public void Debug(string message, Dictionary<string, object> otherProperties)
        {
            this.WriteLog(Level.Debug, message, otherProperties, null);
        }

        /// <summary>
        ///     Write an ERROR message to the log
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="otherProperties">Other properties</param>
        /// <param name="exception">Exception instance</param>
        public void Error(string message, Dictionary<string, object> otherProperties, Exception exception)
        {
            this.WriteLog(Level.Error, message, otherProperties, exception);
        }

        /// <summary>
        ///     Writes the log using log4net
        /// </summary>
        /// <param name="logLevel">Log level</param>
        /// <param name="message">Log message</param>
        /// <param name="otherProperties">Other properties</param>
        /// <param name="exception">Exception instance</param>
        private void WriteLog(Level logLevel, string message, Dictionary<string, object> otherProperties, Exception exception)
        {
            // Create the logging event data
            var loggingEventData = new LoggingEventData()
            {
                Level = logLevel,
                LoggerName = this.loggerName,
                Domain = AppDomain.CurrentDomain.FriendlyName,
                TimeStamp = DateTime.Now,
                Message = message
            };

            // Create the logging event
            var loggingEvent = new LoggingEvent(loggingEventData);

            // Check for other properties
            if (otherProperties != null)
            {
                foreach (var property in otherProperties)
                {
                    if (property.Key != null && property.Value != null)
                    {
                        loggingEvent.Properties[property.Key] = property.Value;
                    }
                }
            }

            // Check for exception
            if (exception != null)
            {
                loggingEvent.Properties["exception"] = exception.ToString();
            }

            // Log the data
            this.log4NetLogger.Log(loggingEvent);
        }
    }

    /// <summary>
    ///     Service for Server Hosting Environment
    /// </summary>
    public class ServerHostingEnvironmentService : IHostingEnvironmentService
    {
        /// <summary>
        ///     Map's the specified path to the hosting environment's path
        /// </summary>
        /// <param name="path">The path</param>
        /// <returns>The hosting environment's path</returns>
        public string MapPath(string path)
        {
            return HostingEnvironment.MapPath("~/" + path);
        }
    }

    /// <summary>
    ///     Data service for manipulating data
    /// </summary>
    public class HW_DataService : IDataService
    {
        /// <summary>
        ///     The application settings service
        /// </summary>
        private readonly IAppSettings appSettings;

        /// <summary>
        ///     The DateTime wrapper
        /// </summary>
        private readonly IDateTime dateTimeWrapper;

        /// <summary>
        ///     The File IO service
        /// </summary>
        private readonly IFileIOService fileIOService;

        /// <summary>
        ///     The Mapper
        /// </summary>
        private readonly IHW_Mapper HW_Mapper;

        /// <summary>
        ///     Initializes a new instance of the <see cref="HW_DataService" /> class.
        /// </summary>
        /// <param name="appSettings">The injected application settings service</param>
        /// <param name="dateTimeWrapper">The injected DateTime wrapper</param>
        /// <param name="fileIOService">The injected File IO Service</param>
        /// <param name="HW_Mapper">The injected Mapper</param>
        public HW_DataService(
            IAppSettings appSettings,
            IDateTime dateTimeWrapper,
            IFileIOService fileIOService,
            IHW_Mapper HW_Mapper)
        {
            this.appSettings = appSettings;
            this.dateTimeWrapper = dateTimeWrapper;
            this.fileIOService = fileIOService;
            this.HW_Mapper = HW_Mapper;
        }

        /// <summary>
        ///     The HW_Message's data
        /// </summary>
        /// <returns>A HW_Message model containing today's data</returns>
        public HW_Message GetHW_Message()
        {
            // Get the file path
            var filePath = this.appSettings.Get(AppSettingsKeys.HW_MessageFileKey);

            if (string.IsNullOrEmpty(filePath))
            {
                // No file path was found, throw exception
                throw new SettingsPropertyNotFoundException(
                    ErrorCodes.HW_MessageFileSettingsKeyError,
                    new SettingsPropertyNotFoundException("The HW_MessageFile settings key was not found or had no value."));
            }

            // Get the data from the file
            var rawData = this.fileIOService.ReadFile(filePath);

            // Map to the return type
            var hw_Message = this.HW_Mapper.StringToHW_Message(rawData);

            return hw_Message;
        }

        public HW_Message HW_Load_Fleet_File()
        {
            // Get the file path
            var filePath = this.appSettings.Get(AppSettingsKeys.HW_MessageFileKey);

            if (string.IsNullOrEmpty(filePath))
            {
                // No file path was found, throw exception
                throw new SettingsPropertyNotFoundException(
                    ErrorCodes.HW_MessageFileSettingsKeyError,
                    new SettingsPropertyNotFoundException("The HW_MessageFile settings key was not found or had no value."));
            }

            // Get the data from the file
            var rawData = this.fileIOService.ReadFile(filePath);

            // Map to the return type
            var hw_Message = this.HW_Mapper.StringToHW_Message(rawData);

            return hw_Message;
        }

        //public void Save_Fleet_File()
        //{
        //    if (session_fleet_file_invalid)
        //    {
        //        var filePath = ConfigurationManager.AppSettings.Get(AppSettingsKeys.Fleet_File);

        //        var iFileIO = new TextFileIOService(new ServerHostingEnvironmentService());
        //        iFileIO.WriteFile(filePath, Common.ToXML(session_fleet));
        //    }
        //}
    }
}

