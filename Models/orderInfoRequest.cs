using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace delfin.mvc.api.Models
{
    public class orderInfoRequest
    {
        public Access Access { get; set; }
        public string Code { get; set; }
    }

    public class Access
    {
        public string Case { get; set; }
        public string[] Fields { get; set; }
    }
}