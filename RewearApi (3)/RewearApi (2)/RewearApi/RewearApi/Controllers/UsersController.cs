using Microsoft.AspNetCore.Mvc;
using RewearApi.BL;
using RewearApi.DAL;
using System.Collections.Generic;
using System.Linq;

namespace RewearApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserDAL _userDal = new UserDAL();


        [HttpGet]
        public ActionResult<List<User>> Get()
        {
            List<User> users = _userDal.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            User? user = _userDal.GetUserById(id);

            if (user == null)
            {
                return NotFound($"User with id {id} was not found");
            }

            return Ok(user);
        }

        [HttpPost]
        public ActionResult Post([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("User object is null");
            }

            var errors = user.Validate();
            if (errors.Any())
            {
                return BadRequest(errors);  
            }

            int newId = _userDal.AddUser(user);
            user.UserId = newId;

            return CreatedAtAction(nameof(Get), new { id = newId }, user);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("User object is null");
            }

            if (id != user.UserId)
            {
                return BadRequest("Id in URL does not match User.UserId");
            }

            var errors = user.Validate();
            if (errors.Any())
            {
                return BadRequest(errors);
            }

            int rowsAffected = _userDal.UpdateUser(user);
            if (rowsAffected == 0)
            {
                return NotFound($"User with id {id} was not found");
            }

            return Ok($"User with id {id} was updated successfully");
        }
    }
}
