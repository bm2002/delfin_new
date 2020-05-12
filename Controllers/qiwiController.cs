using Newtonsoft.Json;
using qiwi.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace qiwi.Content
{
    public class qiwiController : Controller
    {
        dbContext db = new dbContext();

        CultureInfo cultureinfo = new CultureInfo("ru-RU");

        [HttpGet]
        public ActionResult payment(string dgcode, string summa)
        {
            var time = (DateTime.Now - new DateTime(2020, 1, 1)).Minutes + (DateTime.Now - new DateTime(2020, 1, 1)).Days * 1440 + (DateTime.Now - new DateTime(2020, 1, 1)).Hours * 60;

            decimal sumdecimal = Convert.ToDecimal(summa.Replace(".", ","));

            var bill = string.Format("{0}_{1}", dgcode, time);

            Models.qiwi.qiwirequest qr = new Models.qiwi.qiwirequest
            {
                expirationDateTime = DateTime.Now.AddHours(1),
                amount = new Models.qiwi.Amount
                {
                    currency = "RUB", //dog.DG_RATE == "рб" ? "RUB" : dog.DG_RATE,
                    value = sumdecimal.ToString("#.00").Replace(",", ".")
                }
            };

            var url = string.Format("https://api.qiwi.com/partner/bill/v1/bills/{0}", bill);

            var values = JsonConvert.SerializeObject(qr);
            var bytes = Encoding.ASCII.GetBytes(values);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "PUT";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Bearer dc61428d-272f-4ed4-9330-5c14284f81f6");
            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(bytes, 0, bytes.Length);
            }
            var response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            string text = reader.ReadToEnd();

            var resp = JsonConvert.DeserializeObject<Models.qiwi.qiwiresponse>(text);

            PaymentsQiwi pw = new PaymentsQiwi
            {
                Amount = Convert.ToDecimal(resp.amount.value.Replace(".", ",")),
                DateCreate = DateTime.Now,
                DateModified = DateTime.Now,
                DgCode = dgcode,
                Status = resp.status.value,
                payUrl = resp.payUrl,
                billId = resp.billId,
                response = text,
                paymentIdqiwi = resp.payUrl.Split(new[] { "invoice_uid=" }, StringSplitOptions.None)[1].Trim()
            };
            db._PaymentsQiwies.Add(pw);
            db.SaveChanges(); 

            return Json(resp.payUrl, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        public ActionResult paymentConfirm(qiwiPayment request)
        {
            var qp = db._PaymentsQiwies.SingleOrDefault(x => x.billId == request.payment.billId && x.paymentIdqiwi == request.payment.paymentId);
            
            if (qp == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Нет такого платежа");

            if (qp.Amount != decimal.Parse(request.payment.amount.value.Replace(".", ","), cultureinfo))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Сумма платежа не совпадает");

            qp.Status = request.payment.status.value;
            qp.DateModified = DateTime.Now;
            db.SaveChanges();

            PaymentsQiwiResponse pqr = new PaymentsQiwiResponse
            {
                Amount = decimal.Parse(request.payment.amount.value.Replace(".", ","), cultureinfo),
                billIdqiwi = request.payment.billId,
                paymentIdqiwi = request.payment.paymentId,
                date = DateTime.Now,
                response = JsonConvert.SerializeObject(request)
            };


            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}