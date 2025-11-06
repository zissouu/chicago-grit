using System;

namespace ChicagoGrit
{
    class Player
    {
        public int Health { get; private set; } = 100;
        public int Money { get; private set; } = 50;
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
            Console.WriteLine($"Money: ${Money}");
        }

        public void AddReputation(int amount)
        {
            Reputation += amount;
            Console.WriteLine($"Reputation: {Reputation}");
        }

        public void LoseHealth(int amount)
        {
            Health -= amount;
            Console.WriteLine($"You lost {amount} health ({Health} remaining).");
            if (Health <= 0)
            {
                Console.WriteLine("You collapse... The streets win.");
                Environment.Exit(0);
            }
        }
    }
}
