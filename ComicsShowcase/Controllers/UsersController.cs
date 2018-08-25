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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ComicsShowcase.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly ComicsContext _context;
        public UsersController(ComicsContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<List<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        // POST api/users/register
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

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {

            return Json(new {
                result = "success",
                value = "pepe"
            });
        }
    }
}
