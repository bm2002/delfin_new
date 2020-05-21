using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Delfin.Shared.Data;
using Newtonsoft.Json;
using qiwi.Models;

namespace qiwi.Controllers
{
    public class json2Controller : Controller
    {
        MTPricesDataContext db = new MTPricesDataContext(ConfigurationManager.ConnectionStrings["apiDatabase"].ToString());
        logContext log = new logContext();

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Get_Dlf_Active_Tour()
        {
            var d = DateTime.Now;

            var t = (from tt in db.TP_Tours
                     join tl in db.TP_Lists on tt.TO_Key equals tl.TI_TOKey
                     join td in db.TP_TurDates on tt.TO_Key equals td.TD_TOKey
                     where td.TD_Date >= DateTime.Now
                     select tt.TO_TRKey).Distinct().ToList();

            log.RequestDumps2.Add(new RequestDump2
            {
                ProviderId = 201,
                Url = Request.Url.AbsoluteUri,
                Success = true,
                Duration = DateTime.Now - d,
                Response = JsonConvert.SerializeObject(t),
                ClientIP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.UserHostAddress,
                Created = DateTime.Now,
                Label = "Get_Dlf_Active_Tour"
            });
            log.SaveChanges();

            return Json(t, JsonRequestBehavior.AllowGet); 
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}