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

        public Session(string Token, string UserId, string UserName, DateTime Expiration)
        {
            this.Token = Token;
            this.UserId = UserId;
            this.UserName = UserName;
            this.Expiration = Expiration;
        }
    }
}