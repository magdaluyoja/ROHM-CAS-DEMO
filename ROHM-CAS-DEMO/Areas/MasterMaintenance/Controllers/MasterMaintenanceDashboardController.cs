using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ROHM_CAS_DEMO.Areas.MasterMaintenance.Controllers
{
    public class MasterMaintenanceDashboardController : Controller
    {
        // GET: MasterMaintenance/MasterMaintenanceDashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}