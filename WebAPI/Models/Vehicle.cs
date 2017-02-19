using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using API.Library.Common;
using System.Web;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace SampleApp.Models
{
    public class Fleet
    {
        public List<Vehicle> FleetList { set; get; }
        private static int nextId { set; get; }

        public Fleet()
        {
            FleetList = new List<Vehicle>();
            nextId = -1;
        }

        public void Add(Vehicle v)
        {
            if (nextId <= v.id) nextId = v.id + 1;
            this.Add(v);
        }

        public void set_NextID(int nxt)
        {
            nextId = nxt;
        }

        public void fix_NextID()
        {
            foreach(var item in FleetList)
            {
                if (item.id + 1 > nextId) nextId = item.id + 1;
            }
        }

        public int get_NextID()
        {
            return nextId;
        }

        public int CheckSum()
        {
            int ret = 0;

            foreach(var item in FleetList)
            {
                ret += item.CheckSum();
            }

            return ret;
        }
    }

    public enum v_type {
        [Description("Truck")]
        TRUCK = 1,
        [Description("Van")]
        VAN = 2,
        [Description("Car")]
        CAR = 3,
        [Description("Semi")]
        SEMI = 4,
        [Description("Trailer")]
        TRAILER = 5
    };

    public enum v_status
    {
        [Description("stand-by")]
        STANDBY = 1,
        [Description("in transit")]
        INTRANSIT = 2,
        [Description("in service")]
        INSERVICE = 3,
        [Description("in repair")]
        INREPAIR = 4,
    };

    public enum v_location
    {
        [Description("Distribution Center")]
        DCENTER = 1,
        [Description("Branch")]
        BRANCH = 2,
    }

    public class Vehicle
    {
        [Display(Name = "Application ID")]
        public int id { set; get; }

        [Display(Name = "Make")]
        public string VMake { set; get; }

        [Display(Name = "Model")]
        public string VModel { set; get; }

        [Display(Name = "Model Year")]
        public string VYear { set; get; }

        [Display(Name = "VIN")]
        [VIN_Format]
        public string VVIN { set; get; }

        [Display(Name = "Status")]
        public v_status? VStatus { set; get; }

        [Display(Name = "Distribution Center")]
        [Required(ErrorMessage = "A Distribution Center is Required.")]
        public string VRegion { set; get; }

        [Display(Name = "Location")]
        public string VLocation { set; get; }

        [Display(Name = "Type")]
        public v_type VType { set; get; }

        //[Display(Name = "Type")]
        //public string TName { set; get; }

        public Vehicle()
        {
            id = -1;

        }

        public Vehicle(int Id, string Make, string Model, string Year, v_type Type, string Vin,
            string Region, string Location, v_status? Status = v_status.STANDBY)
        {
            id = Id;
            VMake = Make;
            VModel = Model;
            VYear = Year;
            VType = Type;
            VVIN = Vin;
            VStatus = Status;
            VRegion = Region;
            VLocation = Location;
//            TName = VType.ToDescription();
        }

        public int CheckSum()
        {
            int ret = 0;

            ret += VMake.CalCheckSum();
            ret += VModel.CalCheckSum();
            ret += VYear.CalCheckSum();
            ret += VType.ToDescription().CalCheckSum();
            ret += VVIN.CalCheckSum();
            ret += VRegion.CalCheckSum();
            ret += VLocation.CalCheckSum();
            ret += VStatus.ToString().CalCheckSum();

            return ret;
        }
    }

    public class VIN_Format : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult("Can not be Blank", new[] { validationContext.MemberName });
            }

            Regex test0 = new Regex("([A-Za-z0-9]*)$");
            Match m = test0.Match(value.ToString());

            if (value.ToString() != m.ToString())
            {
                return new ValidationResult("must be only alphanumeric characters", new[] { validationContext.MemberName });
            }

            if (!(m.Success && value.ToString().Length == 24))
            {
                return new ValidationResult("Must be 24 alphanumeric characters", new[] { validationContext.MemberName });
            }

            Regex test1 = new Regex("^(?:(?<ch>[A-Za-z])|(?<num>[0-9])){8,20}$");
            string first = value.ToString().Substring(0,19);
            m = test1.Match(first);

            var c = m.Groups["ch"].Captures.Count;
            //var n = m.Groups["num"].Captures.Count;

            if (!(m.Success && m.Groups["ch"].Captures.Count >= 8))
            {
                // It's a VIN
                return new ValidationResult("The first 19 must have a minimum of 8 alphas", new[] { validationContext.MemberName });
            }

            Regex test2 = new Regex("[0-9]{5,5}$");
            string second = value.ToString().Substring(19, 5);
            m = test2.Match(second);

            if (!(m.Success))
            {
                // It's a VIN
                return new ValidationResult("the last 5 must be numeric", new[] { validationContext.MemberName });
            }

            //Get a list of all properties that are marked with [VIN_Format]
            var props = validationContext.ObjectInstance.GetType().GetProperties().Where(
                prop => Attribute.IsDefined(prop, typeof(VIN_Format)));

            // var values = new HashSet<string>();

            ////Read the values of all other properties
            //foreach (var prop in props)
            //{
            //    var pValue = (string)prop.GetValue(validationContext.ObjectInstance);
            //    if (prop.Name != validationContext.MemberName && !values.Contains(pValue))
            //    {
            //        values.Add(pValue);
            //    }
            //}

            ////                if (db.LookupApplicationStatusModels.Where(x => x.Name == value).FirstOrDefault() != null)

            //if (values.Contains(value))
            //{
            //    return new ValidationResult("is a Duplicate Entry", new[] { validationContext.MemberName });
            //}
            return null;
        }
    }
}