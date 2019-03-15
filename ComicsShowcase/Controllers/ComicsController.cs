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
using System.ComponentModel.DataAnnotations;

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
        [HttpGet("user/{userID}")]
        public async Task<IActionResult> GetComics([FromRoute] int userID)
        {
            List<ComicBook> comicsFound = await _context.Comics.Include(c => c.User.Username).Include(c => c.Creators).Where(c => c.User.ID == userID).ToListAsync();
            if (comicsFound != null && comicsFound.Any())
            {
                comicsFound.ForEach(c => {
                    if(c.ImageStr != null && c.ImageData != null)
                    {
                        c.ImageStr = c.ImageStr + "base64," + Convert.ToBase64String(c.ImageData);
                    }
                });
                return Ok(new { statusMessage = "Comic books retrieved.", comics = comicsFound });
            }
            return BadRequest(new { statusMessage = "No comic books found." });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetComic([FromRoute]int id)
        {
            ComicBook comicFound = await _context.Comics.Include(c=> c.User).Include(c => c.Creators).FirstOrDefaultAsync(c => c.ID == id);
            if (comicFound != null)
            {
                if(comicFound.ImageStr != null && comicFound.ImageData != null)
                {
                    comicFound.ImageStr = comicFound.ImageStr + "base64," + Convert.ToBase64String(comicFound.ImageData);
                }
                return Ok(new { statusMessage = "Comic information retrieved!", comic = comicFound });
            }
            return BadRequest(new { statusMessage = "Unable to find comic." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComic([FromRoute] int id)
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
                comicModel.User = await _context.Users.FirstOrDefaultAsync(u => u.ID == int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
                var validationContext = new ValidationContext(comicModel, null, null);
                var results = new List<ValidationResult>();
                if (Validator.TryValidateObject(comicModel, validationContext, results, true))
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
                return BadRequest(new {statusMessage = "Unable to add comic.", errors = results});
            }catch(Exception e){
                return BadRequest(new { statusMessage = e.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateComic([FromBody] ComicBook comicModel)
        {
            int uID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            comicModel.User = await _context.Users.FirstOrDefaultAsync(u => u.ID == uID);
            var validationContext = new ValidationContext(comicModel, null, null);
            var results = new List<ValidationResult>();
            if(Validator.TryValidateObject(comicModel, validationContext, results, true))
            {
                if (!string.IsNullOrEmpty(comicModel.ImageStr))
                {
                    string[] imgData = comicModel.ImageStr.Split(new[] { "base64," }, StringSplitOptions.None);
                    comicModel.ImageStr = imgData[0];
                    comicModel.ImageData = Convert.FromBase64String(imgData[1]);
                }
                _context.Comics.Update(comicModel);
                await _context.SaveChangesAsync();
                return Ok(new {statusMessage = "Comic updated successfully!", comic = comicModel});
            }
            return BadRequest(new {statusmessage = "Unable to update comic at this time.", errors = results});
        }
    }
}
