using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ROHM_CAS_DEMO.Models
{
    public class ReturnObject
    {
        public bool flag { get; set; }
        public string message { get; set; }
        public List<object> items { get; set; }
        public bool Matome { get; set; }
        public ReturnObject()
        {
            items = new List<object>();
            message = "";
            Matome = false;
        }
    }
}