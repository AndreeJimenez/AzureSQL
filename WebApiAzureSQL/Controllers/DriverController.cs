using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiAzureSQL.Model;

namespace WebApiAzureSQL.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        [HttpGet]
        public List<DriverModel> Get()
        {
            return new DriverModel().GetAll();
        }

        [HttpGet("{id}", Name = "Get")]
        public DriverModel Get(int id)
        {
            return new DriverModel().Get(id);
        }

        [HttpPost]
        public ApiResponse Post([FromBody] DriverModel driver)
        {
            return driver.Insert();
        }

        [HttpPut("{id}")]
        public ApiResponse Put(int id, [FromBody] DriverModel driver)
        {
            return driver.Update(id);
        }

        [HttpDelete("{id}")]
        public ApiResponse Delete(int id)
        {
            return new DriverModel().Delete(id);
        }
    }
}
