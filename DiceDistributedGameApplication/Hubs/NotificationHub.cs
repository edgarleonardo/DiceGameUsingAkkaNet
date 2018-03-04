using Akka.Actor;
using DiceDistributedGame.Actors.Actors;
using DiceDistributedGame.Actors.Commands.PlayBoardCommand;
using DiceDistributedGame.Model.Player;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace DiceDistributedGameApplication.Hubs
{
    public class NotificationHub : Hub
    {
        private ActorSystem _actorSystem;
        private IActorRef PlayerCoordinator;
        public NotificationHub(ActorSystem actorSystem)
        {
            this._actorSystem = actorSystem;
        }
        public string ConnectionId { get; set; }
        public override Task OnConnectedAsync()
        {
            ConnectionId = Context.ConnectionId;
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
        public void SelectExistingsGames(Player player, string gameId)
        {
            var messageShowGames = PlayerCoordinator.Ask<ShowOpenGames>(new ShowOpenGames()).Result;
            if (messageShowGames != null && messageShowGames.OpenGames != null &&
                messageShowGames.OpenGames.Count > 0 &&
                messageShowGames.OpenGames.Exists(g => g.GameId == gameId))
            {
                try
                {
                    var enterExist = new EnterExistingGame(player, gameId);
                    Clients.Client(ConnectionId).InvokeAsync("OnGameRegistrationComplete", gameId);
                }
                catch
                {

                }
            }
        }
        public  void ShowGames()
        {
            var result = PlayerCoordinator.Ask<ShowOpenGames>(new ShowOpenGames()).Result;
            Clients.Client(ConnectionId).InvokeAsync("SendOpenedGames", result.OpenGames);
        }
        public void CreateGame(string name)
        {
            var actorRef = _actorSystem.ActorOf(Props.Create<PlayBoardActor>());
            var playerObject = new Player(name, ConnectionId);
            var gameRequest = new CreateNewGame(playerObject);
            actorRef.Tell(gameRequest);
            /// Accesing to the game
            SelectExistingsGames(playerObject, gameRequest.Id);
        }
    }
}
