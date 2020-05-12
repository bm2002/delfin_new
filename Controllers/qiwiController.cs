using Newtonsoft.Json;
using qiwi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Delfin.Data;

namespace qiwi.Content
{
    public class qiwiController : Controller
    {
        dbContext db = new dbContext();

        CultureInfo cultureinfo = new CultureInfo("ru-RU");

        [HttpPost]
        public string test()
        {
            return "dsffdsdfs";
        }

        [HttpGet]
        public ActionResult payment(string dgcode, string summa)
        {
            var time = (DateTime.Now - new DateTime(2020, 1, 1)).Minutes + (DateTime.Now - new DateTime(2020, 1, 1)).Days * 1440 + (DateTime.Now - new DateTime(2020, 1, 1)).Hours * 60;

            decimal sumdecimal = Convert.ToDecimal(summa.Replace(".", ","));

            var bill = string.Format("{0}_{1}", dgcode, time);

            //if (db._PaymentsQiwies.Where(x => x.billId == bill).Count() > 0)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Повторный платеж. Попробуйте позже");
            //}

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

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;

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

        [HttpPost]
        public ActionResult paymentConfirm2()
        {
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult paymentConfirm(qiwiPayment request)
        {
            var qp = db._PaymentsQiwies.SingleOrDefault(x => x.billId == request.payment.billId && x.paymentIdqiwi == request.payment.paymentId);
            
            if (qp == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Нет такого платежа");

            if (qp.Amount != decimal.Parse(request.payment.amount.value.Replace(".", ","), cultureinfo))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Сумма платежа не совпадает");

            qp.Status = request.payment.status.value;
            qp.DateModified = DateTime.Now;
            db.SaveChanges();

            var percent = 1.0 - double.Parse(ConfigurationManager.AppSettings.Get("percent").Replace(".", ","), cultureinfo) / 100;

            tbl_Dogovor dogovor = null;
            using (var db1 = new MegatecDBDataContext(ConfigurationManager.ConnectionStrings["masterTour"].ToString()))
            {
                dogovor = db1.tbl_Dogovors.Where(x => x.DG_CODE == qp.billId.Split('_')[0].Trim()).SingleOrDefault();
            }

            PaymentsQiwiResponse pqr = new PaymentsQiwiResponse
            {
                Amount = decimal.Parse(request.payment.amount.value.Replace(".", ","), cultureinfo),
                billIdqiwi = request.payment.billId,
                paymentIdqiwi = request.payment.paymentId,
                date = DateTime.Now,
                response = JsonConvert.SerializeObject(request)
            };
            db._PaymentsQiwiResponses.Add(pqr);
            db.SaveChanges();


            qiwi.Models.Payment payment = new qiwi.Models.Payment
            {
                Pm_DocumentNumber = pqr.billIdqiwi,
                Pm_CreateDate = DateTime.Now,
                Pm_Prkey = (int)dogovor.DG_PARTNERKEY,
                Pm_Poid = 178,
                Pm_CreatorKey = 0,
                Pm_Date = DateTime.Now,
                Pm_FilialKey = 1,
                Pm_DepartmentKey = 0,
                Pm_Export = 0,
                Pm_Number = dogovor.DG_Key,
                Pm_Rakey = 2,
                Pm_Reason = string.Format("Оплата за туристическ. путевку {0}", dogovor.DG_CODE),
                Pm_RepresentName = dogovor.DG_MAINMEN ?? "",
                Pm_Sum = Convert.ToDecimal(Math.Round((double)pqr.Amount * percent, 2)),
                Pm_SumNational = Convert.ToDecimal(Math.Round((double)pqr.Amount * percent, 2)),
                Pm_Used = Convert.ToDecimal(Math.Round((double)pqr.Amount * percent, 2)),
                Pm_Vdata = pqr.paymentIdqiwi
            };
            db.Payments.Add(payment);
            db.SaveChanges();

            qiwi.Models.PaymentDetail pd = new qiwi.Models.PaymentDetail
            {
                Pd_Pmid = payment.Pm_Id,
                Pd_Course = 1,
                Pd_CreateDate = DateTime.Now,
                Pd_CreatorKey = 0,
                Pd_Date = DateTime.Now,
                Pd_Dgkey = dogovor.DG_Key,
                Pd_Percent = 0,
                Pd_Reason = string.Format("Оплата за туристическ. путевку {0}", dogovor.DG_CODE),
                Pd_Sum = pqr.Amount,
                Pd_SumNational = pqr.Amount,
                Pd_SumInDogovorRate = pqr.Amount
            };
            db.PaymentDetails.Add(pd);

            qiwi.Models.PaymentDetail pd2 = new qiwi.Models.PaymentDetail
            {
                Pd_Pmid = payment.Pm_Id,
                Pd_Course = 1,
                Pd_CreateDate = DateTime.Now,
                Pd_Date = DateTime.Now,
                Pd_CreatorKey = 0,
                Pd_Percent = 0,
                Pd_Sum = -Convert.ToDecimal(Math.Round((double)pqr.Amount * percent, 2)),
                Pd_SumNational = -Convert.ToDecimal(Math.Round((double)pqr.Amount * percent, 2)),
                Pd_SumInDogovorRate = -Convert.ToDecimal(Math.Round((double)pqr.Amount * percent, 2))
            };
            db.PaymentDetails.Add(pd2);
            db.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}