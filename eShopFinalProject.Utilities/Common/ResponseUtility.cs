using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Utilities.Common
{
    public static class ResponseUtility
    {
        public static HttpResponseMessage ProcessResponse(HttpRequestMessage request, ResultWrapperDto<object> result)
        {
            //var content = result.Dto ?? result.Message;

            //return !HttpStatusCode.OK.Equals(result.StatusCode) ?
            //    request.CreateResponse(result.StatusCode, content) :
            //    request.CreateResponse(content);
            throw new NotImplementedException();
        }
    }
}
