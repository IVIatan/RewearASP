using Microsoft.AspNetCore.Mvc;
using RewearApi.BL;
using RewearApi.DAL;
using System.Collections.Generic;
using System.Linq;

namespace RewearApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClothingItemsController : ControllerBase
    {
        private readonly ClothingItemDAL _clothingDal = new ClothingItemDAL();

        [HttpGet]
        public ActionResult<List<ClothingItem>> Get()
        {
            List<ClothingItem> items = _clothingDal.GetAllClothingItems();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public ActionResult<ClothingItem> Get(int id)
        {
            ClothingItem? item = _clothingDal.GetClothingItemById(id);

            if (item == null)
            {
                return NotFound($"Clothing item with id {id} was not found");
            }

            return Ok(item);
        }

        [HttpGet("owner/{ownerUserId}")]
        public ActionResult<List<ClothingItem>> GetByOwner(int ownerUserId)
        {
            List<ClothingItem> items = _clothingDal.GetClothingItemsByOwnerUserId(ownerUserId);

            if (items == null || items.Count == 0)
            {
                return NotFound($"No clothing items found for ownerUserId {ownerUserId}");
            }

            return Ok(items);
        }

        [HttpGet("store/{storeId}")]
        public ActionResult<List<ClothingItem>> GetByStore(int storeId)
        {
            List<ClothingItem> items = _clothingDal.GetClothingItemsByStoreId(storeId);

            if (items == null || items.Count == 0)
            {
                return NotFound($"No clothing items found for storeId {storeId}");
            }

            return Ok(items);
        }

        [HttpPost]
        public ActionResult Post([FromBody] ClothingItem item)
        {
            if (item == null)
            {
                return BadRequest("ClothingItem object is null");
            }

            var errors = item.Validate();
            if (errors.Any())
            {
                return BadRequest(errors);
            }

            int newId = _clothingDal.AddClothingItem(item);
            item.ItemId = newId;

            return CreatedAtAction(nameof(Get), new { id = newId }, item);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] ClothingItem item)
        {
            if (item == null)
            {
                return BadRequest("ClothingItem object is null");
            }

            if (id != item.ItemId)
            {
                return BadRequest("Id in URL does not match ClothingItem.ItemId");
            }

            var errors = item.Validate();
            if (errors.Any())
            {
                return BadRequest(errors);
            }

            int rowsAffected = _clothingDal.UpdateClothingItem(item);
            if (rowsAffected == 0)
            {
                return NotFound($"Clothing item with id {id} was not found");
            }

            return Ok($"Clothing item with id {id} was updated successfully");
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            int rows = _clothingDal.DeleteClothingItem(id);

            if (rows == 0)
            {
                return NotFound($"Clothing item with id {id} was not found");
            }

            return Ok($"Clothing item with id {id} was deleted successfully");
        }
        [HttpPost("{id}/image")]
        public IActionResult UploadImage(int id, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("לא התקבל קובץ");
                }

                string wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                string imagesPath = Path.Combine(wwwrootPath, "images");

                if (!Directory.Exists(imagesPath))
                {
                    Directory.CreateDirectory(imagesPath);
                }

                string extension = Path.GetExtension(file.FileName);
                string fileName = $"{Guid.NewGuid()}{extension}";
                string fullPath = Path.Combine(imagesPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                string relativePath = $"/images/{fileName}";

                ClothingItemDAL dal = new ClothingItemDAL();
                int rows = dal.UpdateImage(id, relativePath);

                if (rows == 0)
                {
                    return NotFound("לא נמצא פריט עם הקוד הזה");
                }

                return Ok(new { imagePath = relativePath });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
