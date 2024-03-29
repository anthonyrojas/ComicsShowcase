﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ComicsShowcaseV3.Models;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ComicsShowcaseV3.Controllers
{
    // api/collectibles
    [Authorize(Roles = "User")]
    [Route("api/[controller]")]
    public class CollectiblesController : Controller
    {
        private readonly ComicsContext _context;
        public CollectiblesController(ComicsContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAccountCollectibles()
        {
            int uID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            List<Collectible> collectibles = await _context.Collectibles.Where(u => u.User.ID == uID).ToListAsync();
            if(collectibles.Any() && collectibles != null)
            {
                collectibles.ForEach(c =>
                {
                    if(c.ImageData != null && c.ImageStr != null)
                    {
                        c.ImageStr = c.ImageStr + "base64," + Convert.ToBase64String(c.ImageData);
                    }
                });
            }
            return Ok(new { statusMessage = "Collectibles found!", collectibles = collectibles });
        }

        [HttpGet("user/{userID}")]
        public async Task<IActionResult> GetCollectibles([FromRoute]int userID)
        {
            var collectibles = await _context.Collectibles.Where(c => c.User.ID == userID).ToListAsync();
            if (collectibles.Any() && collectibles != null)
            {
                collectibles.ForEach(c => { 
                    if(c.ImageData != null && c.ImageStr != null)
                    {
                        //convert the image data byte array in image str property
                        c.ImageStr = c.ImageStr + "base64," + Convert.ToBase64String(c.ImageData);
                    }
                });
                return Ok(new {statusMessage = "Collectibles retrieved successfully.", collectibles});
            }
            return BadRequest(new {statusMessage = "No collectibles found."});
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCollectible(int id)
        {
            Collectible collectibleFound = await _context.Collectibles.Include(c => c.User).FirstOrDefaultAsync(c => c.ID == id);
            collectibleFound.User.BirthDate = null;
            collectibleFound.User.Password = null;
            if(collectibleFound != null)
            {
                if(collectibleFound.ImageStr != null && collectibleFound.ImageData != null)
                {
                    collectibleFound.ImageStr = collectibleFound.ImageStr + "base64," + Convert.ToBase64String(collectibleFound.ImageData);
                }
                return Ok(new {statusMessage = "Collectible retrieved.", collectible = collectibleFound});
            }
            return BadRequest(new {statusMessage = "Unable to retrieve collectible information."});
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveCollectible(int id)
        {
            int uID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            Collectible collectibleFound = await _context.Collectibles.FirstOrDefaultAsync(c => c.ID == id && c.User.ID == uID);
            if(collectibleFound != null)
            {
                _context.Collectibles.Remove(collectibleFound);
                await _context.SaveChangesAsync();
                return Ok(new {statusMessage = "Collectible removed!"});
            }
            return BadRequest(new {statusMessage = "Unable to remove collectible at this time."});
        }

        [HttpPost]
        public async Task<IActionResult> CreateCollectible([FromBody] Collectible collectibleModel)
        {
            int uID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            collectibleModel.User = await _context.Users.FirstOrDefaultAsync(u => u.ID == uID);
            var validationContext = new ValidationContext(collectibleModel, null, null);
            var results = new List<ValidationResult>();
            if(Validator.TryValidateObject(collectibleModel, validationContext, results, true))
            {
                if(!string.IsNullOrEmpty(collectibleModel.ImageStr))
                {
                    string[] imgData = collectibleModel.ImageStr.Split(new[] { "base64," }, StringSplitOptions.None);
                    collectibleModel.ImageStr = imgData[0];
                    collectibleModel.ImageData = Convert.FromBase64String(imgData[1]);
                }
                await _context.Collectibles.AddAsync(collectibleModel);
                await _context.SaveChangesAsync();
                return Ok(new {statusMessage = "Collectible added!", collectible = collectibleModel});
            }
            return BadRequest(new {statusMessage = "Unable to add collectible at this time.", errors = results});
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCollectible([FromBody] Collectible collectibleModel)
        {
            int uID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            collectibleModel.User = await _context.Users.FirstOrDefaultAsync(u => u.ID == uID);
            var validationContext = new ValidationContext(collectibleModel, null, null);
            var results = new List<ValidationResult>();
            if (Validator.TryValidateObject(collectibleModel, validationContext, results, true))
            {
                if (!string.IsNullOrEmpty(collectibleModel.ImageStr))
                {
                    string[] imgData = collectibleModel.ImageStr.Split(new[] { "base64," }, StringSplitOptions.None);
                    collectibleModel.ImageStr = imgData[0];
                    collectibleModel.ImageData = Convert.FromBase64String(imgData[1]);
                }
                _context.Collectibles.Update(collectibleModel);
                await _context.SaveChangesAsync();
                return Ok(new { statusMessage = "Collectible updated!", collectible = collectibleModel });
            }
            return BadRequest(new { statusMessage = "Unable to update collectible at this time.", errors = results });
        }
    }
}
