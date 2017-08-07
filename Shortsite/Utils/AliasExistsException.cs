using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shortsite.Utils
{
    public class AliasExistsException : Exception
    {
        private string message = "{ERR_CODE: 001, Description:CUSTOM ALIAS ALREADY EXISTS}";

        public override string Message {
            get {
                return message;
            }}
    }
}