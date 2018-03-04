using Akka.TestKit.Xunit2;
using DiceDistributedGame.Actors.Actors;
using DiceDistributedGame.Actors.Commands.PlayBoardCommand;
using DiceDistributedGame.Actors.Commands.PlayerCommand;
using DiceDistributedGame.Actors.Events.GameEvent;
using DiceDistributedGame.Actors.Events.PlayBoardEvent;
using DiceDistributedGame.Model.Player;
using Moq;
using System;
using Xunit;

namespace DiceDistributedGameApplication.Test
{
    public class PlayBoardShould : TestKit
    {
        [Fact]
        public void UserCreateNewGame()
        {
            
            var probe = CreateTestProbe();
            var playboard = Sys.ActorOf(PlayBoardActor.Props();
            var game = new CreateNewGame(new Player( "Edgar Leonardo", ""));
            playboard.Tell(game, probe.Ref);

            var received = probe.ExpectMsg<GameRegister>();
            Assert.Equal(game.Id, received.GameEventDashboard.GameId);
        }
     
       
        [Fact]
        public void UserWithCreatedNewGameMustBeOwnerOfGame()
        {
           
            var probe = CreateTestProbe();
            var connectionId = Guid.NewGuid().ToString();
            var ownerGame = new Player( "Edgar Leonardo", connectionId);
            var game = new CreateNewGame(ownerGame);
             var playboard = Sys.ActorOf(PlayBoardActor.Props());

            playboard.Tell(game, probe.Ref);

            var received = probe.ExpectMsg<GameRegister>();
            Assert.Equal(game.Id, received.GameEventDashboard.GameId);

            Assert.Equal(ownerGame.Id, received.GameEventDashboard.PlayerInfo.Id);
        }
        [Fact]
        public void UserEnterNotStartedGame()
        {
            var probe = CreateTestProbe();
            var gamesNotStarted = new PlayBoardEvent();
            var gameKey = Guid.NewGuid().ToString();
            var connectionId = Guid.NewGuid().ToString();
            var owner = new Player("Pedro Perez", connectionId);

            var playerRegistering = new Player( "Edgar Leonardo", Guid.NewGuid().ToString());
            var gameEvent = new GameEvent(gameKey, owner);
            var gameRegister = new GameRegister(gameEvent);
            gamesNotStarted.GameRegistered.Add(gameKey, gameRegister);
            var playboard = Sys.ActorOf(PlayBoardActor.Props( gamesNotStarted));

            var message = new EnterExistingGame(playerRegistering, gameKey);
            playboard.Tell(message, probe.Ref);

            var received = probe.ExpectMsg<UserRegistrationDone>();
            Assert.Equal(gameKey, received.GameId);

            Assert.Equal(playerRegistering.Id, received.PlayerInfo.Id);
        }
        [Fact]
        public void UserNotEntertartedGame()
        {
            var probe = CreateTestProbe();
            var gamesNotStarted = new PlayBoardEvent();
            var gameKey = Guid.NewGuid().ToString();
            var owner = new Player( "Pedro Perez", Guid.NewGuid().ToString());

            var playerRegistering = new Player( "Edgar Leonardo", Guid.NewGuid().ToString());
            var playerRegistering2 = new Player( "Edgar Antonio", Guid.NewGuid().ToString());

            var playerRegistering3 = new Player( "Edgar Miguel", Guid.NewGuid().ToString());
            var gameEvent = new GameEvent(gameKey, owner);
            
            var gameRegister = new GameRegister(gameEvent);
            gamesNotStarted.GameRegistered.Add(gameKey, gameRegister);
            var playboard = Sys.ActorOf(PlayBoardActor.Props( gamesNotStarted));

            var message = new EnterExistingGame(playerRegistering, gameKey);
            playboard.Tell(message, probe.Ref);
            var received = probe.ExpectMsg<UserRegistrationDone>();
            Assert.Equal(gameKey, received.GameId);
            Assert.Equal(playerRegistering.Id, received.PlayerInfo.Id);
            message = new EnterExistingGame(playerRegistering2, gameKey);
            playboard.Tell(message, probe.Ref);
            received = probe.ExpectMsg<UserRegistrationDone>();
            Assert.Equal(gameKey, received.GameId);
            Assert.Equal(playerRegistering2.Id, received.PlayerInfo.Id);

            /// Initializing the game the first player that create the game
            var startGame = new ThrowDice(playerRegistering, gameKey);
            playboard.Tell(startGame, probe.Ref);
            var creceived3 = probe.ExpectMsg<GameRegister>();
            /// The Game must be started, if game is started
            Assert.True(creceived3.GameEventDashboard.IsGameStarted);

            message = new EnterExistingGame(playerRegistering3, gameKey);
            playboard.Tell(message, probe.Ref);
            var receivedIsStarted = probe.ExpectMsg<EnteringNewUserError>();
            Assert.True(receivedIsStarted.GameAlreadyStarted);
        }
        [Fact]
        public void UserNotEnterFinishedGame()
        {
            var probe = CreateTestProbe();
            var gamesNotStarted = new PlayBoardEvent();
            var gameKey = Guid.NewGuid().ToString();
            var owner = new Player( "Pedro Perez", Guid.NewGuid().ToString());

            var playerRegistering = new Player( "Edgar Leonardo", Guid.NewGuid().ToString());
            var playerRegistering2 = new Player( "Edgar Antonio", Guid.NewGuid().ToString());

            var playerRegistering3 = new Player( "Edgar Miguel", Guid.NewGuid().ToString());
            var gameEvent = new GameEvent(gameKey, owner);

            var gameRegister = new GameRegister(gameEvent);
            gamesNotStarted.GameRegistered.Add(gameKey, gameRegister);
            var playboard = Sys.ActorOf(PlayBoardActor.Props( gamesNotStarted));

            var message = new EnterExistingGame(playerRegistering, gameKey);
            playboard.Tell(message, probe.Ref);
            var received = probe.ExpectMsg<UserRegistrationDone>();
            Assert.Equal(gameKey, received.GameId);
            Assert.Equal(playerRegistering.Id, received.PlayerInfo.Id);
            message = new EnterExistingGame(playerRegistering2, gameKey);
            playboard.Tell(message, probe.Ref);
            received = probe.ExpectMsg<UserRegistrationDone>();
            Assert.Equal(gameKey, received.GameId);
            Assert.Equal(playerRegistering2.Id, received.PlayerInfo.Id);

            /// Initializing the game the first player that create the game
            var startGame = new ThrowDice(playerRegistering, gameKey);
            playboard.Tell(startGame, probe.Ref);
            var creceived3 = probe.ExpectMsg<GameRegister>();
            /// The Game must be started, if game is started
            Assert.True(creceived3.GameEventDashboard.IsGameStarted);

            message = new EnterExistingGame(playerRegistering3, gameKey);
            playboard.Tell(message, probe.Ref);
            var receivedIsStarted = probe.ExpectMsg<EnteringNewUserError>();
            Assert.True(receivedIsStarted.GameAlreadyStarted);
        }
        [Fact]
        public void UserMustExistsBeforeFinishGame()
        {
            
            var probe = CreateTestProbe();
            var gamesNotStarted = new PlayBoardEvent();
            var gameKey = Guid.NewGuid().ToString();
            var owner = new Player( "Pedro Perez", Guid.NewGuid().ToString());

            var playerRegistering = new Player( "Edgar Leonardo", Guid.NewGuid().ToString());
            var playerRegistering2 = new Player( "Edgar Antonio", Guid.NewGuid().ToString());

            var playerRegistering3 = new Player( "Edgar Miguel", Guid.NewGuid().ToString());
            var gameEvent = new GameEvent(gameKey, owner);

            var gameRegister = new GameRegister(gameEvent);
            gamesNotStarted.GameRegistered.Add(gameKey, gameRegister);
            var playboard = Sys.ActorOf(PlayBoardActor.Props( gamesNotStarted));

            var message = new EnterExistingGame(playerRegistering, gameKey);
            playboard.Tell(message, probe.Ref);
            var received = probe.ExpectMsg<UserRegistrationDone>();
            Assert.Equal(gameKey, received.GameId);
            Assert.Equal(playerRegistering.Id, received.PlayerInfo.Id);
            message = new EnterExistingGame(playerRegistering2, gameKey);
            playboard.Tell(message, probe.Ref);
            received = probe.ExpectMsg<UserRegistrationDone>();
            Assert.Equal(gameKey, received.GameId);
            Assert.Equal(playerRegistering2.Id, received.PlayerInfo.Id);

            /// Initializing the game the first player that create the game
            var startGame = new ThrowDice(playerRegistering, gameKey);
            playboard.Tell(startGame, probe.Ref);
            var creceived3 = probe.ExpectMsg<GameRegister>();
            /// The Game must be started, if game is started
            Assert.True(creceived3.GameEventDashboard.IsGameStarted);

            var messageFinishGame = new FinishGame(playerRegistering3, gameKey);
            playboard.Tell(messageFinishGame, probe.Ref);
            var receivedIsStarted = probe.ExpectMsg<EnteringNewUserError>();
            Assert.True(receivedIsStarted.PlayerDoentExistsRegistered);

        }
        [Fact]
        public void UserCouldFinishGame()
        {
            
            var probe = CreateTestProbe();
            var gamesNotStarted = new PlayBoardEvent();
            var gameKey = Guid.NewGuid().ToString();
            var owner = new Player( "Pedro Perez", Guid.NewGuid().ToString());

            var playerRegistering = new Player( "Edgar Leonardo", Guid.NewGuid().ToString());
            var playerRegistering2 = new Player( "Edgar Antonio", Guid.NewGuid().ToString());

            var playerRegistering3 = new Player( "Edgar Miguel", Guid.NewGuid().ToString());
            var gameEvent = new GameEvent(gameKey, owner);

            var gameRegister = new GameRegister(gameEvent);
            gamesNotStarted.GameRegistered.Add(gameKey, gameRegister);
            var playboard = Sys.ActorOf(PlayBoardActor.Props(gamesNotStarted));

            var message = new EnterExistingGame(playerRegistering, gameKey);
            playboard.Tell(message, probe.Ref);
            var received = probe.ExpectMsg<UserRegistrationDone>();
            Assert.Equal(gameKey, received.GameId);
            Assert.Equal(playerRegistering.Id, received.PlayerInfo.Id);
            message = new EnterExistingGame(playerRegistering2, gameKey);
            playboard.Tell(message, probe.Ref);
            received = probe.ExpectMsg<UserRegistrationDone>();
            Assert.Equal(gameKey, received.GameId);
            Assert.Equal(playerRegistering2.Id, received.PlayerInfo.Id);

            /// Initializing the game the first player that create the game
            var startGame = new ThrowDice(playerRegistering, gameKey);
            playboard.Tell(startGame, probe.Ref);
            var creceived3 = probe.ExpectMsg<GameRegister>();
            /// The Game must be started, if game is started
            Assert.True(creceived3.GameEventDashboard.IsGameStarted);

            var messageFinishGame = new FinishGame(playerRegistering2, gameKey);
            playboard.Tell(messageFinishGame, probe.Ref);
            var receivedIsStarted = probe.ExpectMsg<GameRegister>();
            Assert.Equal(receivedIsStarted.GameEventDashboard.GameId, gameKey);

        }

    }
}
