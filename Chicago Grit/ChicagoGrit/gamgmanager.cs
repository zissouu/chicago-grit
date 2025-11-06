using System;
using System.Collections.Generic;

namespace ChicagoGrit
{
    class GangManager
    {
        private Dictionary<string, int> relations = new()
        {
            { "South Side Kings", 0 },
            { "Iron Vultures", 0 }
        };

        public int GetRelation(string gangName)
        {
            return relations.ContainsKey(gangName) ? relations[gangName] : 0;
        }

        public void AdjustRelation(string gangName, int amount)
        {
            if (relations.ContainsKey(gangName))
            {
                relations[gangName] = Math.Clamp(relations[gangName] + amount, -100, 100);
                Console.WriteLine($"{gangName} relation: {relations[gangName]}");
            }
        }

        public void ShowAll()
        {
            Console.WriteLine("\nGang Relations:");
            foreach (var kv in relations)
            {
                Console.WriteLine($"{kv.Key}: {kv.Value}");
            }
        }
    }
}
