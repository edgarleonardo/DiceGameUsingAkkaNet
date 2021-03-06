﻿using DiceDistributedGame.Model.Player;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiceDistributedGame.Actors.Commands.PlayBoardCommand
{
    public class UserRegistrationDone
    {
        public string GameId { get; private set; }
        public Player PlayerInfo { get; private set; }
        public UserRegistrationDone(Player playerInfo, string gameId)
        {
            this.PlayerInfo = playerInfo;
            this.GameId = gameId;
        }
    }
}
