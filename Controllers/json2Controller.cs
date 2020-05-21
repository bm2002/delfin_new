using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Delfin.Shared.Data;
using Newtonsoft.Json;
using qiwi.Filters;
using qiwi.Models;

namespace qiwi.Controllers
{
    [log]
    public class json2Controller : Controller
    {
        MTPricesDataContext db = new MTPricesDataContext(ConfigurationManager.ConnectionStrings["apiDatabase"].ToString());
        

        public ActionResult Index()
        {
            return View();
        }

        //[log]
        public JsonResult Get_Dlf_Active_Tour()
        {
            var t = (from tt in db.TP_Tours
                     join tl in db.TP_Lists on tt.TO_Key equals tl.TI_TOKey
                     join td in db.TP_TurDates on tt.TO_Key equals td.TD_TOKey
                     where td.TD_Date >= DateTime.Now
                     select tt.TO_TRKey)
                     .Distinct()
                     .ToList();

            return Json(t, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}