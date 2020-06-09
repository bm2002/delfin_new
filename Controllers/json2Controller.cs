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
using System.Net;
using System.Xml;
using System.IO;
using System.Text;
using Delfin.Data;

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