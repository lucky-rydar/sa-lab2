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
        AuthorizationDomain domain;

        public AuthorizationController(Database db)
        {
            domain = new AuthorizationDomain(db);
        }

        [HttpPost("register")]
        public ActionResult register([FromBody] RegistrationData parameters)
        {
            try
            {
                if (!parameters.Email.Contains("@") || !parameters.Email.Contains("."))
                {
                    return BadRequest("Invalid email");
                }

                domain.register(parameters.Email, parameters.Login, parameters.Password);

                return Ok();
            }
            catch(Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("login")]
        public ActionResult login([FromBody] LoginData parameters)
        {
            try
            {
                int id = domain.login(parameters.Login, parameters.Password);
                return Ok(id);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("request_reset")]
        public ActionResult requestReset([FromBody] RequestResetPasswordData parameters)
        {
            try
            {
                var id = domain.requestReset(parameters.Email, parameters.OldPassword);
                return Ok(id);
            }
            catch(Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("accept_reset")]
        public ActionResult acceptReset([FromBody]RequestAcceptData parameters)
        {
            try
            {
                domain.acceptReset(parameters.RequestId, parameters.NewPassword);
                return Ok();
            }
            catch(Exception)
            {
                return BadRequest();
            }
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
