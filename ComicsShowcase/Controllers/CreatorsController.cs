using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ComicsShowcase.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ModelBinding;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ComicsShowcase.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class CreatorsController : Controller
    {
        private readonly ComicsContext _context;

        public CreatorsController(ComicsContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCreator([FromBody]Creator creatorModel)
        {
            int uID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            creatorModel.User = await _context.Users.SingleOrDefaultAsync(u => u.ID == uID);
            if (ModelState.IsValid)
            {
                await _context.Creators.AddAsync(creatorModel);
                await _context.SaveChangesAsync();
                return Ok(new { statusMessage = "Creator added.", creator = creatorModel });
            }else
            {
                return BadRequest(new { statusMessage = "Unable to add creator at this time. " + ModelState });
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCreator(int id)
        {
            int uID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            Creator creatorFound = await _context.Creators.Include(c => c.User).FirstOrDefaultAsync(c => c.ID == id && c.User.ID == uID);
            if(creatorFound != null){
                return Ok(new {creator = creatorFound, statusMessage = "Creator information retreived."});
            }
            return BadRequest(new {statusMessage = "Unable to retrieve creator information."});
        }

        [HttpGet]
        public async Task<IActionResult> GetCreators()
        {
            int uID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            List<Creator> creatorsFound = await _context.Creators.Include(c => c.User).Where(c => c.User.ID == uID).ToListAsync();
            if(creatorsFound != null && creatorsFound.Count() > 0)
            {
                return Ok(new {statusMessage = "Creators found.", creators = creatorsFound});
            }
            return BadRequest(new {statusMessage = "Unable to retrieve creators or no creators exist."});
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCreator(int id)
        {
            int uID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            Creator creatorFound = await _context.Creators.Include(c => c.User).FirstOrDefaultAsync(c => c.ID == id && c.User.ID == uID);
            if(creatorFound != null)
            {
                _context.Creators.Remove(creatorFound);
                await _context.SaveChangesAsync();
                return Ok(new {statusMessage = "Creator removed successfully!"});
            }
            return BadRequest(new {statusMessage = "Unable to remove creator."});
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCreator([FromBody] Creator creatorModel)
        {
            int uID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if(ModelState.IsValid)
            {
                _context.Creators.Update(creatorModel);
                await _context.SaveChangesAsync();
                return Ok(new {statusMessage = "Creator updated."});
            }
            return BadRequest(new {statusMessage = "Unable to update creator. " + ModelState});
        }
    }
}
