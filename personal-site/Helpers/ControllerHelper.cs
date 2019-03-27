using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace personal_site.Helpers
{
    public class ControllerHelper
    {
        private ControllerHelper() { }

        public static JsonResult JsonActionResponse(bool actionIsSuccess, string responseMsg, 
        IEnumerable actionErrors = null, ICollection actionData = null)
        {
            var ResponseObj = new 
            { 
                ActionSuccess = actionIsSuccess,
                ResponseMsg = responseMsg,
                Errors = actionErrors,
                Data = actionData
            };

            return new JsonResult() { Data = ResponseObj };
        }
    }
}