using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadmansFileshareAppCSharp.Models
{
    internal class File
    {

        public String original_filename { get; set; } = String.Empty;
        public DateTime time_of_death { get; set; } = DateTime.MinValue;
        public String user_id { get; set; } = String.Empty;
        public String? password {  get; set; } = null;
        public String? allowed_users { get; set; } = null;
        public int num_allowed_access { get; set; } = 1;
        public String? file { get; set; } = String.Empty;
    }
}
