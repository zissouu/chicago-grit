using System;
using System.Collections.Generic;

namespace ChicagoGrit
{
    public class GangManager
    {
        private Dictionary<string, int> relations = new()
        {
            {"South Side Kings", 0},
            {"Iron Vultures", 0},
            {"West Side Reapers", 0},
            {"North End Ghosts", 0}
        };

        public void AdjustRelation(string gang, int amount)
        {
            if (relations.ContainsKey(gang))
            {
                relations[gang] += amount;
                Console.WriteLine($"[Gang Update] {gang} relation is now {relations[gang]}");
            }
        }

        public int GetRelation(string gang)
        {
            return relations.ContainsKey(gang) ? relations[gang] : 0;
        }

        public void ShowRelations()
        {
            Console.WriteLine("\n--- Gang Relations ---");
            foreach (var kvp in relations)
            {
                string status = kvp.Value >= 20 ? "Friendly" : kvp.Value <= -10 ? "Hostile" : "Neutral";
                Console.WriteLine($"{kvp.Key}: {kvp.Value} ({status})");
            }
        }
    }
}
