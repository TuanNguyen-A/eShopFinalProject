using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Utilities.Common
{
    public class ResultWrapperDto<T>
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public T? Dto { get; set; }

        public ResultWrapperDto(T dto)
        {
            StatusCode = 200;
            Message = "Success";
            Dto = dto;
        }

        public ResultWrapperDto(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public ResultWrapperDto(int statusCode, string message, T dto)
        {
            StatusCode = statusCode;
            Message = message;
            Dto = dto;
        }

    }
}
