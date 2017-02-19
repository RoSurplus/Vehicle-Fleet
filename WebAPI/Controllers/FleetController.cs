using API.Library.APIResources;
using API.Library.APIServices;
using API.Library.Common;
using WebAPI.Models;
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

namespace WebAPI.Controllers
{
    public class FleetController : Controller
    {
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

        public void Create_Fleet_File()
        {
            session_fleet = new Fleet();

            Add_Vehicle(new Vehicle(1, "Mercury", "Cougar", "1967", v_type.CAR,
                "qwertqwert01234567890123", "North", "Center", v_status.INREPAIR));
            Add_Vehicle(new Vehicle(2, "GM", "G20", "1971", v_type.VAN,
                "kwertkwert01234567890123", "South", "Branch 3", v_status.STANDBY));
            Add_Vehicle(new Vehicle(3, "Chevy", "S150", "1988", v_type.TRUCK,
                "dwertdwert01234567890123", "East", "Branch 1", v_status.INTRANSIT));
            Add_Vehicle(new Vehicle(4, "Peterbilt", "Semi", "1994", v_type.SEMI,
                "twerttwert01234567890123", "North", "Center", v_status.INREPAIR));
            Add_Vehicle(new Vehicle(5, "Vox", "Trailer", "2011", v_type.TRAILER,
                "pwertpwert01234567890123", "North", "", v_status.INSERVICE));
        }

        public void Load_Fleet_File()
        {
            session_fleet_file_invalid = false;

            var filePath = ConfigurationManager.AppSettings.Get(AppSettingsKeys.Fleet_File);
            var iFileIO = new TextFileIOService(new ServerHostingEnvironmentService());
            String text = iFileIO.ReadFile(filePath);

            session_fleet_file_invalid = false;

            session_fleet = Common.FromXml<Fleet>(text);
            session_fleet.fix_NextID();
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

        public int CheckSum()
        {
            return session_fleet.CheckSum();
        }

        // GET: Fleet
        public ActionResult Index()
        {
            ViewBag.Title = "Fleet Page";

            Load_Fleet_File();

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
                    Load_Fleet_File();

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

            Load_Fleet_File();

            Vehicle veh = session_fleet.FleetList.FirstOrDefault(x => x.id == id);
            if (veh == null)
            {
                return HttpNotFound();
            }
            return View(veh);
            //            return View();
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
            Load_Fleet_File();
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
}
