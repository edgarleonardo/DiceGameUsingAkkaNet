using DiceDistributedGame.Actors.Commands.PlayBoardCommand;

namespace DiceDistributedGame.Actors.Commands.GameCommands
{
    public class StartNewGame
    {
        public CreateNewGame CreateGameRequest { get; private set; }
        public StartNewGame(CreateNewGame createGameRequest)
        {
            this.CreateGameRequest = createGameRequest;
        }
    }
}
