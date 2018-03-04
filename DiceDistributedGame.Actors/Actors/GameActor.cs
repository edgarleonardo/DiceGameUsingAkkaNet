using Akka.Actor;
using DiceDistributedGame.Actors.Commands.GameCommands;
using DiceDistributedGame.Actors.Commands.PlayBoardCommand;
using DiceDistributedGame.Actors.Commands.PlayerCommand;
using DiceDistributedGame.Actors.Events.GameEvent;
using DiceDistributedGame.Actors.ExternalSystem;
using DiceDistributedGame.Model.Games;
using System;
namespace DiceDistributedGame.Actors.Actors
{
    public class GameActor : ReceiveActor
    {
        public GameEvent GameStatus = null;
        public GameActor()
        {
            Receive<StartNewGame>(message => StartNewGameMethod(message));
            Receive<AddPlayerToGame>(message => AddPlayerToGameMethod(message));
            Receive<ThrowDice>(message => ThrowDiceMethod(message));
            Receive<FinishGame>(message => FinishGameMethod(message));
        }

        private void FinishGameMethod(FinishGame message)
        {
            if (GameStatus.GamePlayPerPlayer.ContainsKey(message.PlayerInfo.Id))
            {
                GameStatus.GamePlayPerPlayer.Remove(message.PlayerInfo.Id);
                Context.Parent.Tell(new GameRegister(GameStatus));
            }
            else
            {
                var messageAnswer = new EnteringNewUserError(false, false, false, false, true);
                Context.Parent.Tell(messageAnswer);

            }
        }

        private void ThrowDiceMethod(ThrowDice message)
        {
            if (GameStatus.IsGameFinished)
            {
                var messageAnswer = new EnteringNewUserError(false, false, true, false, false);
                Context.Parent.Tell(messageAnswer);
            }
            else
            /// thow an error
            if (GameStatus.GamePlayPerPlayer.Count <= 1)
            {
                var messageAnswer = new EnteringNewUserError(false, false, false, true, false);
                Context.Parent.Tell(messageAnswer);
            }
            else
            {
                var result = GameStatus.ThrowDice(message.PlayerInfo.Id);
                if (result == GameStatusThrowResult.NotPlayerTurn)
                {
                    var messageAnswer = new EnteringNewUserError(false, false, false, false, false, true);
                    Context.Parent.Tell(messageAnswer);
                }
                else if (result == GameStatusThrowResult.ThrowExecuted || result == GameStatusThrowResult.ThrowWithVictory)
                {
                    Context.Parent.Tell(new GameRegister(GameStatus));
                }
            }
        }

        private void AddPlayerToGameMethod(AddPlayerToGame message)
        {
            EnteringNewUserError messageAnswer = null;
            if (GameStatus.IsGameFinished)
            {
                messageAnswer = new EnteringNewUserError(false, false, true, false, false);
                return;
            }else
            /// User Already Exists
            if (GameStatus.GamePlayPerPlayer.ContainsKey(message.EnterExistingGameRequest.PlayerInfo.Id))
            {
                messageAnswer = new EnteringNewUserError(true, false, false, false, false);
                return;
            } else
            /// Game is finished
            if (!GameStatus.GamePlayPerPlayer.ContainsKey(message.EnterExistingGameRequest.PlayerInfo.Id))
            {               
                if (GameStatus.IsGameStarted)
                {
                    messageAnswer = new EnteringNewUserError(false, true, false, false, false);
                    return;
                }
            }

            if (messageAnswer != null)
            {
                Context.Parent.Tell(messageAnswer);
            }
            else
            {
                GameStatus.GamePlayPerPlayer.Add(message.EnterExistingGameRequest.PlayerInfo.Id, 
                    new GameInfo(message.EnterExistingGameRequest.PlayerInfo,
                    message.EnterExistingGameRequest.GameId));
                Context.Parent.Tell(new GameRegister(GameStatus));
            }
        }

        private void StartNewGameMethod(StartNewGame message)
        {
            GameStatus = new GameEvent(message.CreateGameRequest.Id,
                 message.CreateGameRequest.PlayerInfo);
            Context.Parent.Tell(new GameRegister(GameStatus));
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(
                maxNrOfRetries: 10,
                withinTimeRange: TimeSpan.FromMinutes(1),
                localOnlyDecider: ex =>
                {
                    switch (ex)
                    {
                        case ArithmeticException ae:
                            return Directive.Resume;
                        case NullReferenceException nre:
                            return Directive.Restart;
                        case ArgumentException are:
                            return Directive.Stop;
                        default:
                            return Directive.Escalate;
                    }
                });
        }
    }
}
