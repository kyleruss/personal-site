﻿using System;
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
        IEnumerable actionErrors = null, dynamic actionData = null)
        {
            var ResponseObj = new 
            { 
                ActionSuccess = actionIsSuccess,
                ResponseMsg = responseMsg,
                Errors = actionErrors,
                Data = actionData
            };

            return new JsonResult() 
            { 
                Data = ResponseObj,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public static JsonResult JsonObjectResponse(dynamic jsonData)
        {
            return new JsonResult()
            {
                Data = jsonData,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public static IEnumerable GetModelStateErrors(ModelStateDictionary state)
        {
            return state.Keys.Where(i => state[i].Errors.Count > 0)
                    .Select(k => new KeyValuePair<string, string>(k, state[k].Errors.First().ErrorMessage));
        }
    }
}