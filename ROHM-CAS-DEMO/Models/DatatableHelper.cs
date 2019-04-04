using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ROHM_CAS_DEMO.Models
{
    public class DatatableHelper
    {
        public object GetPropertyValue(object obj, string name)
        {
            return obj == null ? null : obj.GetType()
            .GetProperty(name)
            .GetValue(obj, null);
        }
    }
}