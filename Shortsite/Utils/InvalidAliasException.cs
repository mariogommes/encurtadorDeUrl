using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shortsite.Utils
{
    public class InvalidAliasException : Exception
    {
        private string message = "{ERR_CODE: 002, Description:CUSTOM ALIAS IS INVALID, TRY ANOTHER}";

        public override string Message
        {
            get
            {
                return message;
            }
        }
    }
}