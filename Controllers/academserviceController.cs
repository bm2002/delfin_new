using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;

namespace qiwi.Controllers
{
    public class academserviceController : Controller
    {
        System.Globalization.CultureInfo cultureinfo = new System.Globalization.CultureInfo("ru-ru");
        // GET: academservice
        public ActionResult academHotelPricingRequest2(string d, string d2, string hotel, string ProductCode, string NumberOfGuests)
        {
            HotelPricingRequest2 request = new HotelPricingRequest2
            {
                BuyerId = "DELFIN-CENTRE",
                UserId = "XML",
                Password = "83finled",
                Language = LanguageType.ru,
                Hotel = hotel,
                ProductCode = ProductCode,
                Currency = "2",
                WhereToPay = WhereToPayCodeType.Item3,
                ArrivalDate = d,
                DepartureDate = d2,
                NumberOfGuests = NumberOfGuests
            };

            XmlSerializer serializer = new XmlSerializer(typeof(HotelPricingRequest2));
            string txt = string.Empty;
            using (StringWriter textWriter = new StringWriter())
            {
                serializer.Serialize(textWriter, request);
                txt = textWriter.ToString().Replace("UserId=\"XML\"", "UserId=\"XML\" Language=\"ru\"");
            }
            var requesttext = string.Format("RequestName=HotelPricingRequest2&xml={0}", txt);

            //var responsetext = GetAcademserviceXML(requesttext);

            string responsetext = System.IO.File.ReadAllText(@"C:\json\academ.xml", Encoding.GetEncoding(1251));

            

            XmlSerializer serializer2 = new XmlSerializer(typeof(HotelPricingResponse2));
            var response = ((HotelPricingResponse2)serializer2.Deserialize(XmlReader.Create(new StringReader(responsetext))));

            foreach (var it in response.Items)
            {
                if (it.GetType().ToString().Contains("HotelPricingDetail"))
                {
                    DateTime penaltyDate = DateTime.Parse(((qiwi.Controllers.HotelPricingResponse2HotelPricingDetail)it).DeadlineDate, cultureinfo);
                    decimal penaltySum = decimal.Parse(((qiwi.Controllers.HotelPricingResponse2HotelPricingDetail)it).PenaltySize.Replace(".", ","), cultureinfo);
                }
            }

            return View();
        }

        private string GetAcademserviceXML(string postdata) 
        {
            //using (System.IO.TextWriter writer = System.IO.File.CreateText("C:\\_1\\ex.txt")) writer.WriteLine(postdata);
            var d1 = DateTime.Now;
            string url = "https://www.acase.ru/xml/form.jsp";
            try
            {
                HttpWebRequest httpRequest = null;
                Uri uri = new Uri(url);
                httpRequest = (HttpWebRequest)WebRequest.Create(uri);
                httpRequest.UserAgent = "delfin-tour";
                httpRequest.Method = "POST";
                byte[] bytes = Encoding.UTF8.GetBytes(postdata);
                httpRequest.ContentType = "application/x-www-form-urlencoded";
                httpRequest.ContentLength = postdata.Length;
                Stream requestStream = httpRequest.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11;
                HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                var text = reader.ReadToEnd();
                reader.Close();
                stream.Close();
                response.Close();
                //logAcadem(url, postdata, text, DateTime.Now - d1, true, "");
                return text;
            }
            catch (Exception ex)
            {
                //logAcadem(url, postdata, null, DateTime.Now - d1, false, ex.Message);
                return "";
            }
        }

    }
}