using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CR.Models
{
    public class mail
    {
        [RegularExpression(@"^((\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)\s*[;]{0,1}\s*)+$", ErrorMessage = "Enter valid email address")]
        public string EmailID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string Position { get; set; }
        [RegularExpression(@"^((\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)\s*[;]{0,1}\s*)+$", ErrorMessage = "Enter valid email address")]
        public string CC { get; set; }
        [RegularExpression(@"^((\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)\s*[;]{0,1}\s*)+$", ErrorMessage = "Enter valid email address")]
        public string BCC { get; set; }
        [RegularExpression(@"^((\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)\s*[;]{0,1}\s*)+$", ErrorMessage = "Enter valid email address")]
        public string To { get; set; }
        public string emailbody { get; set; }
        [Required]
        public string Subject { get; set; }
        public string hiretype { get; set; }
        public IHD_USER_MAIN user { get; set; }
        public IHD_CR cr { get; set; }
    }
}