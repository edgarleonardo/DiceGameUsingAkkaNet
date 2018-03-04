using DiceDistributedGame.Model.Games;
using DiceDistributedGame.Model.Player;
using System.Collections.Generic;

namespace DiceDistributedGame.Actors.Events.GameEvent
{
    public class GameEvent
    {
        public string GameId { get; private set; }
        public Player PlayerInfo { get; private set; }
        public bool IsGameFinished { get; private set; }
        public bool IsGameStarted { get; private set; }
        public Dictionary<string, GameInfo> GamePlayPerPlayer { get; private set; }
        public Player Winner { get; private set; }
        public GameEvent(string id, Player playerInfo)
        {
            this.GameId = id;
            this.PlayerInfo = playerInfo;
            GamePlayPerPlayer = new Dictionary<string, GameInfo>();
            GamePlayPerPlayer.Add(PlayerInfo.Id, new GameInfo(PlayerInfo, GameId));
        }
        private void MarkNextToPlay(string CurrentUserId)
        {
            var first = "";
            var isTheNextTheOne = false;
            var isTheFist = true;
            foreach (var key in GamePlayPerPlayer.Keys)
            {
                if (isTheFist)
                {
                    first = key;
                    isTheFist = false;
                }
                if (isTheNextTheOne)
                {
                    GamePlayPerPlayer[key].MarkAsNextToPlay();
                    isTheNextTheOne = false;
                }
                if (CurrentUserId == key)
                {
                    isTheNextTheOne = true;
                    GamePlayPerPlayer[CurrentUserId].ClearMarkAsNextToPlay();
                }
            }
            if (isTheNextTheOne)
            {
                GamePlayPerPlayer[first].MarkAsNextToPlay();
            }
        }
        public GameStatusThrowResult ThrowDice(string UserId)
        {
            if (IsGameStarted == false)
            {
                IsGameStarted = true;
            }
            if (GamePlayPerPlayer[UserId].InTurn)
            {
                var result = GamePlayPerPlayer[UserId].ThrowDice();
                if (GamePlayPerPlayer[UserId].Position == GamePlayPerPlayer[UserId].Goal)
                {
                    Winner = GamePlayPerPlayer[UserId].PlayerInfo;
                    IsGameFinished = true;
                }
                else
                {
                    MarkNextToPlay(UserId);
                }
                return result;
            }
            else
            {
                return GameStatusThrowResult.NotPlayerTurn;
            }
        }
    }
}
