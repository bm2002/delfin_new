using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Delfin.Shared.Data;
using Newtonsoft.Json;
using qiwi.Filters;
using qiwi.Models;
using Delfin.Shared2.Data;
using System.Web.Helpers;
using System.Globalization;
using Newtonsoft.Json.Linq;
using System.Data.Linq;
using System.Net;
using System.Xml;
using System.IO;
using System.Text;
using Delfin.Data;
using delfin.mvc.api.Models;
using static delfin.mvc.api.Models.orderInfoResponse;
using System.Threading.Tasks;
using delfin.mvc.api.Models.gosha;
using Microsoft.EntityFrameworkCore;
using Delfin.Shared;

//[Route("/json2/subagents")]
namespace qiwi.Controllers
{
    
    [log]
    public class SubagentsController : Controller
    {
        MTPricesDataContext db = new MTPricesDataContext(ConfigurationManager.ConnectionStrings["apiDatabase"].ToString());
        MegatecDBDataContext db2 = new MegatecDBDataContext(ConfigurationManager.ConnectionStrings["masterTour"].ToString());

        CultureInfo cultureinfo = new CultureInfo("ru-RU");
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

        public JsonResult Get_Dlf_Tour_Hotel_HR_Pan()
        {
            JsonResult content = null; 

            content = Json((from tpt in db.TP_Tours
                     join costs in db.tbl_Costs on tpt.TO_TRKey equals costs.CS_PKKEY
                     join hr in db.HotelRooms on costs.CS_SUBCODE1 equals hr.HR_KEY
                     where (costs.CS_DATEEND ?? DateTime.Now.AddYears(-10)) >= DateTime.Now
                     select new gdthPan { master_hotel_id = costs.CS_CODE, master_tour_id = tpt.TO_TRKey, master_pansion_id = (int)costs.CS_SUBCODE2, master_room_category_id = hr.HR_RCKEY, master_room_id = hr.HR_RMKEY }).Distinct().ToList(), JsonRequestBehavior.AllowGet);
            content.MaxJsonLength = int.MaxValue; 
            return content;
        } 

        public JsonResult Rooms()
        {
            JsonResult content = null;
            content = Json(db.Rooms.Select(x => new dictionaryjson { Id = x.RM_KEY, Name = x.RM_NAME }).Distinct().ToList(), JsonRequestBehavior.AllowGet);
            content.MaxJsonLength = int.MaxValue;
            return content;
        }

        public JsonResult RoomCategories()
        {
            JsonResult content = null;
            content = Json(db.RoomsCategories.Select(x => new dictionaryjson { Id = x.RC_KEY, Name = x.RC_NAME }).Distinct().ToList(), JsonRequestBehavior.AllowGet);
            content.MaxJsonLength = int.MaxValue;
            return content;
        }

        //public JsonResult ToursDuration()


        [HttpPost]
        public JsonResult ToursDuration(reqTourDuration req)
        {
            //var req = JsonConvert.DeserializeObject<reqTourDuration>("{\"tourkeys\": [3278],\"date\": \"01.12.2020\"}");

            using (var gosha = new goshaContext())
            {
                //var toKey = gosha.TpTours.SingleOrDefault(x => x.ToTrkey == req.tourkeys[0]).ToKey;
                
                var toKeys = gosha.TpTours.Where(x => req.tourkeys.Contains(x.ToTrkey)).Select(x => x.ToKey).ToList();

                var Lists = gosha.TpLists
                    .Where(x => toKeys.Contains(x.TiTokey))
                    .ToList();

                var Prices = gosha.TpPrices
                    .Where(x => toKeys.Contains(x.TpTokey) && x.TpDateBegin == DateTime.Parse(req.date, cultureinfo))
                    .ToList();

                var obj = (from lists in Lists
                         join prices in Prices on lists.TiKey equals prices.TpTikey
                         select lists.TiDays -1)
                        .Distinct()
                        .OrderBy(x => x)
                        .ToList();

                JsonResult content = null;
                content = Json(obj);
                content.MaxJsonLength = int.MaxValue;
                return content;
            }
        }

        public JsonResult TourRestrictions(int id)
        {
            using (var gosha = new goshaContext())
            {
                var toKey = gosha.TpTours.SingleOrDefault(x => x.ToTrkey == id).ToKey;

                var tourNights = gosha.TpLists
                    .Where(x => x.TiTokey == toKey)
                    //.Include(x => x.TpPrices)
                    .Select(x => x.TiDays - 1)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList();

                var tourDates = gosha.TpTurDates
                    .Where(x => x.TdTokey == toKey && x.TdDate >= DateTime.Now.Date)
                    .Select(x => x.TdDate)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList();


                List<Period> periods = new List<Period>();
                var dFirst = tourDates.Min();
                var dLast = tourDates.Min().AddDays(-1);
                foreach (var d in tourDates)
                {
                    if (d.AddDays(-1) != dLast)
                    {
                        periods.Add(new Period
                        {
                            Start = dFirst.ToString("dd.MM.yyyy"),
                            Stop = dLast.ToString("dd.MM.yyyy")
                        });
                        dFirst = d;
                    }
                    dLast = d;
                }

                periods.Add(new Period
                {
                    Start = dFirst.ToString("dd.MM.yyyy"),
                    Stop = dLast.ToString("dd.MM.yyyy")
                });

                JsonResult content = null;
                content = Json(Tuple.Create(tourNights, periods), JsonRequestBehavior.AllowGet);
                content.MaxJsonLength = int.MaxValue;
                return content;
            }
        }

        public JsonResult GetCourses()
        {
            JsonResult content = null;
            var courses = db.dlf_RealCourses
                .Where(x => x.RC_DATEBEG >= DateTime.Now.Date)
                .ToList();
            content = Json(courses
                .Select(x => new { 
                    RC_RCOD1 = x.RC_RCOD1,
                    RC_RCOD2 = x.RC_RCOD2,
                    RC_DATEBEG = ((DateTime)x.RC_DATEBEG).ToString("dd.MM.yyyy"),
                    RC_DATEEND = ((DateTime)x.RC_DATEEND).ToString("dd.MM.yyyy"),
                    RC_Key = x.RC_Key,
                    RC_COURSE = x.RC_COURSE,
                    RC_COURSE_CB = x.RC_COURSE_CB
                })
                .ToList(), JsonRequestBehavior.AllowGet);
            content.MaxJsonLength = int.MaxValue;
            return content;
        }

