using DiceDistributedGame.Actors.ExternalSystem;
using DiceDistributedGame.Model.Games;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;

namespace DiceDistributedGameApplication.ActorSystemFolder
{
    public class SignalRPusher : IGameEventsPusher
    {
        private IHubContext<Hub> _hub;
        public SignalRPusher(IHubContext<Hub> hub)
        {
            this._hub = hub;
        }
        public void NotifyGameCreated(string gameId, string connectionId)
        {
            _hub.Clients.Client(connectionId).InvokeAsync("GameCreated", gameId);
        }
        
    }
}