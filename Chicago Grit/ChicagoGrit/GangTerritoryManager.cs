using System;
using System.Collections.Generic;

namespace ChicagoGrit
{
    public class GangTerritoryManager
    {
        private Dictionary<string, string> territoryOwners = new()
        {
            {"South Side", "South Side Kings"},
            {"Iron District", "Iron Vultures"},
            {"West Side", "West Side Reapers"},
            {"North End", "North End Ghosts"}
        };

        public void ShowTerritories()
        {
            Console.WriteLine("\n--- Gang Territories ---");
            foreach(var kvp in territoryOwners)
            {
                Console.WriteLine($"{kvp.Key}: Controlled by {kvp.Value}");
            }
        }

        public string GetGangControlling(string area)
        {
            return territoryOwners.ContainsKey(area) ? territoryOwners[area] : "None";
        }

        public void ChangeControl(string area, string newGang)
        {
            if (territoryOwners.ContainsKey(area))
            {
                Console.WriteLine($"[Territory Change] {area} is now controlled by {newGang}");
                territoryOwners[area] = newGang;
            }
        }

        public void RandomTerritoryShift(Random rng, GangManager gangManager)
        {
            foreach(var area in new List<string>(territoryOwners.Keys))
            {
                if(rng.NextDouble() < 0.2)
                {
                    var gangs = new[] {"South Side Kings","Iron Vultures","West Side Reapers","North End Ghosts"};
                    string newGang = gangs[rng.Next(gangs.Length)];
                    ChangeControl(area,newGang);
                    gangManager.AdjustRelation(newGang,5);
                }
            }
        }
    }
}
