namespace API.Library.APIResources
{
    /// <summary>
    /// Contains the application settings keys for the application
    /// </summary>
    public class AppSettingsKeys
    {
        /// <summary>
        ///     The application settings key for the HW Message file
        /// </summary>
        public const string HW_MessageFileKey = "HW_MessageFile";

        /// <summary>
        ///     The application settings key for the HW Message file
        /// </summary>
        public const string Fleet_File = "Fleet_File";

        /// <summary>
        ///     The application settings key for the API URL
        /// </summary>
        public const string WebAPIUrlKey = "WebAPIURL";
    }

    /// <summary>
    /// Contains the error codes for the application
    /// </summary>
    public class ErrorCodes
    {
        /// <summary>
        ///     The General Error error code
        /// </summary>
        public const string GeneralError = "general-error";

        /// <summary>
        ///     TheFile Settings error code
        /// </summary>
        public const string HW_MessageFileSettingsKeyError = "message-file-settings-error";

        /// <summary>
        ///     The HW_Message File error code
        /// </summary>
        public const string HW_MessageFileError = "message-file-error";
    }
}