using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using ComicsShowcase.Models;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ComicsShowcase.Controllers
{
    [Authorize(Roles = "User")]
    [Route("api/[controller]")]
    public class ComicsController : Controller
    {
        private readonly ComicsContext _context;
        public ComicsController(ComicsContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetComics()
        {
            int uID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var query = from c in _context.Comics where c.User.ID == uID select c;
            if (query.Any())
            {
                ComicBook[] comics = await query.ToArrayAsync();
                return Ok(new { statusMessage = "Comic books retrieved.", comics });
            }
            else
            {
                return Ok(new { statusMessage = "No comic books to show." });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetComic([FromBody]int id)
        {
            int uID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            ComicBook comicFound = await _context.Comics.FirstOrDefaultAsync(c => c.ID == id);
            if (comicFound != null)
            {
                return Ok(new { statusMessage = "Comic information retrieved!", comic = comicFound });
            }
            return BadRequest(new { statusMessage = "Unable to find comic." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComic([FromBody] int id)
        {
            int uID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            ComicBook comicFound = await _context.Comics.FirstOrDefaultAsync(c => c.ID == id && c.User.ID == uID);
            if(comicFound != null)
            {
                _context.Comics.Remove(comicFound);
                await _context.SaveChangesAsync();
                return Ok(new { statusMessage = "Comic book deleted!" });
            }
            return BadRequest(new { statusMessage = "Comic book could not be deleted at this time." });
        }

        [HttpPost]
        public async Task<IActionResult> CreateComic([FromBody]ComicBook comicModel)
        {
            try{
                comicModel.User = _context.Users.FirstOrDefault(u => u.ID == int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(comicModel.ImageStr))
                    {
                        string[] imgData = comicModel.ImageStr.Split(new[] { "base64," }, StringSplitOptions.None);
                        comicModel.ImageStr = imgData[0];
                        comicModel.ImageData = Convert.FromBase64String(imgData[1]);
                    }
                    await _context.Comics.AddAsync(comicModel);
                    await _context.SaveChangesAsync();
                    return Ok(new { statusMessage = "Comic added successfully!", comic = comicModel });
                }
                return BadRequest(ModelState);
            }catch(Exception e){
                return BadRequest(new { statusMessage = e.Message });
            }
        }
    }
}
