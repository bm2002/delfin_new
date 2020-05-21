using Newtonsoft.Json;
using qiwi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace qiwi.Filters
{
    public class logAttribute : ActionFilterAttribute
    {
        logContext log = new logContext();
        public DateTime d { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var request = filterContext.HttpContext.Request;

            if (filterContext.Exception == null)            
                log.RequestDumps2.Add(new RequestDump2
                {
                    ProviderId = 201,
                    Url = request.Url.AbsoluteUri,
                    Success = true,
                    Duration = DateTime.Now - d,
                    Response = JsonConvert.SerializeObject(((JsonResult)filterContext.Result).Data),
                    ClientIP = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.UserHostAddress,
                    Created = DateTime.Now,
                    Label = filterContext.ActionDescriptor.ActionName
                });
            else
            {
                log.RequestDumps2.Add(new RequestDump2
                {
                    ProviderId = 201,
                    Url = request.Url.AbsoluteUri,
                    Success = false,
                    ErrorMessage = filterContext.Exception.Message,
                    Duration = DateTime.Now - d,
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
            //var request = filterContext.HttpContext.Request;
            //base.OnActionExecuting(filterContext);
            d = DateTime.Now;
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