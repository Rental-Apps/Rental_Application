using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rental_Application.EntityLayer.Utility
{
    public class MessageConstrains
    {
        //status
        public const string SUCCESS = "Success";
        public const string FAIL = "Fail";
        //message
        public const string MSG_SUCCESS = "Records get Succssfully";
        public const string MSG_NOTFOUND = "Records not found";
        public const string MSG_ERROR = "Internal Server Error";
        public const string MSG_UPDATE = "Upsert records Successfully";
    }
}
