//-----------------------------------------------------------------------
// <copyright file="IHostingEnvironmentService.cs" company="Kenneth Larimer">
//  Copyright (c) 2017 All Rights Reserved
//  <author>Kenneth Larimer</author>
// </copyright>
//-----------------------------------------------------------------------

namespace API.Library.Services
{
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
}