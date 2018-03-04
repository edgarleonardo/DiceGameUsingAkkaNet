using DiceDistributedGame.Model.Player;
using System;

namespace DiceDistributedGame.Actors.Commands.PlayBoardCommand
{
    public class CreateNewGame
    {
        public string Id { get; private set; }
        public Player PlayerInfo { get; private set; }
        public CreateNewGame(Player player)
        {
            this.PlayerInfo = player;
            Id = Guid.NewGuid().ToString();
        }
    }
}
