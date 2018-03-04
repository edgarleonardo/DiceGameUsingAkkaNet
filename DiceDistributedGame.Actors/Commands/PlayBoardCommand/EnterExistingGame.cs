using DiceDistributedGame.Model.Player;

namespace DiceDistributedGame.Actors.Commands.PlayBoardCommand
{
    public class EnterExistingGame
    {
        public string GameId { get; private set; }
        public Player PlayerInfo { get; private set; }
        public EnterExistingGame(Player playerInfo, string gameId)
        {
            this.PlayerInfo = playerInfo;
            this.GameId = gameId;
        }
    }
}
