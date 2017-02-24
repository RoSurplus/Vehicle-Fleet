//-----------------------------------------------------------------------
// <copyright file="SampleAppController.cs" company="Kenneth Larimer">
//  Copyright (c) 2017 All Rights Reserved
//  <author>Kenneth Larimer</author>
// </copyright>
//-----------------------------------------------------------------------

using API.Library.APIResources;
using API.Library.APIServices;
using API.Library.Common;
using API.Library.APIModels;
using SampleApp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml;
using System.Net;
using API.Library.APIAttributes;
using API.Library.APIWrappers;
using API.Library.APIMappers;

namespace SampleApp.Controllers
{
    public class FleetController : Controller
    {
        /// <summary>
        ///     The data service
        /// </summary>
        private readonly IDataService dataService;

        ///// <summary>
        /////     The application settings service
        ///// </summary>
        //private readonly IAppSettings appSettings;

        ///// <summary>
        /////     The DateTime wrapper
        ///// </summary>
        //private readonly IDateTime dateTimeWrapper;

        ///// <summary>
        /////     The File IO service
        ///// </summary>
        //private readonly IFileIOService fileIOService;

        ///// <summary>
        /////     The Mapper
        ///// </summary>
        //private readonly IHW_Mapper HW_Mapper;

        public FleetController()
        {
            this.dataService = null;
//            this.dataService = new HW_DataService(new AppSettings(), new IDateTime(), new IFileIOService(), IHW_Mapper);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SampleAppController" /> class.
        /// </summary>
        /// <param name="dataService">The injected data service</param>
//        public HW_MessageController(IDataService dataService)
        public FleetController(IDataService dataService)
        {
            this.dataService = dataService;
        }

        /// <summary>
        ///     Gets HW_Message value
        /// </summary>
        /// <returns>A HW_Message model containing HW_Message value</returns>
        [WebApiExceptionFilter(Type = typeof(IOException), Status = HttpStatusCode.ServiceUnavailable, Severity = SeverityCode.Error)]
        [WebApiExceptionFilter(Type = typeof(SettingsPropertyNotFoundException), Status = HttpStatusCode.ServiceUnavailable, Severity = SeverityCode.Error)]
        public HW_Message Get()
        {

            HW_Message ret = null;
            ret = this.dataService.GetHW_Message();
            return ret;
        }

        [WebApiExceptionFilter(Type = typeof(IOException), Status = HttpStatusCode.ServiceUnavailable, Severity = SeverityCode.Error)]
        [WebApiExceptionFilter(Type = typeof(SettingsPropertyNotFoundException), Status = HttpStatusCode.ServiceUnavailable, Severity = SeverityCode.Error)]
        public String HW_Load_Fleet_File()
        {
            string ret = null;

            ret = this.dataService.HW_Load_Fleet_File();
            return ret;
        }

        public static Fleet session_fleet = new Fleet();
        static bool session_fleet_file_invalid = false;

        public void Add_Vehicle(Vehicle veh)
        {
            if (session_fleet.get_NextID() > veh.id)
            {
                veh.id = session_fleet.get_NextID();
            }

            if (session_fleet.get_NextID() <= veh.id)
            {
                session_fleet.set_NextID(veh.id + 1);
            }

            session_fleet.FleetList.Add(veh);
        }

        public Fleet Load_Fleet_File()
        {
            session_fleet_file_invalid = false;

            var filePath = ConfigurationManager.AppSettings.Get(AppSettingsKeys.Fleet_File);
            var iFileIO = new TextFileIOService(new ServerHostingEnvironmentService());
            String text = iFileIO.ReadFile(filePath);
            // var text = HW_Load_Fleet_File();

            return Common.FromXml<Fleet>(text);
        }

        public void Save_Fleet_File()
        {
            if (session_fleet_file_invalid)
            {
                var filePath = ConfigurationManager.AppSettings.Get(AppSettingsKeys.Fleet_File);

                var iFileIO = new TextFileIOService(new ServerHostingEnvironmentService());
                iFileIO.WriteFile(filePath, Common.ToXML(session_fleet));
            }
        }

        //public int CheckSum()
        //{
        //    return session_fleet.CheckSum();
        //}

        // GET: Fleet
        public ActionResult Index()
        {
            //            /// <summary>
            //            ///     The Web Service
            //            /// </summary>
            //private readonly IHW_WebService WebService;

            ///// <summary>
            /////     The logger
            ///// </summary>
            //private readonly ILogger logger;

            //ViewBag.Title = "Fleet Page";
            //    this.WebService = WebService;
            //    this.logger = logger;
            ViewBag.Title = "Fleet Page";

            session_fleet = Load_Fleet_File();
            session_fleet.fix_NextID();

            var model = session_fleet; //  Create_Fleet_File();

            return View(model);
        }

        // GET: Fleet/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Fleet/Create
        public ActionResult Create()
        {
            Vehicle viewmodel = new Vehicle();

            var vlist = Enum.GetValues(typeof(v_type)).Cast<v_type>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
                ,
                Selected = (v.ToString() == "CAR")
            }).ToList();

