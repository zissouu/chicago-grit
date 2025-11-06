using System;

namespace ChicagoGrit
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=====================================");
            Console.WriteLine("   CHICAGO GRIT: STREET SURVIVAL");
            Console.WriteLine("=====================================\n");

            Player player = new Player();
            GangManager gangManager = new GangManager();
            GangTerritoryManager territoryManager = new GangTerritoryManager();
            MissionManager missionManager = new MissionManager(player, gangManager);
            GameTime gameTime = new GameTime();

            // Create a multi-day story arc
            var kingsArc = new StoryArc(
                "kings_arc",
                "Rise with the Kings",
                new List<Mission>
                {
                    missionManager.CreateKingsIntro()
                    // Add additional missions for follow-ups, heists, etc.
                });
            missionManager.AddStoryArc(kingsArc);

            bool running = true;
            while(running)
            {
                Console.WriteLine($"\n[Day {gameTime.Day}, {gameTime.CurrentTime()}]");
                Console.WriteLine("What do you want to do?");
                Console.WriteLine("[1] Hit the streets");
                Console.WriteLine("[2] Check stats");
                Console.WriteLine("[3] Check gang relations");
                Console.WriteLine("[4] Check gang territories");
                Console.WriteLine("[0] Exit game");
                Console.Write("> ");

                string input = Console.ReadLine()?.Trim();
                Random rng = new();

                switch(input)
                {
                    case "1":
                        missionManager.CheckStoryArcs();
                        missionManager.ShowMissions();
                        gameTime.AdvanceHours(rng.Next(2,4));
                        break;
                    case "2":
                        Console.WriteLine($"\nMoney: ${player.Money}, Reputation: {player.Reputation}");
                        break;
                    case "3":
                        gangManager.ShowRelations();
                        break;
                    case "4":
                        territoryManager.ShowTerritories();
                        break;
                    case "0":
                        running=false;
                        Console.WriteLine("Exiting game. Stay alive!");
                        break;
                    default:
                        Console.WriteLine("Invalid input.");
                        break;
                }

                if(gameTime.Hour>=22)
                    EvolveGangWars(gangManager, missionManager, territoryManager);
            }
        }

        static void EvolveGangWars(GangManager gangManager, MissionManager missionManager, GangTerritoryManager territoryManager)
        {
            Console.WriteLine("\n--- Night falls. Gangs are active. ---");
            Random rng = new();
            foreach(var gang in new[] {"South Side Kings","Iron Vultures","West Side Reapers","North End Ghosts"})
            {
                int change = rng.Next(-5,6);
                if(gangManager.GetRelation(gang)<0){ change -= rng.Next(0,5); Console.WriteLine($"[Gang Alert] {gang} becomes aggressive!");}
                gangManager.AdjustRelation(gang,change);
            }
            if(rng.NextDouble()<0.5) missionManager.TriggerRandomEvent();
            territoryManager.RandomTerritoryShift(rng,gangManager);
        }
    }
}
