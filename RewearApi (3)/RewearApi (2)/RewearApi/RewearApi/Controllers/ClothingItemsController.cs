using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using RewearApi.BL;
using RewearApi.DAL;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RewearApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClothingItemsController : ControllerBase
    {
        private readonly ClothingItemDAL _dal = new ClothingItemDAL();

        [HttpGet]
        public IActionResult Get()
        {
            var items = _dal.GetAllClothingItems();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var item = _dal.GetClothingItemById(id);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public IActionResult Post([FromBody] ClothingItem item)
        {
            if (item == null)
                return BadRequest("לא התקבל פריט.");

            var errors = item.Validate();
            if (!string.IsNullOrEmpty(errors))
                return BadRequest(errors);

            var newId = _dal.AddClothingItem(item);
            item.ItemId = newId;

            return CreatedAtAction(nameof(Get), new { id = newId }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ClothingItem item)
        {
            if (item == null)
                return BadRequest("לא התקבל פריט.");

            if (id != item.ItemId)
                return BadRequest("אי התאמה בין ה־Id שב־URL ל־Id של הפריט.");

            var errors = item.Validate();
            if (!string.IsNullOrEmpty(errors))
                return BadRequest(errors);

            var rows = _dal.UpdateClothingItem(item);
            if (rows == 0)
                return NotFound();

            return Ok(item);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var rows = _dal.DeleteClothingItem(id);
            if (rows == 0)
                return NotFound();

            return NoContent();
        }

        [HttpPost("{id}/image")]
        public async Task<IActionResult> UploadImage(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("קובץ לא התקבל.");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var relativePath = $"/images/{fileName}";
            var rows = _dal.UpdateImage(id, relativePath);
            if (rows == 0)
                return NotFound();

            return Ok(new { ImagePath = relativePath });
        }
    }
}
