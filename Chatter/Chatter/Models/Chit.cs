using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Chatter.Models
{
    public class Chit
    {
        public int ChitID { get; set; }
        public string ChitText { get; set; }
        [DataType(DataType.DateTime)]
        public string ChitCreatedAt { get; set; }
    }
}