using Hangfire_Demo.Models;
using Hangfire_Demo.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hangfire_Demo.Controllers
{
    public class productcontroller : ControllerBase
    {
        public static List<product> products = new List<product>();
        private readonly Iservice _iservice;

        public productcontroller(Iservice iservice)
        {
            _iservice = iservice;
        }
        public IActionResult Addproduct(product product)
        {
            if (ModelState.IsValid)
            {
                products.Add(product);
                _iservice.InsertRecords(product);
                BackgroundJob.Enqueue<Iservice>(x => x.SendEmail());
                return CreatedAtAction("GetProduct", new { product.id }, product);
            }
            return BadRequest();
        }

        [HttpGet]
        public IActionResult GetProduct(int id)
        {
            var product = _iservice.GetAllRecords();

            var product = products.FirstOrDefault(x => x.id == id);
            if (product == null)
                return NotFound();
            BackgroundJob.Enqueue<Iservice>(x => x.SyncData());
            return Ok(product);
        }

    }
}
       