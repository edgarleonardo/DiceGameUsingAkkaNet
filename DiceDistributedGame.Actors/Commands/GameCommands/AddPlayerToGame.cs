using DiceDistributedGame.Actors.Commands.PlayBoardCommand;

namespace DiceDistributedGame.Actors.Commands.GameCommands
{
    public class AddPlayerToGame
    {
        public EnterExistingGame EnterExistingGameRequest { get; private set; }
        public AddPlayerToGame(EnterExistingGame enterExistingGameRequest)
        {
            this.EnterExistingGameRequest = enterExistingGameRequest;
        }
    }
}
