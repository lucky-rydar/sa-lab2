using authorization_service.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authorization_service
{
    public class AuthorizationDomain
    {
        Database db_;

        public AuthorizationDomain(Database db)
        {
            db_ = db;
        }

        public void register(string email, string login, string password)
        {
            Account account = new Account()
            {
                Id = db_.lastAccountId,
                Email = email,
                Login = login,
                Password = password
            };

            db_.accounts_.Add(account);
            db_.lastAccountId++;
        }

        public int login(string login, string password)
        {
            Account account = db_.accounts_.Where(x => x.Login == login).FirstOrDefault();
            if (account.Password != password)
            {
                throw new ArgumentException();
            }
            return account.Id;
        }

        public string requestReset(string email, string oldPassword)
        {
            Account account = db_.accounts_.First(x => x.Email == email);

            if (account.Password != oldPassword)
            {
                throw new ArgumentException();
            }

            ResetRequest resetRequest = new ResetRequest()
            {
                AccountId = account.Id,
                RequestId = Guid.NewGuid().ToString()
            };
            db_.resetRequests_.Add(resetRequest);

            return resetRequest.RequestId;
        }

        public void acceptReset(string requestId, string newPassword)
        {
            ResetRequest resetRequest = db_.resetRequests_.Where(x => x.RequestId == requestId).FirstOrDefault();
            Account account = db_.accounts_.Where(x => x.Id == resetRequest.AccountId).FirstOrDefault();
            account.Password = newPassword;
            db_.resetRequests_.RemoveAll(x => x.RequestId == resetRequest.RequestId);
        }
    }
}
