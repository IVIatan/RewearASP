using Microsoft.AspNetCore.Mvc;
using RewearApi.BL;
using RewearApi.DAL;
using System.Collections.Generic;
using System.Linq;

namespace RewearApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderDAL _orderDal = new OrderDAL();

        [HttpGet]
        public ActionResult<List<Order>> Get()
        {
            List<Order> orders = _orderDal.GetAllOrders();
            return Ok(orders);
        }


        [HttpGet("{id}")]
        public ActionResult<Order> Get(int id)
        {
            Order? order = _orderDal.GetOrderById(id);

            if (order == null)
            {
                return NotFound($"Order with id {id} was not found");
            }

            return Ok(order);
        }


        [HttpPost]
        public ActionResult Post([FromBody] Order order)
        {
            if (order == null)
            {
                return BadRequest("Order object is null");
            }

            var errors = order.Validate();
            if (errors.Any())
            {
                return BadRequest(errors);
            }

            int newId = _orderDal.AddOrder(order);
            order.OrderId = newId;

            return CreatedAtAction(nameof(Get), new { id = newId }, order);
        }


        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Order order)
        {
            if (order == null)
            {
                return BadRequest("Order object is null");
            }

            if (id != order.OrderId)
            {
                return BadRequest("Id in URL does not match Order.OrderId");
            }

            var errors = order.Validate();
            if (errors.Any())
            {
                return BadRequest(errors);
            }

            int rows = _orderDal.UpdateOrder(order);
            if (rows == 0)
            {
                return NotFound($"Order with id {id} was not found");
            }

            return Ok($"Order with id {id} was updated successfully");
        }
    }
}
