using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadmansFileshareAppCSharp.Models
{
    internal class File
    {
        private string filename { get; set; } = string.Empty;
        private string userID { get; set; } = string.Empty;
        private DateTime expirationDate { get; set; } = DateTime.MinValue;

        public File(string filename, DateTime expirationDate)
        {
            this.filename = filename;
            this.expirationDate = expirationDate;
        }
    }
}
