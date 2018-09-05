using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ComicsShowcase.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ComicsShowcase.Controllers
{
    // api/graphicnovels
    [Authorize(Roles = "User")]
    [Route("api/[controller]")]
    public class GraphicNovelsController : Controller
    {
        private readonly ComicsContext _context;
        public GraphicNovelsController(ComicsContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetGraphicNovels()
        {
            int uID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            List<GraphicNovel> graphicNovelsFound = await _context.GraphicNovels.Include(g => g.Creators).Where(g => g.User.ID == uID).ToListAsync();
            if(graphicNovelsFound.Any()){
                graphicNovelsFound.ForEach(g => {
                    if(g.ImageStr != null && g.ImageData != null)
                    {
                        g.ImageStr = g.ImageStr + "base64," + Convert.ToBase64String(g.ImageData);
                    }
                });
                return Ok(new {statusMessage = "Graphic novels retrieved!", graphicNovels = graphicNovelsFound});
            }
            return BadRequest(new {statusMessage = "No graphic novels found."});
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGraphicNovel(int id)
        {
            int uID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            GraphicNovel novelFound = await _context.GraphicNovels.Include(g => g.Creators).Include(g => g.User).FirstOrDefaultAsync(g => g.User.ID == uID && g.ID == id);
            if(novelFound != null)
            {
                if(novelFound.ImageStr != null && novelFound.ImageData != null)
                {
                    novelFound.ImageStr = novelFound.ImageStr + "base64," + Convert.ToBase64String(novelFound.ImageData);
                }
                return Ok(new {statusMessage = "Graphic novel retrieved.", graphicNovel = novelFound});
            }
            return BadRequest(new {statusMessage = "Graphic novel information not found."});
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteGraphicNovel(int id)
        {
            int uID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            GraphicNovel novelFound = await _context.GraphicNovels.FirstOrDefaultAsync(g => g.ID == id && g.User.ID == uID);
            if(novelFound != null)
            {
                _context.GraphicNovels.Remove(novelFound);
                await _context.SaveChangesAsync();
                return Ok(new {statusMessage = "Graphic novel deleted successfully!"});
            }
            return BadRequest(new {statusMessage = "Unable to delete graphic novel at this time."});
        }
        [HttpPost]
        public async Task<IActionResult> CreateGraphicNovel([FromBody] GraphicNovel graphicNovelModel)
        {
            int uID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            graphicNovelModel.User = await _context.Users.FirstOrDefaultAsync(u => u.ID == uID);
            var validationContext = new ValidationContext(graphicNovelModel, null, null);
            List<ValidationResult> results = new List<ValidationResult>();
            if(Validator.TryValidateObject(graphicNovelModel, validationContext, results, true))
            {
                if(!string.IsNullOrEmpty(graphicNovelModel.ImageStr))
                {
                    string[] imgData = graphicNovelModel.ImageStr.Split(new[] { "base64," }, StringSplitOptions.None);
                    graphicNovelModel.ImageStr = imgData[0];
                    graphicNovelModel.ImageData = Convert.FromBase64String(imgData[1]);
                }
                await _context.GraphicNovels.AddAsync(graphicNovelModel);
                await _context.SaveChangesAsync();
                return Ok(new {statusMessage = "Graphic novel added!", graphicNovel = graphicNovelModel});
            }
            return BadRequest(new {statusMessage = "Unable to add graphic novel at this time.", errors = results});
        }
        [HttpPut]
        public async Task<IActionResult> UpdateGraphicNovel([FromBody] GraphicNovel graphicNovelModel)
        {
            int uID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            graphicNovelModel.User = await _context.Users.FirstOrDefaultAsync(u => u.ID == uID);
            var validationContext = new ValidationContext(graphicNovelModel, null, null);
            var results = new List<ValidationResult>();
            if(Validator.TryValidateObject(graphicNovelModel, validationContext, results, true))
            {
                if (!string.IsNullOrEmpty(graphicNovelModel.ImageStr))
                {
                    string[] imgData = graphicNovelModel.ImageStr.Split(new[] { "base64," }, StringSplitOptions.None);
                    graphicNovelModel.ImageStr = imgData[0];
                    graphicNovelModel.ImageData = Convert.FromBase64String(imgData[1]);
                }
                _context.GraphicNovels.Update(graphicNovelModel);
                await _context.SaveChangesAsync();
                return Ok(new {statusMessage = "Graphic novel updated!", graphicNovel = graphicNovelModel});
            }
            return BadRequest(new {statusMessage = "Unable to update graphic novel at this time.", errors = results});
        }
    }
}
