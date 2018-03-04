using DiceDistributedGame.Actors.Commands.PlayBoardCommand;
using System.Collections.Generic;

namespace DiceDistributedGame.Actors.Events.PlayBoardEvent
{
    public class PlayBoardEvent
    {
        public Dictionary<string, GameRegister> GameRegistered { get; private set; }
        public PlayBoardEvent()
        {
            GameRegistered = new Dictionary<string, GameRegister>();
        }
    }
}
