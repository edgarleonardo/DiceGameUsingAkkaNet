using System;

namespace DiceDistributedGame.Model.Player
{
    public class Player
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string SignalRConnectionId { get; private set; }
        public Player(
            string name,
            string signalRConnectionId
            )
        {
            this.Id = Guid.NewGuid().ToString();
            this.Name = name;
            this.SignalRConnectionId = signalRConnectionId;
        }
    }
}
