using authorization_service.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authorization_service
{
    public class Database
    {
        public int lastAccountId = 0;
        public List<Account> accounts_ = new List<Account>();
        public List<ResetRequest> resetRequests_ = new List<ResetRequest>();
    }
}
