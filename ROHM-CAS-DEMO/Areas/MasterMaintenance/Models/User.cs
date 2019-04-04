using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ROHM_CAS_DEMO.Areas.MasterMaintenance.Models
{
    public class User
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "User ID is required")]
        public string UserID { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Firstname is required")]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Lastname is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Access Type is required")]
        public int AccessType { get; set; }

        [Required(ErrorMessage = "Division is required")]
        public string REPIDiv { get; set; }
        public string Photo { get; set; }
        public bool IsDeleted { get; set; }
        public string CreateID { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateID { get; set; }
        public System.DateTime UpdateDate { get; set; }

        [Required(ErrorMessage = "Please select at least one module.")]
        public string ModuleID { get; set; }
        public int UserModuleId { get; set; }
        public string StrModuleID { get; set; }
    }
}