            ViewBag.selectVehicleType = vlist;

            return View(viewmodel);
        }

        // POST: Fleet/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VMake,VModel,VYear,VVIN,VType,VStatus,VRegion,VLocation")] Vehicle model)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(model.VVIN))
            {
                try
                {
                    // TODO: Add insert logic here
                    session_fleet = Load_Fleet_File();
                    session_fleet.fix_NextID();

                    model.id = session_fleet.get_NextID();

                    Add_Vehicle(new Vehicle(model.id, model.VMake, model.VModel, model.VYear, model.VType, model.VVIN,
                        model.VRegion, model.VLocation, model.VStatus));

                    session_fleet_file_invalid = true;

                    return RedirectToAction("Index");
                }
                catch
                {
                    return View(model);
                }
            }
            return View(model);
        }

        // GET: Fleet/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Fleet/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Fleet/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            session_fleet = Load_Fleet_File();
            session_fleet.fix_NextID();

            Vehicle veh = session_fleet.FleetList.FirstOrDefault(x => x.id == id);
            if (veh == null)
            {
                return HttpNotFound();
            }
            return View(veh);
        }

        // POST: Fleet/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            session_fleet = Load_Fleet_File();
            session_fleet.fix_NextID();

            Vehicle veh = session_fleet.FleetList.FirstOrDefault(x => x.id == id);

            try
            {
                session_fleet.FleetList.Remove(veh);
                session_fleet_file_invalid = true;
                Save_Fleet_File();
            }
            catch (Exception)
            {
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //             db.Dispose();
                Save_Fleet_File();
            }
            base.Dispose(disposing);
        }
    }

    //static void Main (string[] args)
    //{
    //    Dal<Supplier> obj = new Dal<Supplier>();
    //    obj.Add(new Supplier());

    //    Dal<int> test = new Dal<int>(); // is a syntax error only when the "where AnyType : class" is part of the Dal definition
    //    test.Add(100);
    //}

    //public class Dal<AnyType> where AnyType : class // so generic can not be an int or string, etc
    //{
    //    public void Add(AnyType obj)
    //    {

    //    }
    //}
    //public class Customer { }
    //public class Supplier { }

    //static void Main(string[] args)
    //{
    //    Check<int> obj = new Check<int>();
    //    bool b1 = obj.Compare(1, 2);

    //    Check<string> objString = new Check<string>();
    //    bool b2 = objString.Compare("shiv", "raju");
    //}

    //public class Check<UNKNOWDATATYPE>
    //{
    //    public bool Compare(UNKNOWDATATYPE i, UNKNOWDATATYPE j)
    //    {
    //        if (i.Equals(j))
    //        {
    //            return false;
    //        }
    //        return false;
    //    }
    //}
}
