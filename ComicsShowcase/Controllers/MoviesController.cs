using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using ComicsShowcase.Models;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ComicsShowcase.Controllers
{
    [Authorize(Roles = "User")]
    [Route("api/[controller]")]
    public class MoviesController : Controller
    {
        private readonly ComicsContext _context;
        public MoviesController(ComicsContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetMovies()
        {
            int uID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            List<Movie> moviesFound = await .Movies.Where(m => m.User.ID == uID).ToListAsync();
            if (moviesFound.Any())
            {
                moviesFound.ForEach(m => {
                    if(m.ImageStr != null && m.ImageData != null)
                    {
                        m.ImageStr = m.ImageStr + "base64," + Convert.ToBase64String(m.ImageData);
                    }
                });
                return Ok(new {statusMessage = "Movies found.", movies = moviesFound});
            }
            return BadRequest(new {statusMessage = "No movies found."});
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovie(int id)
        {
            int uID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            Movie movieFound = await _context.Movies.Include(m => m.User).FirstOrDefaultAsync(m => m.User.ID == uID);
            if(movieFound != null)
            {
                if(movieFound.ImageStr != null && movieFound.ImageData != null)
                {
                    movieFound.ImageStr = movieFound.ImageStr + "base64," + Convert.ToBase64String(movieFound.ImageData);
                }
                return Ok(new {statusMessage = "Movie information retrieved.", movie = movieFound});
            }
            return BadRequest(new {statusMessage = "Unable to fetch movie information."});
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            int uID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            Movie movieFound = await _context.Movies.FirstOrDefaultAsync(m => m.User.ID == uID && m.ID == id);
            if(movieFound != null)
            {
                _context.Movies.Remove(movieFound);
                await _context.SaveChangesAsync();
                return Ok(new {statusMessage = "Movie deleted successfully!"});
            }
            return BadRequest(new {statusMessage = "Unable to delete movie at this time."});
        }
        [HttpPost]
        public async Task<IActionResult> CreateMovie([FromBody] Movie movieModel)
        {
            int uID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            movieModel.User = await _context.Users.FirstOrDefaultAsync(u => u.ID == uID);
            var validationContext = new ValidationContext(movieModel, null, null);
            List<ValidationResult> results = new List<ValidationResult>();
            if (Validator.TryValidateObject(movieModel, validationContext, results, true))
            {
                if(!string.IsNullOrEmpty(movieModel.ImageStr))
                {
                    string[] imgData = movieModel.ImageStr.Split(new[] { "base64," }, StringSplitOptions.None);
                    movieModel.ImageStr = imgData[0];
                    movieModel.ImageData = Convert.FromBase64String(imgData[1]);
                }
                await _context.Movies.AddAsync(movieModel);
                await _context.SaveChangesAsync();
                return Ok(new {statusMessage = "Movie added!", movie = movieModel});
            }
            return BadRequest(new {statusMessage = "Unable to add movie at this time."});
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMovie([FromBody] Movie movieModel)
        {
            int uID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            movieModel.User = await _context.Users.FirstOrDefaultAsync(u => u.ID == uID);
            var validationContext = new ValidationContext(movieModel, null, null);
            List<ValidationResult> results = new List<ValidationResult>();
            if (Validator.TryValidateObject(movieModel, validationContext, results, true))
            {
                if (!string.IsNullOrEmpty(movieModel.ImageStr))
                {
                    string[] imgData = movieModel.ImageStr.Split(new[] { "base64," }, StringSplitOptions.None);
                    movieModel.ImageStr = imgData[0];
                    movieModel.ImageData = Convert.FromBase64String(imgData[1]);
                }
                _context.Movies.Update(movieModel);
                await _context.SaveChangesAsync();
                return Ok(new { statusMessage = "Movie updated!", movie = movieModel });
            }
            return BadRequest(new { statusMessage = "Unable to update movie at this time." });
        }
    }
}
