//-----------------------------------------------------------------------
// <copyright file="FilterConfig.cs">
//  Copyright (c) 2017 All Rights Reserved
//  <author>Kenneth Larimer</author>
// </copyright>
//-----------------------------------------------------------------------
using System.Web;
using System.Web.Mvc;

namespace SampleApp
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ErrorHandler.AiHandleErrorAttribute());
        }
    }
}
