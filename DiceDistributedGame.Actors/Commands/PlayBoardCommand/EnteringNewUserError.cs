namespace DiceDistributedGame.Actors.Commands.PlayBoardCommand
{
    public class EnteringNewUserError
    {
        public bool UserAlreadyAdded { get; private set; }
        public bool GameAlreadyStarted { get; private set; }
        public bool GameAlreadyFinished { get; private set; }
        public bool GameWithNotEnoughPlayer { get; private set; }
        public bool PlayerDoentExistsRegistered { get; private set; }
        public bool PlayerNotInTurn { get; private set; }
        public EnteringNewUserError(bool userAlreadyAdded,
            bool gameAlreadyStarted,
            bool gameAlreadyFinished,
            bool gameWithNotEnoughPlayer,
            bool playerDoentExistsRegistered,
            bool playerNotInTurn)
        {
            this.UserAlreadyAdded = userAlreadyAdded;
            this.GameAlreadyStarted = gameAlreadyStarted;
            this.GameAlreadyFinished = gameAlreadyFinished;
            this.GameWithNotEnoughPlayer = gameWithNotEnoughPlayer;
            this.PlayerDoentExistsRegistered = playerDoentExistsRegistered;
            this.PlayerNotInTurn = playerNotInTurn;
        }
        public EnteringNewUserError(bool userAlreadyAdded,
          bool gameAlreadyStarted,
          bool gameAlreadyFinished,
          bool gameWithNotEnoughPlayer,
          bool playerDoentExistsRegistered)
        {
            this.UserAlreadyAdded = userAlreadyAdded;
            this.GameAlreadyStarted = gameAlreadyStarted;
            this.GameAlreadyFinished = gameAlreadyFinished;
            this.GameWithNotEnoughPlayer = gameWithNotEnoughPlayer;
            this.PlayerDoentExistsRegistered = playerDoentExistsRegistered;
        }
    }
}
