using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rental_Application.EntityLayer.Response;

namespace Rental_Application.EntityLayer.Utility
{
    public static class GenericResponse
    {
        public static ListResponse<T> CreateResponse<T>(List<T> data, string message, string status, int statusCode)
        {
            var response = new ListResponse<T>();
            response.Data = data;
            response.Message = message;
            response.Status = status;
            response.StatusCode = statusCode;
            return response;
        }

        public static ListResponse<T> CreateSingleResponse<T>(T data, string message, string status, int statusCode)
        {
            var response = new ListResponse<T>();
            response.Data = data != null ? new List<T> { data } : new List<T>();
            response.Message = message;
            response.Status = status;
            response.StatusCode = statusCode;
            return response;

        }
    }
}
