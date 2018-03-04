
namespace DiceDistributedGame.Model.Games
{
    public class GameInfo
    {
        public string Id { get; private set; }
        public Player.Player PlayerInfo { get; private set; }
        public int Position { get; private set; }
        public bool InTurn { get; private set; }
        public int Goal { get { return 100; } }
        public GameInfo(Player.Player playerInfo, string id)
        {
            this.PlayerInfo = playerInfo;
            this.Id = id;
        }
        public void MarkAsNextToPlay()
        {
            InTurn = true;
        }
        public void ClearMarkAsNextToPlay()
        {
            InTurn = false;
        }
        public GameStatusThrowResult ThrowDice()
        {
            var random = new System.Random();
            int value = random.Next(1, 6);
            if ((value + Position) <= Goal)
            {
                Position += value;
                return GameStatusThrowResult.ThrowExecuted;
            }
            else if ((value + Position) <= Goal)
            {
                return GameStatusThrowResult.ThrowBiggerThanGoal;
            }
            else
            {
                return GameStatusThrowResult.ThrowWithVictory;
            }
        }
    }
}
