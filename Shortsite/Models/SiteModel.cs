using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shortsite.Models
{
    public class SiteModel
    {
        public string Url { get; set; }
        public string Error { get; set; }
        public string Alias { get; set; }
        public int Time { get; set; }
    }
}