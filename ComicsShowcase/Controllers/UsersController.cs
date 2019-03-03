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
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Cors;

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
                    if (!string.IsNullOrEmpty(user.ProfileStr))
                    {
                        string[] imgData = user.ProfileStr.Split(new []{"base64,"}, StringSplitOptions.None);
                        user.ProfileStr = imgData[0];
                        user.Profile = Convert.FromBase64String(imgData[1]);
                    }
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                    user.Password = null;
                    return Ok(new { statusMessage = "Successfully registered!", user });
                }
                catch (Exception e)
                {
                    return BadRequest(new { statusMessage = e.Message });
                }
            }
            return BadRequest(ModelState);
        }

        // POST api/users/login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginModel userLogin)
        {
            if(String.IsNullOrEmpty(userLogin.Username) && String.IsNullOrEmpty(userLogin.Password)){
                return BadRequest(new { 
                    statusMessage = "You have errors in your input. Please fill out all fields with valid data.",
                    usernameErr = "You must enter your username.",
                    passwordErr = "You must enter your password."
                });
            }
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

        // GET api/users/account
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
                if(userInfo.ProfileStr != null && userInfo.Profile != null)
                {
                    userInfo.ProfileStr = userInfo.ProfileStr + "base64," + Convert.ToBase64String(userInfo.Profile);
                }
                return Ok(new { user = userInfo, statusMessage = "User account information retrieved!" });
            }
            return BadRequest(new {statusMessage = "Unable to find your account information. Please log out and sign in again."});
        }

        //PUT api/users/account
        [HttpPut("account")]
        public async Task<IActionResult> UpdateAccount([FromBody] User userModel)
        {
            int userID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            User userInfo = await _context.Users.FirstOrDefaultAsync(u => u.ID == userID);
            if(string.IsNullOrWhiteSpace(userModel.Password) || string.IsNullOrEmpty(userModel.Password)) 
            {
                userModel.Password = userInfo.Password; 
            }
            if (!string.IsNullOrEmpty(userModel.ProfileStr))
            {
                string[] imgData = userModel.ProfileStr.Split(new[] { "base64," }, StringSplitOptions.None);
                userModel.ProfileStr = imgData[0];
                userModel.Profile = Convert.FromBase64String(imgData[1]);
            }

            var validationContext = new ValidationContext(userModel, null, null);
            List<ValidationResult> results = new List<ValidationResult>();
            if(Validator.TryValidateObject(userModel, validationContext, results, true))
            {
                //check if new username is already taken
                int userCount = await _context.Users.Where(u => u.Username == userModel.Username && u.ID != userModel.ID).CountAsync();
                if(userCount > 0)
                {
                    return BadRequest(new { statusMessage = "A user with that username already exists.", errors = results });
                }
                userModel.Password = BCrypt.Net.BCrypt.HashPassword(userModel.Password, SaltRevision.Revision2B);
                _context.Entry(userInfo).State = EntityState.Detached;
                _context.Users.Update(userModel);
                await _context.SaveChangesAsync();
                userModel.Password = null;
                userModel.ProfileStr = userModel.ProfileStr + "base64," + Convert.ToBase64String(userModel.Profile);
                return Ok(new { statusMessage = "User updated!", user = userModel });
            }
            return BadRequest(new { statusMessage="Unable to update your username.", errors=results });
        }
    }
}
