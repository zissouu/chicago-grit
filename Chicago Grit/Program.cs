using System;
using System.Collections.Generic;

namespace ChicagoGrit
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();
        }
    }

    class Game
    {
        private bool isRunning = true;
        private Player player;
        private GangManager gangManager;
        private Random random = new Random();

        public void Start()
        {
            Console.Title = "CHICAGO GRIT: STREET SURVIVAL";
            ShowIntro();
            player = new Player();
            gangManager = new GangManager();

            while (isRunning)
            {
                ShowMainMenu();
                HandleInput(Console.ReadLine()?.Trim().ToLower());
            }
        }

        void ShowIntro()
        {
            Console.WriteLine("=====================================");
            Console.WriteLine("   CHICAGO GRIT: STREET SURVIVAL");
            Console.WriteLine("=====================================");
            Console.WriteLine("You’re fresh on the South Side. No money. No crew. No respect.");
            Console.WriteLine("The city waits to see what you’ll become.\n");
        }

        void ShowMainMenu()
        {
            Console.WriteLine("\nWhat do you want to do?");
            Console.WriteLine("[1] Hit the streets");
            Console.WriteLine("[2] Check your status");
            Console.WriteLine("[3] Check gang standings");
            Console.WriteLine("[0] Exit game");
            Console.Write("> ");
        }

        void HandleInput(string choice)
        {
            switch (choice)
            {
                case "1":
                    StreetAction();
                    break;
                case "2":
                    player.ShowStatus();
                    break;
                case "3":
                    gangManager.ShowAll();
                    break;
                case "0":
                    Console.WriteLine("You walk away from the life... for now.");
                    isRunning = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        void StreetAction()
        {
            int eventRoll = random.Next(1, 4);

            switch (eventRoll)
            {
                case 1:
                    RivalEncounter();
                    break;
                case 2:
                    GangRecruitment();
                    break;
                case 3:
                    HustleOpportunity();
                    break;
            }
        }

        void RivalEncounter()
        {
            Console.WriteLine("A couple of Iron Vultures corner you in an alley. They want tribute.");
            Console.WriteLine("[1] Pay $20 and walk away");
            Console.WriteLine("[2] Fight back");
            Console.Write("> ");
            string input = Console.ReadLine();

            if (input == "1")
            {
                if (player.Money >= 20)
                {
                    player.AddMoney(-20);
                    Console.WriteLine("You hand them the cash. They sneer but let you go.");
                    gangManager.AdjustRelation("Iron Vultures", 5);
                }
                else
                {
                    Console.WriteLine("You don’t have the money — they rough you up instead.");
                    player.LoseHealth(15);
                    gangManager.AdjustRelation("Iron Vultures", -10);
                }
            }
            else if (input == "2")
            {
                Console.WriteLine("You swing first — it’s chaos.");
                if (random.Next(0, 2) == 0)
                {
                    Console.WriteLine("You get away bruised but alive.");
                    player.LoseHealth(10);
                    gangManager.AdjustRelation("Iron Vultures", -20);
                }
                else
                {
                    Console.WriteLine("You knock one out cold — word gets around fast.");
                    player.AddReputation(10);
                    gangManager.AdjustRelation("Iron Vultures", -30);
                }
            }
        }

        void GangRecruitment()
        {
            Console.WriteLine("A South Side King scout spots your hustle.");
            Console.WriteLine("[1] Hear him out");
            Console.WriteLine("[2] Brush him off");
            Console.Write("> ");
            string input = Console.ReadLine();

            if (input == "1")
            {
                Console.WriteLine("He nods. 'We got eyes on you. Keep it real, we might have work soon.'");
                gangManager.AdjustRelation("South Side Kings", 15);
            }
            else
            {
                Console.WriteLine("He shrugs and walks off. Opportunity lost.");
                gangManager.AdjustRelation("South Side Kings", -5);
            }
        }

        void HustleOpportunity()
        {
            Console.WriteLine("You find a small hustle — flipping cheap phones.");
            Console.WriteLine("[1] Take the risk");
            Console.WriteLine("[2] Pass — seems sketchy");
            Console.Write("> ");
            string input = Console.ReadLine();

            if (input == "1")
            {
                if (random.Next(0, 2) == 0)
                {
                    Console.WriteLine("The deal pays off — $60 profit.");
                    player.AddMoney(60);
                    player.AddReputation(5);
                }
                else
                {
                    Console.WriteLine("Cops roll up — you lose the goods and get a fine.");
                    player.AddMoney(-30);
                    player.LoseHealth(5);
                    player.AddReputation(-5);
                }
            }
            else
            {
                Console.WriteLine("You walk away. Sometimes staying low is the smart move.");
            }
        }
    }

    class Player
    {
        public int Health { get; private set; } = 100;
        public int Money { get; private set; } = 20;
        public int Reputation { get; private set; } = 0;

        public void ShowStatus()
        {
            Console.WriteLine($"\nHealth: {Health}");
            Console.WriteLine($"Money: ${Money}");
            Console.WriteLine($"Reputation: {Reputation}");
        }

        public void AddMoney(int amount)
        {
            Money += amount;
            Console.WriteLine(amount > 0 ? $"You gained ${amount}." : $"You lost ${Math.Abs(amount)}.");
        }

        public void LoseHealth(int amount)
        {
            Health -= amount;
            Console.WriteLine($"You lost {amount} health. Current health: {Health}");
            if (Health <= 0)
            {
                Console.WriteLine("You’ve been taken out. The streets claim another soul.");
                Environment.Exit(0);
            }
        }

        public void AddReputation(int amount)
        {
            Reputation += amount;
            Console.WriteLine($"Your reputation changed by {amount}. Now at {Reputation}.");
        }
    }

    class Gang
    {
        public string Name { get; }
        public int Relation { get; private set; }

        public Gang(string name, int startingRelation)
        {
            Name = name;
            Relation = startingRelation;
        }

        public void AdjustRelation(int amount)
        {
            Relation = Math.Clamp(Relation + amount, -100, 100);
        }
    }

    class GangManager
    {
        private Dictionary<string, Gang> gangs = new Dictionary<string, Gang>()
        {
            { "South Side Kings", new Gang("South Side Kings", 0) },
            { "Iron Vultures", new Gang("Iron Vultures", 0) },
            { "Lakeside Crew", new Gang("Lakeside Crew", 0) }
        };

        public void AdjustRelation(string gangName, int amount)
        {
            if (gangs.ContainsKey(gangName))
            {
                gangs[gangName].AdjustRelation(amount);
                Console.WriteLine($"Your relationship with {gangName} changed by {amount}.");
            }
        }

        public void ShowAll()
        {
            Console.WriteLine("\nGang Standings:");
            foreach (var gang in gangs.Values)
            {
                Console.WriteLine($"{gang.Name}: {gang.Relation}");
            }
        }
    }
}
