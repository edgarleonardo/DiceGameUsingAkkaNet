using Akka.Actor;
using DiceDistributedGame.Actors.ExternalSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiceDistributedGameApplication.ActorSystemFolder
{
    public static class DiceGameActorSystem
    {
        private static ActorSystem ActorSystemObject;
        private static IGameEventsPusher _gameEventsPusher;

        public static void Create()
        {
            _gameEventsPusher = new SignalRGameEventPusher();

            ActorSystemObject = Akka.Actor.ActorSystem.Create("GameSystem");

            ActorReferences.GameController =
                ActorSystemObject.ActorSelection("akka.tcp://GameSystem@127.0.0.1:8091/user/GameController")
                    .ResolveOne(TimeSpan.FromSeconds(3))
                    .Result;

            ActorReferences.SignalRBridge = ActorSystem.ActorOf(
                Props.Create(() => new SignalRBridgeActor(_gameEventsPusher, ActorReferences.GameController)),
                "SignalRBridge"
                );
        }

        public static void Shutdown()
        {
            ActorSystemObject.Shutdown();

            ActorSystemObject.AwaitTermination(TimeSpan.FromSeconds(1));
        }


        public static class ActorReferences
        {
            public static IActorRef GameController { get; set; }
            public static IActorRef SignalRBridge { get; set; }
        }
    }
}
