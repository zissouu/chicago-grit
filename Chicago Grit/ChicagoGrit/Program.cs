using System;

namespace ChicagoGrit
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "CHICAGO GRIT: Street Survival";

            Player player = new Player();
            GangManager gangManager = new GangManager();
            MissionManager missionManager = new MissionManager(player, gangManager);

            Console.WriteLine("=====================================");
            Console.WriteLine("   CHICAGO GRIT: STREET SURVIVAL");
            Console.WriteLine("=====================================");
            Console.WriteLine("Fresh on the South Side. No money. No crew. No respect.\n");

            bool isRunning = true;

            while (isRunning)
            {
                Console.WriteLine("\nWhat do you want to do?");
                Console.WriteLine("[1] Hit the streets (check missions)");
                Console.WriteLine("[2] Check your stats");
                Console.WriteLine("[3] Check gang relations");
                Console.WriteLine("[0] Exit game");
                Console.Write("> ");

                string choice = Console.ReadLine()?.Trim().ToLower() ?? "";

                switch (choice)
                {
                    case "1":
                        missionManager.CheckUnlocks();
                        missionManager.ShowMissions();
                        break;
                    case "2":
                        player.ShowStatus();
                        break;
                    case "3":
                        gangManager.ShowAll();
                        break;
                    case "0":
                        Console.WriteLine("You fade into the shadows. Game over.");
                        isRunning = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }
    }
}
