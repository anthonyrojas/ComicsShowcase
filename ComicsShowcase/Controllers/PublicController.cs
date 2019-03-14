using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ComicsShowcase.Models;
using Microsoft.AspNetCore.Authorization;

namespace ComicsShowcase.Controllers
{
    // api/public
    // allows users to search for public information of other users
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class PublicController : Controller
    {
        private readonly ComicsContext _context;
        public PublicController(ComicsContext context)
        {
            _context = context;
        }

        //usernames are unique so only a single user should be returned
        [HttpPost("search/username")]
        public async Task<IActionResult> SearchByUsername([FromBody]string searchValue)
        {
            var usersFound = _context.Users.Where(u => u.Username == searchValue).Select(u => new {
                username = u.Username,
                email = u.Email,
                profileStr = u.Profile != null && u.ProfileStr != null ? u.ProfileStr + "base64," + Convert.ToBase64String(u.Profile) : null,
                u.ID,
                u.redditUsername,
                u.instagramUsername
            });
            if (usersFound.Any())
            {
                return Ok(new { statusMessage = "Users found!", users = usersFound });
            }
            return BadRequest(new { statusMessage = "There is no user with that username." });
        }

        [HttpPost("search/email")]
        public async Task<IActionResult> SearchByEmail([FromBody]string searchValue)
        {
            var usersFound = _context.Users.Where(u => u.Email == searchValue).Select(u => new {
                username = u.Username,
                email = u.Email,
                profileStr = u.Profile != null && u.ProfileStr != null ? u.ProfileStr + "base64," + Convert.ToBase64String(u.Profile) : null,
                u.ID,
                u.redditUsername,
                u.instagramUsername
            });
            if(usersFound.Any())
            {
                return Ok(new {statusMessage = "Users found!", users = usersFound});
            }
            return BadRequest(new {statusMessage = "There are no users with that email."});
        }

        [HttpPost("search/instagram")]
        public async Task<IActionResult> SearchByIG([FromBody] string searchValue)
        {
            var usersFound = _context.Users.Where(u => u.instagramUsername == searchValue).Select(u => new
            {
                username = u.Username,
                email = u.Email,
                profileStr = u.Profile != null && u.ProfileStr != null ? u.ProfileStr + "base64," + Convert.ToBase64String(u.Profile) : null,
                u.ID,
                u.redditUsername,
                u.instagramUsername
            });
            if(usersFound.Any())
            {
                return Ok(new {statusMessage = "Users found!", users = usersFound});
            }
            return BadRequest(new {statusMessage = "There are no users with that instagram username on their profile."});
        }

        [HttpPost("search/reddit")]
        public async Task<IActionResult> SearchByReddit([FromBody] string searchValue)
        {
            var usersFound = _context.Users.Where(u => u.redditUsername == searchValue).Select(u => new
            {
                username = u.Username,
                email = u.Email,
                profileStr = u.Profile != null && u.ProfileStr != null ? u.ProfileStr + "base64," + Convert.ToBase64String(u.Profile) : null,
                u.ID,
                u.redditUsername,
                u.instagramUsername
            });
            if (usersFound.Any())
            {
                return Ok(new { statusMessage = "Users found!", users = usersFound });
            }
            return BadRequest(new { statusMessage = "There are no users with that reddit username on their profile." });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPublicUserInfo(int id)
        {
            User userFound = await _context.Users.FirstOrDefaultAsync(u => u.ID == id);
            if (userFound != null)
            {
                userFound.Password = null;
                int comicsCount = _context.Comics.Where(u => u.User.ID == userFound.ID).Count();
                int novelsCount = _context.GraphicNovels.Where(g => g.User.ID == userFound.ID).Count();
                int collectiblesCount = _context.Collectibles.Where(c => c.User.ID == userFound.ID).Count();
                return Ok(new
                {
                    statusMessage = "User information found!",
                    user = userFound,
                    comicsCount,
                    novelsCount,
                    collectiblesCount
                });
            }
            return BadRequest(new { statusMessage = "User with that username does not exist." });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFullPublicUserInfo(int id){
            User userFound = await _context.Users.FirstOrDefaultAsync(u => u.ID == id);
            if (userFound != null)
            {
                userFound.Password = null;
                List<ComicBook> comicsFound = await _context.Comics.Where(u => u.User.ID == userFound.ID).Include(c => c.Creators).Include(c => c.Conidition).ToListAsync();
                List<GraphicNovel> novelsFound = await _context.GraphicNovels.Where(g => g.User.ID == userFound.ID).Include(g => g.Creators).Include(g => g.BookCondition).ToListAsync();
                List<Collectible> collectiblesFound = await _context.Collectibles.Where(c => c.User.ID == userFound.ID).ToListAsync();
                return Ok(new
                {
                    statusMessage = "User information found!",
                    user = userFound,
                    comics = comicsFound,
                    graphicNovels = novelsFound,
                    collectibles = collectiblesFound,
                    comicsCount = comicsFound.Count(),
                    graphicNovelCount = novelsFound.Count(),
                    collectiblesCount = collectiblesFound.Count(),

                });
            }
            return BadRequest(new { statusMessage = "User with that username does not exist." });
        }
    }
}
