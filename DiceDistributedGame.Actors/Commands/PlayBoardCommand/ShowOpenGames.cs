

using DiceDistributedGame.Model.Games;
using System.Collections.Generic;

namespace DiceDistributedGame.Actors.Commands.PlayBoardCommand
{
    public class ShowOpenGames
    {
        public List<GameInfoForReport> OpenGames { get; private set; }
        public ShowOpenGames()
        {
            OpenGames = new List<GameInfoForReport>();
        }
    }
}
