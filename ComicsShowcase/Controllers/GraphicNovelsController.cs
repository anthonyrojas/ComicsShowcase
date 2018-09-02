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
            List<GraphicNovel> graphicNovelsFound = await _context.GraphicNovels.Where(g => g.User.ID == uID).ToListAsync();
            if(graphicNovelsFound.Any()){
                return Ok(new {statusMessage = "Graphic novels retrieved!", graphicNovels = graphicNovelsFound});
            }
            return BadRequest(new {statusMessage = "No graphic novels found."});
        }
    }
}