        public JsonResult HotelContent(int id)
        {
            JsonResult content = null;

            var ppt_Hotel = db.ppt_Hotels.Where(x => x.HotelId == id).SingleOrDefault();

            var ppt_HotelContent = db.ppt_HotelContents.Where(x => x.Id == ppt_Hotel.PPTHotelId).SingleOrDefault();

            var ppt_HotelsGeo = db.ppt_HotelsGeos.Where(x => x.Id == ppt_Hotel.PPTHotelId).SingleOrDefault();

            var ppt_HotelType = db.ppt_HotelTypes.Where(x => x.PPT_Id == ppt_HotelContent.PPT_HotelTypeId).SingleOrDefault();

            var tours = (from ppt_ToursToHotels in db.ppt_ToursToHotels.Where(x => x.HotelId == ppt_Hotel.PPTHotelId)
                         join ppt_Tours in db.ppt_Tours on ppt_ToursToHotels.TourId equals ppt_Tours.Id
                         select new { ppt_Tours.MTId, ppt_Tours.Name }).Distinct().ToArray();

            var photos = db.ppt_AllPhotos.Where(x => x.HotelId == ppt_Hotel.PPTHotelId && x.Type == "Hotel" && !x.RefStr.ToLower()
                                            .Contains("unsorted")).OrderByDescending(x => x.Hotel_view).ToArray();

            var apiHotelDescription = new hotelcontent2
            {
                Name = ppt_HotelContent.Title,
                NameAdd = db.HotelDictionaries.SingleOrDefault(x => x.HD_KEY == id).HD_NAME.Replace("\"", ""),
                Id = ppt_Hotel.HotelId,
                Stars = ppt_HotelContent.Level,
                Stars_www = ppt_HotelContent.Stars ?? 0,
                Type = ppt_HotelType != null ? ppt_HotelType.Title : "",
                Contacts = "",
                Adress = ppt_HotelContent.Address_full,
                Longitude = ppt_HotelContent.GeoLongitude,
                Latitude = ppt_HotelContent.GeoLatitude,
                Country = (int)(ppt_HotelsGeo.CountryId ?? 0),
                Region = (int)(ppt_HotelsGeo.RegionId ?? 0),
                City = (int)(ppt_HotelsGeo.CityId ?? 0),
                Distinct = (int)(ppt_HotelsGeo.DistinctId ?? 0),
                CheckinTime = ppt_HotelContent.checkin_time,
                CheckoutTime = ppt_HotelContent.checkout_time,
                CheckinServiceMode = ppt_HotelContent.checkin_service_mode == "all24" ? "24/7" : ppt_HotelContent.checkin_service_mode,
                ConstructionInfo = ppt_HotelContent.Construction,
                Info = ppt_HotelContent.Info,
                Services = new string[] { },
                TransportAccessability = ppt_HotelContent.Transport,
                Tours = tours.Select(x => new dictionaryjson { Id = x.MTId, Name = x.Name }).ToArray(),
                Photos200x150 = photos.Select(x => "https://allkurort.su/system/hotels/" + ppt_Hotel.PPTHotelId.ToString() + "/photos/200x150/" + x.Name).ToArray(),
                Photos800x600 = photos.Select(x => "https://allkurort.su/system/hotels/" + ppt_Hotel.PPTHotelId.ToString() + "/photos/800x600/" + x.Name).ToArray(),
                Photos1020x700 = photos.Select(x => "https://allkurort.su/system/hotels/" + ppt_Hotel.PPTHotelId.ToString() + "/photos/1020x700/" + x.Name).ToArray(),
                CheckinServiceFrom = ppt_HotelContent.checkin_service_from,
                CheckinServiceTill = ppt_HotelContent.checkin_service_till
            };

            content = Json(apiHotelDescription, JsonRequestBehavior.AllowGet);
            content.MaxJsonLength = int.MaxValue;
            return content;
        }

        public JsonResult TourContent(int id)
        {
            JsonResult content = null;

            var tour = db.ppt_Tours.Where(x => x.MTId == id).SingleOrDefault();

            var rate = db.tbl_TurLists.Where(x => x.TL_KEY == id).Select(x => x.TL_RATE).SingleOrDefault();

            var hdkeys = (from hotelstomaster in db.ppt_hotelsToMasters
                          join tpservices in db.TP_Services.Where(x => x.TS_SVKey == 3) on hotelstomaster.masterId equals tpservices.TS_Code
                          where tpservices.TS_OpPacketKey == id
                          select (int)hotelstomaster.masterId).Distinct().ToArray();

            var programs = db.ppt_TourPrograms.Where(x => x.PptTourId == tour.Id).ToList();

            Parts parts = new Parts();
            parts.Case = programs.Select(x => (int)(x.WeekdayNumber ?? 0)).Sum() == 0 ? "ByRotation" : "ByWeekday";

            List<qiwi.Models.Field> fields = new List<qiwi.Models.Field>();

            foreach (var program in programs.OrderBy(x => x.Idx))
            {
                fields.Add(new qiwi.Models.Field
                {
                    DayOfWeek = (int)(program.WeekdayNumber ?? 0),
                    Info = new Info
                    {
                        //Title = program.Name,
                        Detail = program.Name,
                        Areas = program.AreaIds == "" ? new string[] { } : db.ppt_areas.Where(x => program.AreaIds.Split(' ').Select(y => int.Parse(y)).Contains(x.id)).Select(x => x.title).ToArray(),
                        Landmarks = program.LandmarkIds == "" ? new string[] { } : db.ppt_pois.Where(x => program.LandmarkIds.Split(' ').Select(y => int.Parse(y)).Contains(x.Id)).Select(x => x.Title).ToArray()
                    }
                });
            }

            parts.Fields = new qiwi.Models.Field[][] { fields.ToArray() };

            var attr = (from ta in db._TourBitAttrs
                        join ba in db._BitAttrs on ta.ID_BitAttr equals ba.ID_BitAttr
                        where ta.ID_Tour == id && (new List<int> { 11, 13, 15, 17, 60, 62, 63 }).Contains(ba.ID_BitAttr)
                        select new attr { Id = ba.ID_BitAttr, Name = ba.HumanName }).ToArray();
            
            content = Json(new tourcontent
            {
                Hotels = hdkeys,
                Id = id,
                RequiredCharges = tour.RequiredCharges,
                CancellationInfo = tour.Cancellation,
                ImportantInfo = tour.Important,
                Venue = tour.MeetingPoint,
                Name = tour.Name,
                PaidFor = tour.PaidFor,
                WarnInfo = tour.Warning,
                Parts = parts,
                TourAttributes = attr,
                Rate = rate
            }, JsonRequestBehavior.AllowGet);

            content.MaxJsonLength = int.MaxValue;
            return content;
        }

