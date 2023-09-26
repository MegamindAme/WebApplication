using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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

        /// <summary>
        /// Login
        /// </summary>
        /// <remarks>Login to get a JWT token. Requires a username and a password to Login. Once authorized a token will be returned in the format {"token"}.</remarks>
        /// <response code="200">Successfull</response>
        /// <response code="200">Successfull</response>
        /// <response code="4**">Request not well formed/Unsuccessful login</response>
        /// <response code="500">Sorry, Error on our side</response>
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
                    (Encoding.UTF8.GetBytes("thisisasecretkeyanditissupposedtowork"));
                    var signinCredentials = new SigningCredentials
                   (secretKey, SecurityAlgorithms.HmacSha256);
                    var jwtSecurityToken = new JwtSecurityToken(
                        issuer: "webapi",
                        audience: "https://localhost:44349",
                        claims: new List<Claim>(),
                        expires: DateTime.Now.AddMinutes(10),
                        signingCredentials: signinCredentials
                    );
                   return Ok(new object[] {new JwtSecurityTokenHandler().
                    WriteToken(jwtSecurityToken)});
                }
            }
            catch
            {
                return BadRequest
                ("An error occurred in generating the token");
            }
            return Unauthorized();
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <remarks>Register a new user. Requires a username, Email and password to register.Once registered a token will be returned in the format {"token"}.</remarks>
        /// <response code="200">Successfull</response>
        /// <response code="200">Successfull</response>
        /// <response code="4**">Request not well formed/Unsuccessful login</response>
        /// <response code="500">Sorry, Error on our side</response>
        [HttpPost, Route("register")]
        public async Task<IActionResult> Register(User user)
        {
            string? token = null;

            if (string.IsNullOrEmpty(user.Username) ||
               string.IsNullOrEmpty(user.Password) ||
               string.IsNullOrEmpty(user.Password))
                return BadRequest("Username and/or Password and/or Email not specified");

            var userFromDb = _context.Users.Where(u => u.Username == user.Username)
                .Where(u => u.Password == user.Password)
                .FirstOrDefault();

            if (userFromDb != null)
            {
                return Conflict();
            }

            _context.Users.Add(user);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }
            //

            try
            {
                    var secretKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes("thisisasecretkeyanditissupposedtowork"));
                    var signinCredentials = new SigningCredentials
                   (secretKey, SecurityAlgorithms.HmacSha256);
                    var jwtSecurityToken = new JwtSecurityToken(
                        issuer: "webapi",
                        audience: "https://localhost:44349",
                        claims: new List<Claim>(),
                        expires: DateTime.Now.AddMinutes(10),
                        signingCredentials: signinCredentials
                    );

                    token = new JwtSecurityTokenHandler().
                    WriteToken(jwtSecurityToken);

                   return Ok(new object[] { token });
            }
            catch
            {
                return BadRequest
                ("An error occurred in generating the token");
            }
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
