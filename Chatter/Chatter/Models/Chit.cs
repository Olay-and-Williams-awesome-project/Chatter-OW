using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Chatter.Models
{
    public class Chit
    {
        public int ChitID { get; set; }
        public string ChitText { get; set; }
        [DataType(DataType.DateTime)]
        public string ChitCreatedAt { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}