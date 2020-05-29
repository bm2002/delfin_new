using Microsoft.Identity.Client;
using Newtonsoft.Json;
using qiwi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace qiwi.Filters
{
    public class logAttribute : ActionFilterAttribute
    {
        logContext log = new logContext();

        public string body { get; set; }
        public DateTime d { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var request = filterContext.HttpContext.Request;

            if (filterContext.Exception == null)            
                log.RequestDumps2.Add(new RequestDump2
                {
                    ProviderId = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == "json2" ? 201 :
                                            filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == "qiwi" ? 202 : 0,
                    Url = request.Url.AbsoluteUri,
                    Success = true,
                    Duration = DateTime.Now - filterContext.HttpContext.Timestamp,
                    //Duration = DateTime.Now - d,
                    Response = JsonConvert.SerializeObject(((JsonResult)filterContext.Result).Data),
                    ClientIP = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.UserHostAddress,
                    Created = DateTime.Now,
                    Label = filterContext.ActionDescriptor.ActionName,
                    Request = filterContext.HttpContext.Request.HttpMethod == "POST" ? body : ""
                });
            else
            {
                log.RequestDumps2.Add(new RequestDump2
                {
                    ProviderId = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == "json2" ? 201 :
                                            filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == "qiwi" ? 202 : 0,
                    Url = request.Url.AbsoluteUri,
                    Success = false,
                    ErrorMessage = filterContext.Exception.Message,
                    Duration = DateTime.Now - filterContext.HttpContext.Timestamp,
                    //Duration = DateTime.Now - d,
                    //Response = JsonConvert.SerializeObject(((JsonResult)filterContext.Result).Data),
                    ClientIP = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.UserHostAddress,
                    Created = DateTime.Now,
                    Label = filterContext.ActionDescriptor.ActionName
                });
            }
            log.SaveChanges();
        }


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.HttpMethod == "POST")
                try
                {
                    body = JsonConvert.SerializeObject(filterContext.ActionParameters.AsEnumerable().ToList()[0].Value);
                }
                catch { }
            //d = DateTime.Now;
        }

        //public override void OnResultExecuted(ResultExecutedContext filterContext)
        //{
        //    var context = filterContext.Controller.ControllerContext;

        //    base.OnResultExecuted(filterContext);
        //}

        //public override void OnResultExecuting(ResultExecutingContext filterContext)
        //{
        //    base.OnResultExecuting(filterContext);
        //}
    }
}