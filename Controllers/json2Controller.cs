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

//[Route("/json2/subagents")]
namespace qiwi.Controllers
{
    
    [log]
    public class json2Controller : Controller
    {
        MTPricesDataContext db = new MTPricesDataContext(ConfigurationManager.ConnectionStrings["apiDatabase"].ToString());
        
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

        public JsonResult GetCourses()
        {
            JsonResult content = null;
            content = Json(db.dlf_RealCourses.Where(x => x.RC_DATEBEG >= DateTime.Now.Date).ToList(), JsonRequestBehavior.AllowGet);
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
                Id = ppt_Hotel.HotelId,
                Stars = ppt_HotelContent.Level,
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

            List<Field> fields = new List<Field>();

            foreach (var program in programs.OrderBy(x => x.Idx))
            {
                fields.Add(new Field
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

            parts.Fields = new Field[][] { fields.ToArray() };

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

        [HttpPost]
        public JsonResult testPost(testrequest req)
        {
            //var json = JsonConvert.SerializeObject(req);
            return Json(req.x * req.y, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            //db2.Dispose();
            base.Dispose(disposing);
        }

         
    }
    
    public class testrequest
    {
        public int x { get; set; }
        public int y { get; set; }
    }
}