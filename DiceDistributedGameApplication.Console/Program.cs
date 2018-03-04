using Akka.Actor;
using Akka.Configuration;
using Akka.Routing;
using DiceDistributedGame.Actors.Actors;
using DiceDistributedGame.Actors.Commands.PlayBoardCommand;
using DiceDistributedGame.Actors.Commands.PlayerCommand;
using DiceDistributedGame.Model.Games;
using DiceDistributedGame.Model.Player;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading;

namespace DiceDistributedGameApplication.Console
{
    class Program
    {
        private static ActorSystem Systems { get; set; }
        private static IActorRef PlayerCoordinator { get; set; }
        static void Main(string[] args)
        {
            var logger = new LoggerConfiguration().WriteTo.Console()
    .CreateLogger();

            Serilog.Log.Logger = logger;
            var config = ConfigurationFactory.ParseString(@"
 loglevel = DEBUG
}");
            logger.Information("Starting Actor System");
            Systems = ActorSystem.Create("diceDistributedGame", config);
            PlayerCoordinator = Systems.ActorOf( Props.Create(() => new PlayBoardActor()));
            
            DisplayPlayBoardInstructions();


            while (true)
            {
                var action = System.Console.ReadLine();
                
                if (action.Contains("1"))
                {
                    System.Console.Write("Player Name: ");
                    var playerName =System.Console.ReadLine();
                    CreateNewGame(playerName);
                }
                else if (action.Contains("2"))
                {
                    SelectExistingsGames();
                }
                else if (action.Contains("3"))
                {
                    //DisplayPlayer(playerName);
                }
                else if (action.Contains("error"))
                {
                    //ErrorPlayer(playerName);
                }
                else
                {
                    System.Console.WriteLine("Unknown command");
                }
            }
            
        }

        private static void ErrorPlayer(string playerName)
        {
            //System.ActorSelection($"/user/PlayerCoordinator/{playerName}")
            //      .Tell(new SimulateError());
        }

        private static void ShowWinner(string gameId)
        {
            
            //System.ActorSelection($"/user/PlayerCoordinator/{playerName}")
            //      .Tell(new DisplayStatus());
        }

        private static void SelectExistingsGames()
        {
            var messageShowGames = ShowGames();
            if (messageShowGames != null && messageShowGames.OpenGames != null &&
                messageShowGames.OpenGames.Count > 0)
            {
                System.Console.Clear();
                DisplayPlayBoardInstructionsToChooseAGame(messageShowGames.OpenGames);
                while (true)
                {
                    try
                    {
                       var action = System.Console.ReadLine();
                        if (action != "E")
                        {
                            var index = int.Parse(action);
                            var playerName = messageShowGames.OpenGames[index];
                            System.Console.Write("Type Player Your Name: ");
                            var gameName = System.Console.ReadLine();
                            var enterExist = new EnterExistingGame(new Player(gameName, ""), messageShowGames.OpenGames[index].GameId);
                            break;
                        }
                        else
                        if (action.Contains("E"))
                        {
                            break;
                        }
                    }
                    catch
                    {

                    }
                }
            }
        }
        private static ShowOpenGames ShowGames()
        {
            return PlayerCoordinator.Ask<ShowOpenGames>(new ShowOpenGames()).Result;
        }
        private static void CreateNewGame(string player)
        {
            var playerObject = new Player(player, "");
            PlayerCoordinator.Tell(new CreateNewGame(playerObject));
        }
        private static void DisplayPlayBoardInstructionsToChooseAGame(List<GameInfoForReport> OpenGames)
        {
            System.Console.ForegroundColor = ConsoleColor.White;

            System.Console.WriteLine("Available commands:");
            var counter = 1;
            foreach(var data in OpenGames)
            {
                System.Console.WriteLine($"{counter} - GameId: {data.GameId} Game Ower: {data.PlayerName}");
            }
            System.Console.WriteLine("E - exit");
        }
        private static void DisplayPlayBoardInstructions()
        {
            System.Console.ForegroundColor = ConsoleColor.White;

            System.Console.WriteLine("Available commands:");
            System.Console.WriteLine("1 - create new game");
            System.Console.WriteLine("2 - enter existing games");
            System.Console.WriteLine("3 - show winners");
        }
        private static void DisplayGameInstructions()
        {
            System.Console.ForegroundColor = ConsoleColor.White;

            System.Console.WriteLine("Available commands:");
            System.Console.WriteLine("1 - throw dice");
            System.Console.WriteLine("2 - leave game");
            System.Console.WriteLine("3 - player status");
        }
        private static void PlayingRoom(string gameId, Player player)
        {
            DisplayGameInstructions();
            while (true)
            {
                var action = System.Console.ReadLine();

                if (action.Contains("1"))
                {
                    var startGame = new ThrowDice(player, gameId);
                    PlayerCoordinator.Tell(startGame);
                }
                else if (action.Contains("2"))
                {
                    SelectExistingsGames();
                }
                else if (action.Contains("3"))
                {
                    //DisplayPlayer(playerName);
                }
            }
        }
    }
}