        public JsonResult HotelsIdsNew(string tour_type)
        {
            JsonResult content = null;

            var h1 = (from tlists in db.TP_Lists
                      join tptours in db.TP_Tours on tlists.TI_TOKey equals tptours.TO_Key
                      join tours in db.tbl_TurLists on tptours.TO_TRKey equals tours.TL_KEY
                      join ppthots in db.ppt_hotelsToMasters on tlists.TI_FIRSTHDKEY equals ppthots.masterId
                      join pptgeos in db.ppt_HotelsGeos on ppthots.pptId equals pptgeos.Id
                      where tlists.TI_FIRSTHDKEY != null && (pptgeos.CountryId ?? 0) != 0
                            && (tptours.TO_DateEnd >= DateTime.Now || tptours.TO_DateValid >= DateTime.Now)
                      select Tuple.Create(tlists.TI_FIRSTHDKEY, tours.TL_TIP)).Distinct().ToList();

            if ((tour_type ?? "") != "") h1 = h1.Where(x => tour_type.Split(',').Select(y => int.Parse(y)).Contains(x.Item2)).ToList();

            var h2 = (from tlists in db.TP_Lists
                      join tptours in db.TP_Tours on tlists.TI_TOKey equals tptours.TO_Key
                      join tours in db.tbl_TurLists on tptours.TO_TRKey equals tours.TL_KEY
                      join ppthots in db.ppt_hotelsToMasters on tlists.TI_SecondHDKey equals ppthots.masterId
                      join pptgeos in db.ppt_HotelsGeos on ppthots.pptId equals pptgeos.Id
                      where tlists.TI_SecondHDKey != null && (pptgeos.CountryId ?? 0) != 0
                            && (tptours.TO_DateEnd >= DateTime.Now || tptours.TO_DateValid >= DateTime.Now)
                      select Tuple.Create(tlists.TI_SecondHDKey, tours.TL_TIP)).Distinct().ToList();

            if ((tour_type ?? "") != "") h2 = h2.Where(x => tour_type.Split(',').Select(y => int.Parse(y)).Contains(x.Item2)).ToList();

            content = Json(h1.Union(h2).Select(x => (int)x.Item1).Distinct().OrderBy(x => x).ToArray(), JsonRequestBehavior.AllowGet);

            content.MaxJsonLength = int.MaxValue;
            return content;
        }

        public JsonResult HotelsQuotas(int Hotel, string SDate, string EDate)
        {
            JsonResult content = null;

            Nullable<DateTime> d1 = null;
            if (SDate != null) d1 = DateTime.Parse(SDate, cultureinfo);
            Nullable<DateTime> d2 = null;
            if (EDate != null) d2 = DateTime.Parse(EDate, cultureinfo);
            int? hd = null;
            if (Hotel != 0) hd = (int)Hotel;

            var result = db._dlfHotelsQuotas(hd, d1, d2).ToList();

            List<hotelquota> hotelquotes = new List<hotelquota>();
            foreach (var item in result)
            {
                hotelquotes.Add(new hotelquota
                {
                    ByRoom = item.ByRoom,
                    Duration = (int?)item.Duration,
                    EndDate = ((DateTime)item.EndDate).ToString("dd.MM.yyyy"),
                    Hotel = (int?)item.Hotel,
                    Quota = item.Quota,
                    ReleasePeriod = (int?)item.ReleasePeriod,
                    Room = (int?)item.Room,
                    RoomCat = (int?)item.RoomCat,
                    StartDate = ((DateTime)item.StartDate).ToString("dd.MM.yyyy"),
                });
            }

            content = Json(hotelquotes, JsonRequestBehavior.AllowGet);

            content.MaxJsonLength = int.MaxValue;
            return content;
        }


        public JsonResult HotelsStopSales(int Hotel, string SDate, string EDate)
        {
            JsonResult content = null;

            cultureinfo = new CultureInfo("ru-ru");
            Nullable<DateTime> d1 = null;
            if (SDate != null) d1 = DateTime.Parse(SDate, cultureinfo);
            Nullable<DateTime> d2 = null;
            if (EDate != null) d2 = DateTime.Parse(EDate, cultureinfo);
            int? hd = null;
            if (Hotel != 0) hd = (int)Hotel;

            var result = db._dlfHotelsStopSales(hd, d1, d2).ToList();

            var hotstop = new List<hotelstop>();
            foreach (var item in result)
            {
                hotstop.Add(new hotelstop
                {
                    Hotel = (int)item.Hotel,
                    Room = (int)item.Room,
                    RoomCat = (int)item.RoomCat,
                    StartStop = ((DateTime)item.StartStop).ToString("dd.MM.yyyy"),
                    EndStop = ((DateTime)item.EndStop).ToString("dd.MM.yyyy")
                    //period = new Period((DateTime)item.StartStop, (DateTime)item.EndStop)
                });
            }

            content = Json(hotstop, JsonRequestBehavior.AllowGet);

            content.MaxJsonLength = int.MaxValue;
            return content;
        }

        public JsonResult UserInfo(int US_KEY)
        {
            JsonResult content = null;
            
            var ui = (from ulist in db.DUP_USERs
                      join prt in db.tbl_Partners on ulist.US_PRKEY equals prt.PR_KEY
                      where ulist.US_KEY == US_KEY
                      select new UserInfo
                      {
                          fullname = prt.PR_FULLNAME == null || prt.PR_FULLNAME == "" ? ulist.US_COMPANYNAME : prt.PR_FULLNAME,
                          legaladress = prt.PR_LEGALADDRESS == null || prt.PR_LEGALADDRESS == "" ? prt.PR_ADRESS : prt.PR_LEGALADDRESS
                      }).ToList();

            content = Json(ui[0], JsonRequestBehavior.AllowGet);

            content.MaxJsonLength = int.MaxValue;
            return content;
        }

