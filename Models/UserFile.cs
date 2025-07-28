using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadmansFileshareAppCSharp.Models
{
    internal class UserFile
    {
        public String? _id { get; set; }
        public String? original_filename { get; set; }
        public DateTime? time_of_death { get; set; }
        public String? user_id { get; set; }
        public String? password {  get; set; }
        public String? allowed_users { get; set; }
        public int? num_allowed_access { get; set; }
        public String? file { get; set; } 
    }
}
