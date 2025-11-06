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

        public void Start()
        {
            Console.Title = "CHICAGO GRIT: Street Survival";
            ShowIntro();
            player = new Player();
            
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
            Console.WriteLine("Make moves or get swallowed up by the streets.\n");
        }

        void ShowMainMenu()
        {
            Console.WriteLine("\nWhat do you want to do?");
            Console.WriteLine("[1] Hit the streets");
            Console.WriteLine("[2] Check your status");
            Console.WriteLine("[3] Rest for the night");
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
                    Console.WriteLine("You find a quiet corner and rest. The city never sleeps, but you do.");
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
            Random rand = new Random();
            int eventRoll = rand.Next(1, 4);

            switch (eventRoll)
            {
                case 1:
                    Console.WriteLine("A rival crew spots you tagging their wall. A fight breaks out!");
                    player.LoseHealth(10);
                    break;
                case 2:
                    Console.WriteLine("You find a dropped wallet — $40 cash. Luck’s on your side.");
                    player.AddMoney(40);
                    break;
                case 3:
                    Console.WriteLine("A cop car slows down nearby. You duck into an alley and lay low.");
                    break;
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
            Console.WriteLine($"You gained ${amount}. Total money: ${Money}");
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
    }
}