        public JsonResult AcademHotels(int area)
        {
            JsonResult content = null;

            var query = (from ic in db.ImportedContents
                         where ic.Descriptor == 12 && ic.Provider == 10 && ic.Reference == 1
                         select ic.Content).ToList();

            string jsontext = query[0].ToString();

            var objects = JArray.Parse(jsontext);

            List<AcademKurortCity> academKurortCities = new List<AcademKurortCity>();
            foreach (var item in objects) academKurortCities.Add(JsonConvert.DeserializeObject<AcademKurortCity>(item.ToString()));

            var hotels = new List<ExternHotelItem>();

            foreach (var academKurortCity in academKurortCities)
            {
                if (academKurortCity.Item1 != area) continue;

                int accity = academKurortCity.Item2;

                string url = "https://www.acase.ru/xml/form.jsp";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = "POST";
                string postData = string.Format("CompanyId=DELFIN-CENTRE&UserId=XML&Password=83finled&CityCode={0}&RequestName=HotelListRequest&PagesForm=Yes&Language=ru", accity);
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                dataStream = response.GetResponseStream();
                XmlTextReader xmlreader = new XmlTextReader(dataStream);
                xmlreader.WhitespaceHandling = WhitespaceHandling.None;

                string hotelname = "";
                Int32 id = 0;
                while (xmlreader.Read())
                {
                    if (xmlreader.NodeType == XmlNodeType.Element && xmlreader.Name == "Hotel")
                    {
                        id = Convert.ToInt32(xmlreader.GetAttribute("Code"));
                        hotelname = xmlreader.GetAttribute("Name") + " (" + xmlreader.GetAttribute("Address") + ")";
                    }
                    else if (xmlreader.NodeType == XmlNodeType.Element && xmlreader.Name == "FreeSale")
                    {
                        hotels.Add(new ExternHotelItem 
                        {
                            HotelId = id,
                            HotelName = hotelname,
                            PPTAreaId = area,
                            HasFreeSale = (xmlreader.GetAttribute("Code") == "1") ? true : false
                        });
                            //id, hotelname, area, (xmlreader.GetAttribute("Code") == "1") ? true : false);
                    }

                }
                xmlreader.Close(); 
            }

            content = Json(hotels.ToArray(), JsonRequestBehavior.AllowGet);

            content.MaxJsonLength = int.MaxValue;
            return content;
        }

        public JsonResult GetOrderInfo(string auth_token, string email, string order)
        {
            JsonResult content = null;

            bool agency = (auth_token ?? "") != "";

            int partnerkey = 0;
            string operatr = "";
            int opr = 0;

            if (agency)
            {
                try
                {
                    string postdata = JsonConvert.SerializeObject(new Guid(auth_token));
                    string text = lib.getPost("https://www.delfin-tour.ru/auth/byToken", postdata);
                    if (text.Contains("AgencyManager")) partnerkey = (JsonConvert.DeserializeObject<agencylogin.Rootobject>(text)).fields[0].agency.id;
                    if (text.Contains("Operator"))
                    {
                        partnerkey = (JsonConvert.DeserializeObject<Managerlogin.Rootobject>(text)).fields[0].legalEntity;
                        opr = (JsonConvert.DeserializeObject<Managerlogin.Rootobject>(text)).fields[0].id;
                        operatr = (JsonConvert.DeserializeObject<Managerlogin.Rootobject>(text)).fields[0].name;
                    }
                }
                catch { }
            }

            List<dogGOI> doggoi = new List<dogGOI>();
            if (partnerkey == 0 && auth_token != "")
            {
                doggoi.Add(new dogGOI
                {
                    error = "token not valid"
                });
            }
            else
            {
                var dgs = new List<tbl_Dogovor>();
                if (agency)
                {
                    dgs = db2.tbl_Dogovors.Where(x => x.DG_CODE == order).ToList();
                    if (operatr == "") dgs = dgs.Where(x => x.DG_PARTNERKEY == partnerkey).ToList();
                }
                else
                    dgs = db2.tbl_Dogovors.Where(x => x.DG_CODE == order && x.DG_MAINMENEMAIL == email).ToList();

                foreach (var dg in dgs)
                {
                    var trsts = db2.tbl_Turists.Where(x => x.TU_DGCOD == dg.DG_CODE).ToList();

                    var prts = db2.tbl_Partners.Where(X => X.PR_KEY == dg.DG_PARTNERKEY).SingleOrDefault();

                    var dupusers = db2.DUP_USERs.Where(x => x.US_KEY == dg.DG_DUPUSERKEY).ToList();

                    var hdkeys = db2.tbl_DogovorLists.Where(x => x.DL_DGCOD == order && x.DL_SVKEY == 3).Select(x => x.DL_CODE).Distinct().ToArray();

                    foreach (var trst in trsts)
                    {
                        doggoi.Add(new dogGOI 
                        {
                            DG_ENDDATE = dg.DG_TURDATE.AddDays(dg.DG_NDAY - 1).ToString("dd.MM.yyyy", cultureinfo),
                            DG_CNKEY = dg.DG_CNKEY,
                            DG_TURDATE = dg.DG_TURDATE.ToString("dd.MM.yyyy", cultureinfo),
                            DG_CODE = dg.DG_CODE,
                            DG_CTKEY = dg.DG_CTKEY,
                            DG_MAINMENEMAIL = dupusers.Count() == 0 ? dg.DG_MAINMENEMAIL : dupusers[0].US_EMAIL,
                            DG_MAINMENPASPORT = dg.DG_MAINMENPASPORT,
                            DG_MAINMENPHONE = dg.DG_MAINMENPHONE,
                            DG_NMEN = (int)dg.DG_NMEN,
                            DG_TRKEY = dg.DG_TRKEY,
                            PR_CITY = prts.PR_CITY,
                            PR_CTKEY = prts.PR_CTKEY,
                            PR_LEGALADDRESS = prts.PR_LEGALADDRESS,
                            TU_BIRTHDAY = trst.TU_BIRTHDAY != null ? ((DateTime)trst.TU_BIRTHDAY).ToString("dd.MM.yyyy") : "",
                            TU_EMAIL = trst.TU_EMAIL,
                            TU_FNAMELAT = trst.TU_FNAMELAT,
                            TU_FNAMERUS = trst.TU_FNAMERUS,
                            TU_ISMAIN = (int)(trst.TU_ISMAIN ?? 0),
                            TU_NAMELAT = trst.TU_NAMELAT,
                            TU_NAMERUS = trst.TU_NAMERUS,
                            TU_PASPORTDATE = trst.TU_PASPORTDATE,
                            TU_PASPORTDATEEND = trst.TU_PASPORTDATEEND,
                            TU_PASPORTNUM = trst.TU_PASPORTNUM,
                            TU_PASPORTTYPE = trst.TU_PASPORTTYPE,
                            TU_PASPRUDATE = trst.TU_PASPRUDATE,
                            TU_PASPRUNUM = trst.TU_PASPRUNUM,
                            TU_PASPRUSER = trst.TU_PASPRUSER,
                            TU_PHONE = trst.TU_PHONE,
                            TU_PHONECODE = trst.TU_PHONECODE,
                            TU_RealSex = (int)(trst.TU_RealSex ?? 0),
                            TU_SEX = (int)(trst.TU_SEX ?? 0),
                            TU_SNAMELAT = trst.TU_SNAMELAT,
                            TU_SNAMERUS = trst.TU_SNAMERUS,
                            HDKEYS = hdkeys
                        });
                    }
                }          
            }

            content = Json(doggoi, JsonRequestBehavior.AllowGet);

            content.MaxJsonLength = int.MaxValue;
            return content;
        }

