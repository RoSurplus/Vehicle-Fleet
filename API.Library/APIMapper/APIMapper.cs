using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Library.APIModels;

namespace API.Library.APIMappers
{

    /// <summary>
    ///     Mapper service interface for mapping types for the data service
    /// </summary>
    public interface IHW_Mapper
    {
        /// <summary>
        ///     Maps a string to a TodaysData model
        /// </summary>
        /// <param name="input">The input</param>
        /// <returns>A TodaysData model</returns>
        HW_Message StringToHW_Message(string input);
    }

    /// <summary>
    ///     Mapper service for mapping types for the data service
    /// </summary>
    public class HW_Mapper : IHW_Mapper
    {
        /// <summary>
        ///     Maps a string to a HW_Message model
        /// </summary>
        /// <param name="input">The input</param>
        /// <returns>A HW_Message model</returns>
        public HW_Message StringToHW_Message(string input)
        {
            string current_text = null;
            if (!string.IsNullOrEmpty(input))
                current_text = string.Format(input, DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
            else
                current_text = input;

            return new HW_Message { Data = current_text };
        }
    }
}