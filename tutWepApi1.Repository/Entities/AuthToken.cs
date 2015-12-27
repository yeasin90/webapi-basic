using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tutWepApi1.Repository.Entities
{
    public class AuthToken
    {
        public int Id;
        public string Token;
        public DateTime Expiration;
        public ApiUser ApiUser;
    }
}