        [HttpPost]
        public JsonResult testPost1(int i)
        {
            return Json(i * i);
        }

        [HttpPost]
        public JsonResult searchMinMaxPrice(tourhotelrequest[] req)
        {
            JsonResult content = null;

            List<Tours> tours = new List<Tours>(); 

            if (req == null) req = new tourhotelrequest[0];

            if (req.Length == 0 || req == null)
            {
                tours = (from tptours in db.dlf_TP_Tours
                         select new Tours
                         {
                             Tour = (tptours.TO_TRKey != null) ? (int)tptours.TO_TRKey : 0,
                             Hotel = (tptours.HD_KEY != null) ? (int)tptours.HD_KEY : 0,
                             Min01 = (tptours.MIN_01 != null) ? (int)tptours.MIN_01 : 0,
                             Min02 = (tptours.MIN_02 != null) ? (int)tptours.MIN_02 : 0,
                             Min03 = (tptours.MIN_03 != null) ? (int)tptours.MIN_03 : 0,
                             Min04 = (tptours.MIN_04 != null) ? (int)tptours.MIN_04 : 0,
                             Min05 = (tptours.MIN_05 != null) ? (int)tptours.MIN_05 : 0,
                             Min06 = (tptours.MIN_06 != null) ? (int)tptours.MIN_06 : 0,
                             Min07 = (tptours.MIN_07 != null) ? (int)tptours.MIN_07 : 0,
                             Min08 = (tptours.MIN_08 != null) ? (int)tptours.MIN_08 : 0,
                             Min09 = (tptours.MIN_09 != null) ? (int)tptours.MIN_09 : 0,
                             Min10 = (tptours.MIN_10 != null) ? (int)tptours.MIN_10 : 0,
                             Min11 = (tptours.MIN_11 != null) ? (int)tptours.MIN_11 : 0,
                             Min12 = (tptours.MIN_12 != null) ? (int)tptours.MIN_12 : 0,
                             Max01 = (tptours.MAX_01 != null) ? (int)tptours.MAX_01 : 0,
                             Max02 = (tptours.MAX_02 != null) ? (int)tptours.MAX_02 : 0,
                             Max03 = (tptours.MAX_03 != null) ? (int)tptours.MAX_03 : 0,
                             Max04 = (tptours.MAX_04 != null) ? (int)tptours.MAX_04 : 0,
                             Max05 = (tptours.MAX_05 != null) ? (int)tptours.MAX_05 : 0,
                             Max06 = (tptours.MAX_06 != null) ? (int)tptours.MAX_06 : 0,
                             Max07 = (tptours.MAX_07 != null) ? (int)tptours.MAX_07 : 0,
                             Max08 = (tptours.MAX_08 != null) ? (int)tptours.MAX_08 : 0,
                             Max09 = (tptours.MAX_09 != null) ? (int)tptours.MAX_09 : 0,
                             Max10 = (tptours.MAX_10 != null) ? (int)tptours.MAX_10 : 0,
                             Max11 = (tptours.MAX_11 != null) ? (int)tptours.MAX_11 : 0,
                             Max12 = (tptours.MAX_12 != null) ? (int)tptours.MAX_12 : 0,
                             TI_hdnights = (tptours.TI_hdnights != null) ? (int)tptours.TI_hdnights : 0
                         }).ToList();
            }
            else
            {
                foreach (var rq in req)
                {
                    tours.Add((from tptours in db.dlf_TP_Tours
                               where tptours.TO_TRKey == rq.T && tptours.HD_KEY == rq.H
                               select new Tours
                               {
                                   Tour = (int)tptours.TO_TRKey,
                                   Hotel = (int)tptours.HD_KEY,
                                   Min01 = (tptours.MIN_01 != null) ? (int)tptours.MIN_01 : 0,
                                   Min02 = (tptours.MIN_02 != null) ? (int)tptours.MIN_02 : 0,
                                   Min03 = (tptours.MIN_03 != null) ? (int)tptours.MIN_03 : 0,
                                   Min04 = (tptours.MIN_04 != null) ? (int)tptours.MIN_04 : 0,
                                   Min05 = (tptours.MIN_05 != null) ? (int)tptours.MIN_05 : 0,
                                   Min06 = (tptours.MIN_06 != null) ? (int)tptours.MIN_06 : 0,
                                   Min07 = (tptours.MIN_07 != null) ? (int)tptours.MIN_07 : 0,
                                   Min08 = (tptours.MIN_08 != null) ? (int)tptours.MIN_08 : 0,
                                   Min09 = (tptours.MIN_09 != null) ? (int)tptours.MIN_09 : 0,
                                   Min10 = (tptours.MIN_10 != null) ? (int)tptours.MIN_10 : 0,
                                   Min11 = (tptours.MIN_11 != null) ? (int)tptours.MIN_11 : 0,
                                   Min12 = (tptours.MIN_12 != null) ? (int)tptours.MIN_12 : 0,
                                   Max01 = (tptours.MAX_01 != null) ? (int)tptours.MAX_01 : 0,
                                   Max02 = (tptours.MAX_02 != null) ? (int)tptours.MAX_02 : 0,
                                   Max03 = (tptours.MAX_03 != null) ? (int)tptours.MAX_03 : 0,
                                   Max04 = (tptours.MAX_04 != null) ? (int)tptours.MAX_04 : 0,
                                   Max05 = (tptours.MAX_05 != null) ? (int)tptours.MAX_05 : 0,
                                   Max06 = (tptours.MAX_06 != null) ? (int)tptours.MAX_06 : 0,
                                   Max07 = (tptours.MAX_07 != null) ? (int)tptours.MAX_07 : 0,
                                   Max08 = (tptours.MAX_08 != null) ? (int)tptours.MAX_08 : 0,
                                   Max09 = (tptours.MAX_09 != null) ? (int)tptours.MAX_09 : 0,
                                   Max10 = (tptours.MAX_10 != null) ? (int)tptours.MAX_10 : 0,
                                   Max11 = (tptours.MAX_11 != null) ? (int)tptours.MAX_11 : 0,
                                   Max12 = (tptours.MAX_12 != null) ? (int)tptours.MAX_12 : 0,
                                   TI_hdnights = (tptours.TI_hdnights != null) ? (int)tptours.TI_hdnights : 0
                               }).SingleOrDefault());
                }
            }

            content = Json(tours, JsonRequestBehavior.AllowGet);

            content.MaxJsonLength = int.MaxValue;
            return content;
        }

