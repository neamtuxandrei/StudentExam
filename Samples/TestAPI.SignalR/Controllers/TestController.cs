using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
using TestAPI.SignalR.DataModels;
using TestAPI.SignalR.SignalR;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestAPI.SignalR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private IHubContext<TestHub> hubContext;

        public TestController(IHubContext<TestHub> hub) 
        {
            hubContext = hub;
        }

        // GET: api/<TestController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        // POST api/<TestController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TestMessage message)
        {
            await hubContext.Clients.All.SendAsync("postnewmessage", JsonSerializer.Serialize(message));
            return Ok();
        }

      
    }
}
