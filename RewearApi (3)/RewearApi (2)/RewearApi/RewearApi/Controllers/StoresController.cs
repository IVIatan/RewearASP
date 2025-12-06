using Microsoft.AspNetCore.Mvc;
using RewearApi.BL;
using RewearApi.DAL;
using System.Collections.Generic;
using System.Linq;

namespace RewearApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly StoreDAL _storeDal = new StoreDAL();


        [HttpGet]
        public ActionResult<List<Store>> Get()
        {
            List<Store> stores = _storeDal.GetAllStores();
            return Ok(stores);
        }


        [HttpGet("{id}")]
        public ActionResult<Store> Get(int id)
        {
            Store? store = _storeDal.GetStoreById(id);

            if (store == null)
            {
                return NotFound($"Store with id {id} was not found");
            }

            return Ok(store);
        }

        [HttpPost]
        public ActionResult Post([FromBody] Store store)
        {
            if (store == null)
            {
                return BadRequest("Store object is null");
            }

            var errors = store.Validate();
            if (errors.Any())
            {
                return BadRequest(errors);
            }

            int newId = _storeDal.AddStore(store);
            store.StoreId = newId;

            return CreatedAtAction(nameof(Get), new { id = newId }, store);
        }


        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Store store)
        {
            if (store == null)
            {
                return BadRequest("Store object is null");
            }

            if (id != store.StoreId)
            {
                return BadRequest("Id in URL does not match Store.StoreId");
            }

            var errors = store.Validate();
            if (errors.Any())
            {
                return BadRequest(errors);
            }

            int rows = _storeDal.UpdateStore(store);
            if (rows == 0)
            {
                return NotFound($"Store with id {id} was not found");
            }

            return Ok($"Store with id {id} was updated successfully");
        }
    }
}