        public JsonResult Areas(int Country = 0, int Region = 0, int City = 0)
        {
            JsonResult content = null;

            var data = db.ppt_areas.ToList();
            if (Country != 0) data = data.Where(x => x.countryId == Country).ToList();
            if (Region != 0) data = data.Where(x => x.regionId == Region).ToList();
            if (City != 0) data = data.Where(x => x.cityId == City).ToList();

            content = Json(data, JsonRequestBehavior.AllowGet);

            content.MaxJsonLength = int.MaxValue;
            return content;
        }

        public JsonResult empty()
        {
            JsonResult content = null;
          
            //content = Json(, JsonRequestBehavior.AllowGet);

            content.MaxJsonLength = int.MaxValue;
            return content;
        }

        public JsonResult Pansions()
        {
            JsonResult content = null;
            var pcs = db.dlf_PansionClasses.ToList();
            content = Json(db.Pansions.Select(x => 
                          new nodejson
                          {
                              Id = x.PN_KEY,
                              Name = x.PN_NAME,
                              ParentId = x.PN_GlobalCode == null ? -1
                                : db.dlf_PansionClasses.Where(y => y.PC_KEY == Convert.ToInt32(x.PN_GlobalCode)).SingleOrDefault().PC_KEY
                          }).ToList(), JsonRequestBehavior.AllowGet);           
            content.MaxJsonLength = int.MaxValue;
            return content;
        }


