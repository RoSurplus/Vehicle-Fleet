//-----------------------------------------------------------------------
// <copyright file="IConsoleApp.cs">
//  Copyright (c) 2015 All Rights Reserved
//  <author>Kenneth Larimer</author>
// </copyright>
//-----------------------------------------------------------------------

namespace ConsoleApp.Application
{
    /// <summary>
    ///     Console Application
    /// </summary>
    public interface IConsoleApp
    {
        /// <summary>
        ///     Runs the main Console Application
        /// </summary>
        /// <param name="arguments">The command line arguments.</param>
        void Run(string[] arguments);
    }
}