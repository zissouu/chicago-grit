using System;

namespace ChicagoGrit
{
    public class Player
    {
        public int Money { get; private set; } = 0;
        public int Reputation { get; private set; } = 0;

        public void AddMoney(int amount)
        {
            Money += amount;
            Console.WriteLine($"[Money Update] You now have ${Money}");
        }

        public void AddReputation(int amount)
        {
            Reputation += amount;
            Console.WriteLine($"[Reputation Update] Your reputation is now {Reputation}");
        }
    }
}