        public JsonResult Get_Dlf_Tour_Hotel()
        {
            var t = (from ts in db.TurServices
                     join costs in db.tbl_Costs on new { t1 = ts.TS_PKKEY, t2 = ts.TS_SVKEY } equals new { t1 = costs.CS_PKKEY, t2 = costs.CS_SVKEY }
                     where ts.TS_SVKEY == 3 && ts.TS_PKKEY != 0 && ts.TS_TRKEY != 0 && costs.CS_COST != 0
                             && ((costs.CS_DATEEND ?? DateTime.Now.AddYears(-10)) >= DateTime.Now || (costs.CS_CHECKINDATEEND ?? DateTime.Now.AddYears(-10)) >= DateTime.Now)
                     select new gdth { tour_id = ts.TS_TRKEY, hotel_id = costs.CS_CODE }).Distinct().OrderBy(x => x.tour_id).ThenBy(x => x.hotel_id).ToList();
            //content.MaxJsonLength = int.MaxValue;
            return Json(t, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Get_Dlf_Table(string tablename, long? rowid)
        {
            api2DataContext db2 = new api2DataContext(ConfigurationManager.ConnectionStrings["apiDatabase"].ToString());

            JsonResult content = null;
            //content.MaxJsonLength = int.MaxValue;

            switch (tablename.ToLower())
            {
                case "country":
                    {
                        if ((rowid ?? 0) == 0) content = Json(db2._dlf_Countries.ToList(), JsonRequestBehavior.AllowGet);
                        else content = Json(db2._dlf_Countries.ToList().Where(x => x.ROWID > rowid).ToList(), JsonRequestBehavior.AllowGet);
                        break;
                    }
                case "user": { content = Json(db2.UserLists.ToList(), JsonRequestBehavior.AllowGet); break; }
                case "category": { content = Json(db2.CategoriesOfHotels.ToList(), JsonRequestBehavior.AllowGet); break; }
                case "city":
                    {
                        if ((rowid ?? 0) == 0) content = Json(db2._dlf_CityDictionaries.ToList(), JsonRequestBehavior.AllowGet);
                        else content = Json(db2._dlf_CityDictionaries.ToList().Where(x => x.ROWID > rowid).ToList(), JsonRequestBehavior.AllowGet);
                        break;
                    }
                case "resort":
                    {
                        if ((rowid ?? 0) == 0) content = Json(db2._dlf_Resorts.ToList(), JsonRequestBehavior.AllowGet);
                        else content = Json(db2._dlf_Resorts.ToList().Where(x => x.ROWID > rowid).ToList(), JsonRequestBehavior.AllowGet);
                        break;
                    }
                case "hotel":
                    {
                        if ((rowid ?? 0) == 0) content = Json(db2._dlf_HotelDictionaries.ToList().Select(x => new hotelshort
                        {
                            HD_KEY = x.HD_KEY,
                            HD_ADRESS = x.HD_ADDRESS,
                            HD_CNKEY = x.HD_CNKEY,
                            HD_COHID = x.HD_COHId,
                            HD_CTKEY = x.HD_CTKEY,
                            HD_EMAIL = x.HD_EMAIL,
                            HD_FAX = x.HD_FAX,
                            HD_HTTP = x.HD_HTTP,
                            HD_NAME = x.HD_NAME,
                            HD_PHONE = x.HD_PHONE,
                            HD_RSKEY = x.HD_RSKEY,
                            ROWID = x.ROWID
                        }).ToList(), JsonRequestBehavior.AllowGet);
                        else content = Json(db2._dlf_HotelDictionaries.ToList().Where(x => x.ROWID > rowid).Select(x => new hotelshort
                        {
                            HD_KEY = x.HD_KEY,
                            HD_ADRESS = x.HD_ADDRESS,
                            HD_CNKEY = x.HD_CNKEY,
                            HD_COHID = x.HD_COHId,
                            HD_CTKEY = x.HD_CTKEY,
                            HD_EMAIL = x.HD_EMAIL,
                            HD_FAX = x.HD_FAX,
                            HD_HTTP = x.HD_HTTP,
                            HD_NAME = x.HD_NAME,
                            HD_PHONE = x.HD_PHONE,
                            HD_RSKEY = x.HD_RSKEY,
                            ROWID = x.ROWID
                        }).ToList(), JsonRequestBehavior.AllowGet);
                        break;
                    }
                case "tourservice":
                    {
                        if ((rowid ?? 0) == 0) content = Json(db2._dlf_TurServices.ToList(), JsonRequestBehavior.AllowGet);
                        else content = Json(db2._dlf_TurServices.ToList().Where(x => x.ROWID > rowid).ToList(), JsonRequestBehavior.AllowGet);
                        break;
                    }
                case "tour":
                    {
                        if ((rowid ?? 0) == 0) content = Json(db2._dlf_TurLists.ToList().Select(x => new turlistsshort
                        {
                            TL_CNKEY = x.TL_CNKEY,
                            TL_Deleted = x.TL_Deleted,
                            TL_LEADDEPARTMENT = x.TL_LEADDEPARTMENT,
                            TL_EMAIL = x.TL_EMAIL,
                            TL_IsDisabled = x.TL_IsDisabled,
                            TL_KEY = x.TL_KEY,
                            TL_NAME = x.TL_NAME,
                            TL_NAMEWEB = x.TL_NAMEWEB,
                            TL_NDAY = x.TL_NDAY,
                            TL_WEB = x.TL_WEB,
                            TL_OpKey = x.TL_OpKey,
                            TL_TIP = x.TL_TIP,
                            TL_WEBHTTP = x.TL_WEBHTTP,
                            ROWID = x.ROWID
                        }).ToList(), JsonRequestBehavior.AllowGet);
                        else content = Json(db2._dlf_TurLists.ToList().Where(x => x.ROWID > rowid).Select(x => new turlistsshort
                        {
                            TL_CNKEY = x.TL_CNKEY,
                            TL_Deleted = x.TL_Deleted,
                            TL_LEADDEPARTMENT = x.TL_LEADDEPARTMENT,
                            TL_EMAIL = x.TL_EMAIL,
                            TL_IsDisabled = x.TL_IsDisabled,
                            TL_KEY = x.TL_KEY,
                            TL_NAME = x.TL_NAME,
                            TL_NAMEWEB = x.TL_NAMEWEB,
                            TL_NDAY = x.TL_NDAY,
                            TL_WEB = x.TL_WEB,
                            TL_OpKey = x.TL_OpKey,
                            TL_TIP = x.TL_TIP,
                            TL_WEBHTTP = x.TL_WEBHTTP,
                            ROWID = x.ROWID
                        }).ToList(), JsonRequestBehavior.AllowGet);
                        break;
                    }
                case "room":
                    {
                        if ((rowid ?? 0) == 0) content = Json(db2._dlf_Rooms.ToList(), JsonRequestBehavior.AllowGet);
                        else content = Json(db2._dlf_Rooms.ToList().Where(x => x.ROWID > rowid).ToList(), JsonRequestBehavior.AllowGet);
                        break;
                    }
                case "roomcategory":
                    {
                        if ((rowid ?? 0) == 0) content = Json(db2._dlf_RoomsCategories.ToList(), JsonRequestBehavior.AllowGet);
                        else content = Json(db2._dlf_RoomsCategories.ToList().Where(x => x.ROWID > rowid).ToList(), JsonRequestBehavior.AllowGet);
                        break;
                    }
                case "pansion":
                    {
                        if ((rowid ?? 0) == 0) content = Json(db2._dlf_Pansions.ToList(), JsonRequestBehavior.AllowGet);
                        else content = Json(db2._dlf_Pansions.ToList().Where(x => x.rowid > rowid).ToList(), JsonRequestBehavior.AllowGet);
                        break;
                    }
                default:
                    {
                        content = Json(null, JsonRequestBehavior.AllowGet);
                        break;
                    }
            }
            db2.Dispose();
            content.MaxJsonLength = int.MaxValue;
            return content;
        }

        [HttpGet]
        public JsonResult test()
        {
            //var req = JsonConvert.SerializeObject(new tourhotelrequest[]
            //    {new tourhotelrequest {T = 1, H = 2 } });
            return Json("test", JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public JsonResult OrdersInfo()
        [HttpPost]
        public JsonResult OrdersInfo(orderInfoRequest request)
        {
            //string jsonData = System.IO.File.ReadAllText(@"C:\json\OrdersInfo.json", Encoding.GetEncoding(1251));
            //var request = JsonConvert.DeserializeObject<orderInfoRequest>(jsonData);

            //var db = new MegatecDBDataContext(ConfigurationManager.ConnectionStrings["masterTour"].ConnectionString);

            var predicate = PredicateBuilder.True<tbl_Dogovor>();
            if ((request.Code ?? "") != "") predicate = predicate.And(x => x.DG_CODE == request.Code);
            if (request.CreatePeriod != null) 
                predicate = predicate.And(x => x.DG_CRDATE >= DateTime.Parse(request.CreatePeriod.Start, new CultureInfo("ru-Ru"))
                                            && x.DG_CRDATE <= DateTime.Parse(request.CreatePeriod.Stop, new CultureInfo("ru-Ru")));
            if ((request.TouristNameLike ?? "") != "")
                predicate = predicate.And(x => db2.tbl_Turists.Where(y => y.TU_NAMERUS.ToLower().Contains(request.TouristNameLike.ToLower())).Select(y => y.TU_DGCOD).Contains(x.DG_CODE));
            
            var dogovors = db2.tbl_Dogovors.Where(predicate).ToList();

            if (dogovors.Count() == 0) return Json("Заявки не найдены");

            List<onedog> lst = new List<onedog>(); 

            //var dogovor = db.tbl_Dogovors.SingleOrDefault(x => x.DG_CODE == request.Code);
            //if (dogovor == null) return Json("Заявка не найдена");
            foreach (var dogovor in dogovors)
            { 
                var dogovorlists = db2.tbl_DogovorLists.Where(x => x.DL_DGCOD == dogovor.DG_CODE).ToList();
                var dogovorlistshotel = dogovorlists.Where(x => x.DL_SVKEY == 3 ).FirstOrDefault();
                var turists = db2.tbl_Turists.Where(x => x.TU_DGCOD == dogovor.DG_CODE).ToList();
                var hotelroom = dogovorlistshotel == null ? null : db2.HotelRooms.Where(x => x.HR_KEY == dogovorlistshotel.DL_SUBCODE1).SingleOrDefault();
                var partner = db2.tbl_Partners.SingleOrDefault(x => x.PR_KEY == dogovor.DG_PARTNERKEY);
                var dupuser = db2.DUP_USERs.SingleOrDefault(x => x.US_KEY == dogovor.DG_DUPUSERKEY);
                var status = db2.Order_Status.SingleOrDefault(x => x.OS_CODE == dogovor.DG_SOR_CODE);

                lst.Add(new onedog
                {
                    Code = dogovor.DG_CODE,
                    CreateDate = dogovor.DG_CRDATE.ToString("dd.MM.yyyy HH:mm:ss"),
                    State = dogovor.DG_SOR_CODE == 30 ? "Rebooking" :
                            status.OS_GLOBAL == 0 ? "InWork" :
                            status.OS_GLOBAL == 1 ? "Undefined" :
                            status.OS_GLOBAL == 2 ? "Annulated" :
                            status.OS_GLOBAL == 3 ? "WaitList" :
                            status.OS_GLOBAL == 7 ? "Ok" : db2.Order_Status.SingleOrDefault(x => x.OS_CODE == dogovor.DG_SOR_CODE).OS_NameLat,
                    Cost = new Cost
                    {
                        Brutto = dogovor.DG_PRICE,
                        Netto = 0,
                        Fee = dogovor.DG_DISCOUNTSUM,
                        Rate = dogovor.DG_RATE
                    },
                    Fee = new Fee
                    {
                        Fields = new decimal[] { (decimal)dogovor.DG_DISCOUNT },
                        Case = "Percent"
                    },
                    Request = new orderInfoResponse.Request
                    {
                        Guests = turists.Select(x => new Guest
                        {
                            Birthday = x.TU_BIRTHDAY != null ? ((DateTime)x.TU_BIRTHDAY).ToString("dd.MM.yyyy") : "",
                            Citizenship = x.TU_CITIZEN,
                            DocumentNumber = x.TU_PASPRUNUM,
                            DocumentSerial = x.TU_PASPRUSER,
                            Id = x.TU_KEY,
                            Phone = x.TU_PHONE,
                            IsMale = (x.TU_SEX ?? 0) == 0,
                            Cyrilic = new Cyrilic
                            {
                                Name = x.TU_FNAMERUS,
                                Patronymic = x.TU_SNAMERUS,
                                SurName = x.TU_NAMERUS
                            },
                            Latin = new Latin
                            {
                                Name = x.TU_FNAMELAT,
                                Patronymic = x.TU_SNAMELAT,
                                SurName = x.TU_NAMELAT
                            }
                        }).ToArray(),
                        PriceKey = dogovorlistshotel == null 
                        ? new Pricekey
                        {
                            Nights = (short)(Convert.ToInt16(dogovor.DG_NDAY) - 1),
                            Date = dogovor.DG_TURDATE.ToString("dd.MM.yyyy"),
                            Tour = (int)dogovor.DG_TRKEY
                        }
                        : new Pricekey
                        {
                            Hotel = dogovorlistshotel.DL_CODE,
                            Date = dogovorlistshotel.DL_DATEBEG != null ? ((DateTime)dogovorlistshotel.DL_DATEBEG).ToString("dd.MM.yyyy") : "",
                            Nights = (short)dogovorlistshotel.DL_NDAYS,
                            Pansion = (int)dogovorlistshotel.DL_SUBCODE2,
                            Tour = dogovorlistshotel.DL_TRKEY,
                            Room = hotelroom.HR_RMKEY,
                            RoomCat = hotelroom.HR_RCKEY
                        },
                        Customer = new orderInfoResponse.Customer
                        {
                            Case = "AgencyManager",
                            Fields = new orderInfoResponse.Field[]
                            {
                            new orderInfoResponse.Field
                            {
                                Name = dupuser != null ? dupuser.US_FULLNAME : null,
                                Id = dupuser != null ? dupuser.US_KEY : 0,
                                IsActive = dupuser != null ?  (dupuser.US_REG ?? 0) == 1 : false,
                                Agency = new Agency
                                {
                                    City = (int)(partner.PR_CTKEY ?? 0),
                                    Id = partner.PR_KEY,
                                    Name = partner.PR_NAME
                                }
                            }
                            }
                        }
                    },
                    Services = dogovorlists.Select(x =>
                    new orderInfoResponse.Service
                    {
                        Brutto = x.DL_BRUTTO,
                        Fee = dogovorlistshotel == null ? 0M : x.DL_KEY == dogovorlistshotel.DL_KEY ? dogovor.DG_DISCOUNTSUM : 0,
                        Duration = (short)(x.DL_NDAYS ?? 0),
                        Name = dogovor.DG_CODE.Contains("TPL") ? x.DL_NAME.Replace("HOTEL::", "Круиз ") :
                            dogovor.DG_CODE.Contains("AVI") ? x.DL_NAME.Replace("А_П::", "Авиаперелет ") :
                            dogovor.DG_CODE.Contains("INT") ? x.DL_NAME.Replace("Проживание::Академсервис/", "Проживание ") :
                            x.DL_NAME,
                        Persons = x.DL_NMEN,
                        StartDate = x.DL_DATEBEG != null ? ((DateTime)x.DL_DATEBEG).ToString("dd.MM.yyyy") : "",

                    }).ToArray(),
                    ExtendState = dogovor.DG_SOR_CODE == 34 ? "DoubleBooking" : null
                });
            }

            //db.Dispose();

            var content = Json(lst, JsonRequestBehavior.AllowGet);
            content.MaxJsonLength = int.MaxValue;
            return content;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            db2.Dispose();
            base.Dispose(disposing);
        }

         
    }
    
    public class testrequest
    {
        public int x { get; set; }
        public int y { get; set; }
    }
}