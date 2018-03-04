using Akka.Actor;
using Akka.Event;
using DiceDistributedGame.Actors.Commands.GameCommands;
using DiceDistributedGame.Actors.Commands.PlayBoardCommand;
using DiceDistributedGame.Actors.Commands.PlayerCommand;
using DiceDistributedGame.Actors.Events.PlayBoardEvent;
using System;
using System.Collections.Generic;

namespace DiceDistributedGame.Actors.Actors
{
    public class PlayBoardActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        private PlayBoardEvent Event;
        private readonly Dictionary<string, IActorRef> _game;
        public PlayBoardActor()
        {
            Event = new PlayBoardEvent();
            StartingReceivers();
            _game = new Dictionary<string, IActorRef>();
        }

        private PlayBoardActor(PlayBoardEvent Event)
        {
            Event = new PlayBoardEvent();
            StartingReceivers();
        }

        private void StartingReceivers()
        {            
            Receive<ShowOpenGames>(message => ShowOpenGamesMethod(message));
            Receive<CreateNewGame>(message => CreateNewGameMethod(message));
            Receive<GameRegister>(message => GameRegisterChangesMethod(message));
            Receive<EnterExistingGame>(message => EnterExistingGameMethod(message));
            Receive<EnteringNewUserError>(message => EnteringNewUserErrorMethod(message));
            Receive<ThrowDice>(message => ThrowDiceMethod(message));
            Receive<FinishGame>(message => FinishGameMethod(message));
            
        }
        private void ShowOpenGamesMethod(ShowOpenGames message)
        {
            foreach(var key in Event.GameRegistered.Keys)
            {
                var obj = Event.GameRegistered[key];
                if (!obj.GameEventDashboard.IsGameFinished &&
                    !obj.GameEventDashboard.IsGameStarted)
                {
                    message.OpenGames.Add(new Model.Games.GameInfoForReport() {
                        GameId = obj.GameEventDashboard.GameId,
                        PlayerName = obj.GameEventDashboard.PlayerInfo.Name });
                }
            }
            Sender.Tell( message);
        }
        private void FinishGameMethod(FinishGame message)
        {
            var actor = GetActorRefForGame(message.GameId);
            actor.Tell((message));
        }

        private void ThrowDiceMethod(ThrowDice message)
        {
            var actor = GetActorRefForGame(message.GameId);
            actor.Tell(message);
        }

        private void EnteringNewUserErrorMethod(EnteringNewUserError message)
        {
            //var actor = GetActorRefForGame(message.GameId);
            //actor.Tell(new AddPlayerToGame(message));
        }

        private void EnterExistingGameMethod(EnterExistingGame message)
        {
            var actor = GetActorRefForGame(message.GameId);
            actor.Tell(new AddPlayerToGame(message));
        }
        private IActorRef GetActorRefForGame(string gameId)
        {
            IActorRef actor = null;
            if (_game.ContainsKey(gameId))
            {
                actor = _game[gameId]; ;
            }
            else
            {
                actor = Context.ActorOf(Akka.Actor.Props.Create<GameActor>(), gameId);
                _game.Add(gameId, actor);
            }
            return actor;
        }
        private void GameRegisterChangesMethod(GameRegister message)
        {
            if (!Event.GameRegistered.ContainsKey(message.GameEventDashboard.GameId))
            {
                Event.GameRegistered[message.GameEventDashboard.GameId] = message;
            }
            else
            {
                Event.GameRegistered.Add(message.GameEventDashboard.GameId, message);
            }
        }

        private void CreateNewGameMethod(CreateNewGame message)
        {
            IActorRef actor = null;
            if (_game.ContainsKey(message.Id))
            {
                actor = _game[message.Id]; ;
            }
            else
            {
                actor = Context.ActorOf(Akka.Actor.Props.Create<GameActor>(),"Game"+ message.Id);
                _game.Add(message.Id, actor);
            }
            actor.Tell(new StartNewGame(message));
            Console.WriteLine("PlayBoardActor CreateNewGameMethod: Created With ID {0} With Ower {1}", message.Id, message.PlayerInfo.Name);
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

        public static Props Props() =>
          Akka.Actor.Props.Create(() => new PlayBoardActor());

        public static Props Props( PlayBoardEvent eve) =>
                  Akka.Actor.Props.Create(() =>
                          new PlayBoardActor(eve));



        #region Lifecycle hooks

        protected override void PreStart()
        {
            _logger.Debug("PlayBoardActor PreStart");
        }

        protected override void PostStop()
        {
            _logger.Debug("PlayBoardActor PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            _logger.Debug("PlayBoardActor PreRestart because {0}", reason);

            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            _logger.Debug("PlayBoardActor PostRestart because {0}", reason);

            base.PostRestart(reason);
        }
        #endregion

    }
}
