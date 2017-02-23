//-----------------------------------------------------------------------
// <copyright file="IHW_WebService.cs">
//  Copyright (c) 2015 All Rights Reserved
//  <author>Kenneth Larimer</author>
// </copyright>
//-----------------------------------------------------------------------

namespace SampleApp.Services
{
    using API.Library.APIModels;
    using System;

    /// <summary>
    ///     Service for communicating with the Web API
    /// </summary>
    public interface IHW_WebService
    {
        /// <summary>
        ///     Gets HW_Message's data from the web API
        /// </summary>
        /// <returns>A HW_Message model containing Message's data</returns>
        HW_Message GetHW_Message();

        /// <summary>
        ///     Gets FleetFile's data from the web API
        /// </summary>
        /// <returns>A HW_Message model containing Message's data</returns>
//        HW_Message HW_Load_Fleet_File();
    }
}