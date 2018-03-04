using DiceDistributedGame.Actors.Events.GameEvent;
using DiceDistributedGame.Model.Player;

namespace DiceDistributedGame.Actors.Commands.PlayBoardCommand
{
    public class GameRegister
    {
        public GameEvent GameEventDashboard { get; private set; }
        
        public GameRegister(GameEvent gameEventDashboard
            )
        {
            this.GameEventDashboard = gameEventDashboard;
        }
    }
}
