using delfin.mvc.api.models.hotel1c;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;

namespace delfin.mvc.api.Controllers
{
    public class hotel1cController : Controller
    {
        // GET: hotel1c
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult test() 
        {
            return Json(new { test = "test"}, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getHotelList()
        {
            BasicHttpBinding myBinding = new BasicHttpBinding();
            myBinding.Security.Mode = BasicHttpSecurityMode.None;
            EndpointAddress myEndpointAddress = new EndpointAddress("http://93.174.51.131:27385/dolphin/ws/1CHotelReservationInterfaces.1cws");
            myBinding.MaxReceivedMessageSize = 2147483647;
            ReservationInterfacesPortTypeClient client = new ReservationInterfacesPortTypeClient(myBinding, myEndpointAddress);

            var hotellist = client.GetHotelsList("", "");
            return Json(hotellist, JsonRequestBehavior.AllowGet);
        }
    }
}