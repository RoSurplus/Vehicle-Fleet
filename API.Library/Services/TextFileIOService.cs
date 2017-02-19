//-----------------------------------------------------------------------
// <copyright file="TextFileIOService.cs" company="Kenneth Larimer">
//  Copyright (c) 2017 All Rights Reserved
//  <author>Kenneth Larimer</author>
// </copyright>
//-----------------------------------------------------------------------

namespace API.Library.Services
{
    using System;
    using System.IO;
    using APIResources;

    ///// <summary>
    /////     Service for text file IO
    ///// </summary>
    //public class TextFileIOService : IFileIOService
    //{
    //    /// <summary>
    //    ///     The hosting environment service
    //    /// </summary>
    //    private readonly IHostingEnvironmentService hostingEnvironmentService;

    //    /// <summary>
    //    ///     Initializes a new instance of the <see cref="TextFileIOService" /> class.
    //    /// </summary>
    //    /// <param name="hostingEnvironmentService">The injected hosting environment service</param>
    //    public TextFileIOService(IHostingEnvironmentService hostingEnvironmentService)
    //    {
    //        this.hostingEnvironmentService = hostingEnvironmentService;
    //    }

    //    /// <summary>
    //    ///     Reads the specified file
    //    /// </summary>
    //    /// <param name="filePath">File path</param>
    //    /// <returns>The contents of the file</returns>
    //    public string ReadFile(string filePath)
    //    {
    //        string fileContents;

    //        // Map path to server path
    //        var serverPath = this.hostingEnvironmentService.MapPath(filePath);

    //        // Read the contents of the file
    //        try
    //        {
    //            using (var reader = new StreamReader(serverPath))
    //            {
    //                fileContents = reader.ReadToEnd();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            // Throw an IO exception
    //            throw new IOException(ErrorCodes.HW_MessageFileError, new IOException("There was a problem reading the data file!", ex));
    //        }

    //        return fileContents;
    //    }
    //}
}