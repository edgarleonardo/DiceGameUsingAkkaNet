using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using DiceDistributedGame.Actors.Actors;
using DiceDistributedGameApplication.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace DiceDistributedGameApplication.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private IHubContext<NotificationHub> _hub;
        private ActorSystem _actorSystem;
        public ValuesController(IHubContext<NotificationHub> hub, ActorSystem actorSystem)
        {
            this._hub = hub;
            this._actorSystem = actorSystem;
        }
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [Route("api/CreateGame")]
        [HttpPost]
        public void CreateGame([FromBody]string name)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
