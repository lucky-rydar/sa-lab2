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
        [HttpGet("register")]
        public string register([FromBody] RegistrationData parameters)
        {
            return parameters.Email;
        }
    }

    public class RegistrationData
    {
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
