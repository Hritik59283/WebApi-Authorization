using CrudAPI.Models;
using CrudAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace CrudAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IConfiguration _configuration;
        IUserRepository _userRepository=null;

        public UserController(IConfiguration configuration, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _configuration = configuration;
         
        }
        [HttpPost]
        [Route("Registration")]
        public async Task<IActionResult> Registration([FromBody] User model)
        {
            try {
                model = await _userRepository.Save(model);
                return Ok(model);
            }
            catch(Exception ex) { return StatusCode((int)HttpStatusCode.InternalServerError); }
        }

        [HttpGet]
        [Route("SignIn")]
        public async Task<IActionResult> SignIn(string username,string password,string test)
        {
            try
            {
                User model = new User()
                {
                    UserName=username,
                    Password=password   
                };
                var user = await Authentication(model);
                if(user.UserId==0) return StatusCode((int)HttpStatusCode.NotFound,"Invalid User");
                user.Token = GenerateToken(model);
                return Ok(user);
            }
            catch (Exception ex) { return StatusCode((int)HttpStatusCode.InternalServerError); }
        }
        private async Task<User> Authentication(User user)
        {
            return await _userRepository.GetByEmail(user);
        }
        private string  GenerateToken(User model)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //    var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                
                expires: DateTime.Now.AddMinutes(20),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
