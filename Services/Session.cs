using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadmansFileshareAppCSharp.Services
{
    public class Session
    {
        public string Token { get; set; }
        public string UserId { get; set; }
        public DateTime Expiration { get; set; }

        public string UserName { get; set; }

        public bool IsValid => !string.IsNullOrEmpty(Token) && Expiration > DateTime.UtcNow;

        public Session()
        {
            this.Token = string.Empty;
            this.UserId = string.Empty;
            this.UserName = string.Empty;
            this.Expiration = DateTime.UtcNow;
        }
    }
}