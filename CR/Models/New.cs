using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CR.Models;
using System.ComponentModel.DataAnnotations;

namespace CR.Models
{
    public class New
    {
        public IHD_TABDOCS tabdocs { get; set; }
        public Edu_Background edu { get; set; }
        public IHD_Tabusers tabuser { get; set; }
        public HttpPostedFileBase file_uploader { get; set; }
        public personal_data personal { get; set; }
        public string hiddenemail { get; set; }
        public Recent_Employeer employeer { get; set; }
        public Reference refrence { get; set; }
        public sysdiagram sys { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> fromdate { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> todate { get; set; }
        [Required]
        public int age { get; set; }
        public long hiddenid { get; set; }

    }
}