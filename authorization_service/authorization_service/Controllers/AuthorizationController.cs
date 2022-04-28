using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace authorization_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        Database db_;

        public AuthorizationController(Database db)
        {
            db_ = db;
        }

        [HttpPost("register")]
        public ActionResult register([FromBody] RegistrationData parameters)
        {
            if (parameters.Email == null || parameters.Login == null || parameters.Password == null)
            {
                return BadRequest("Invalid data");
            }

            if (db_.accounts_.Exists(x => x.Email == parameters.Email))
            {
                return BadRequest("Email is already registered");
            }

            if (!parameters.Email.Contains("@") || !parameters.Email.Contains("."))
            {
                return BadRequest("Invalid email");
            }

            Account account = new Account()
            {
                Id = db_.lastAccountId,
                Email = parameters.Email,
                Login = parameters.Login,
                Password = parameters.Password
            };

            db_.accounts_.Add(account);
            db_.lastAccountId++;
            return Ok();
        }

        [HttpGet("login")]
        public ActionResult login([FromBody] LoginData parameters)
        {
            if (parameters.Login == null || parameters.Password == null)
            {
                return BadRequest("Invalid data");
            }

            if (!db_.accounts_.Exists(x => x.Login == parameters.Login))
            {
                return BadRequest("Login doesn't exist");
            }

            Account account = db_.accounts_.Where(x => x.Login == parameters.Login).FirstOrDefault();
            if (account.Password != parameters.Password)
            {
                return BadRequest("Invalid password");
            }

            return Ok(account.Id);
        }

        [HttpGet("request_reset")]
        public ActionResult requestReset([FromBody] RequestResetPasswordData parameters)
        {
            if (parameters.Email == null || parameters.OldPassword == null)
            {
                return BadRequest("Invalid data");
            }

            if (!db_.accounts_.Any(x => x.Email == parameters.Email))
            {
                return BadRequest("Email doesn't exist");
            }

            Account account = db_.accounts_.First(x => x.Email == parameters.Email);

            if(account.Password != parameters.OldPassword)
            {
                return BadRequest("Invalid password");
            }

            ResetRequest resetRequest = new ResetRequest()
            {
                AccountId = account.Id,
                RequestId = Guid.NewGuid().ToString()
            };
            db_.resetRequests_.Add(resetRequest);

            return Ok(resetRequest.RequestId);
        }

        [HttpGet("accept_request")]
        public ActionResult acceptRequest([FromBody]RequestAcceptData parameters)
        {
            if (parameters.RequestId== null || parameters.NewPassword == null)
            {
                return BadRequest("Invalid data");
            }

            if(!db_.resetRequests_.Exists(x => x.RequestId == parameters.RequestId))
            {
                return BadRequest("No such request id");
            }

            ResetRequest resetRequest = db_.resetRequests_.Where(x => x.RequestId == parameters.RequestId).FirstOrDefault();
            Account account = db_.accounts_.Where(x => x.Id == resetRequest.AccountId).FirstOrDefault();
            account.Password = parameters.NewPassword;
            db_.resetRequests_.RemoveAll(x => x.RequestId == resetRequest.RequestId);

            return Ok();
        }
    }

    public class Account
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class ResetRequest
    {
        public int AccountId { get; set; }
        public string RequestId { get; set; }
    }

    // parameters for methods
    public class RegistrationData
    {
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class LoginData
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class RequestResetPasswordData
    {
        public string Email { get; set; }
        public string OldPassword { get; set; }
    }

    public class RequestAcceptData
    {
        public string RequestId { get; set; }
        public string NewPassword { get; set; }
    }
}
