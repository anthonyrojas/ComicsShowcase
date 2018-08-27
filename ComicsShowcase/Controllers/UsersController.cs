using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using ComicsShowcase.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ComicsShowcase.Controllers
{
    [Authorize(Roles = "User")]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        public readonly static string SECRET = "you-know-nothing-jon-snow";
        private readonly ComicsContext _context;
        public UsersController(ComicsContext context)
        {
            _context = context;
        }
        // POST api/users/register
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]User user)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    //check if a user currently has the username entered
                    var query = from curr_user in _context.Users where curr_user.Username.Equals(user.Username) select curr_user;
                    if(query.Any())
                    {
                        return BadRequest(new { statusMessage = "A user with that username already exists." });
                    }

                    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password, SaltRevision.Revision2B);
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                    user.Password = null;
                    return Json(user);
                }
                catch (Exception e)
                {
                    return BadRequest(new { statusMessage = e.Message });
                }
            }
            return BadRequest(ModelState);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginModel userLogin)
        {
            if(String.IsNullOrEmpty(userLogin.Username))
            {
                return BadRequest(new { statusMessage = "You must enter your username." });
            }
            if(String.IsNullOrEmpty(userLogin.Password))
            {
                return BadRequest(new { statusMessage = "You must enter your password." });
            }
            try{
                User userFound = await _context.Users.SingleOrDefaultAsync(u => u.Username == userLogin.Username);
                if(userFound != null)
                {
                    //validate password
                    if(BCrypt.Net.BCrypt.Verify(userLogin.Password, userFound.Password)){
                        //password was correct
                        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET));
                        var signInCred = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature);
                        var claimsList = new List<Claim> 
                        {
                            new Claim(ClaimTypes.Name, userFound.Username),
                            new Claim(ClaimTypes.NameIdentifier, userFound.ID.ToString()),
                            new Claim(ClaimTypes.Role, "User")
                        };
                        var tokenOptions = new JwtSecurityToken(
                            issuer: "https://localhost:5001",
                            audience: "https://localhost:5001",
                            claims: claimsList,
                            expires: DateTime.Now.AddYears(1),
                            signingCredentials: signInCred
                        );
                        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                        return Ok(new { token = tokenString, statusMessage = "Login successful!" });
                    }else
                    {
                        //incorrect password
                        return BadRequest(new { statusMessage = "Wrong password." });
                    }
                }
                return BadRequest(new { statusMessage = "A user with that username was not found." });
            }catch(Exception e){
                return BadRequest(new {statusMessage = e});
            }
        }

        // GET api/values/5
        [HttpGet("account")]
        public async Task<IActionResult> GetAccountInfo()
        {
            //get the username from the token
            //string username = User.FindFirst(ClaimTypes.Name)?.Value;
            //get the id from the token
            int userID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            User userInfo = await _context.Users.FirstOrDefaultAsync(u => u.ID == userID);
            if(userInfo != null)
            {
                userInfo.Password = null;
                return Ok(new { user = userInfo, statusMessage = "User account information retrieved!" });
            }
            return BadRequest(new {statusMessage = "Unable to find your account information. Please log out and sign in again."});
        }
    }
}
