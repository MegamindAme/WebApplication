using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication4.DataDB;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{   
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly TestApiContext _context;

        public LoginController(TestApiContext context)
        {
            _context = context;
        }

        [HttpPost, Route("login")]
        public IActionResult Login(User loginDTO)
        {
            try
            {
                if (string.IsNullOrEmpty(loginDTO.Username) ||
                string.IsNullOrEmpty(loginDTO.Password))
                    return BadRequest("Username and/or Password not specified");

                var user = _context.Users.Where(u => u.Username == loginDTO.Username)
                    .Where(u => u.Password == loginDTO.Password)
                    .FirstOrDefault();

                if (user != null)
                {
                    var secretKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes("thisisasecretkey@123"));
                    var signinCredentials = new SigningCredentials
                   (secretKey, SecurityAlgorithms.HmacSha256);
                    var jwtSecurityToken = new JwtSecurityToken(
                        issuer: "ABCXYZ",
                        audience: "http://localhost:51398",
                        claims: new List<Claim>(),
                        expires: DateTime.Now.AddMinutes(10),
                        signingCredentials: signinCredentials
                    );
                    Ok(new JwtSecurityTokenHandler().
                    WriteToken(jwtSecurityToken));
                }
            }
            catch
            {
                return BadRequest
                ("An error occurred in generating the token");
            }
            return Unauthorized();
        }
        
        [HttpPost, Route("register")]
        public IActionResult Register(User user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.Username) ||
                string.IsNullOrEmpty(user.Password) ||
                string.IsNullOrEmpty(user.Password))
                    return BadRequest("Username and/or Password and/or Email not specified");

                _context.Users.Add(user);

                try
                {
                    _context.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    if (UserExists(user.Id))
                    {
                        return Conflict();
                    }
                    else
                    {
                        throw;
                    }
                }

                if (user == null)
                {
                    var secretKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes("thisisasecretkey@123"));
                    var signinCredentials = new SigningCredentials
                   (secretKey, SecurityAlgorithms.HmacSha256);
                    var jwtSecurityToken = new JwtSecurityToken(
                        issuer: "ABCXYZ",
                        audience: "http://localhost:51398",
                        claims: new List<Claim>(),
                        expires: DateTime.Now.AddMinutes(10),
                        signingCredentials: signinCredentials
                    );
                  return Ok(new JwtSecurityTokenHandler().
                    WriteToken(jwtSecurityToken));
                }
            }
            catch
            {
                return BadRequest
                ("An error occurred in generating the token");
            }

        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
