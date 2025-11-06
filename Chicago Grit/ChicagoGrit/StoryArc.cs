using System;
using System.Collections.Generic;

namespace ChicagoGrit
{
    public class StoryArc
    {
        public string Id { get; }
        public string Name { get; }
        public List<Mission> Missions { get; }
        public int CurrentIndex { get; private set; } = 0;
        public bool IsCompleted => CurrentIndex >= Missions.Count;

        public StoryArc(string id, string name, List<Mission> missions)
        {
            Id = id;
            Name = name;
            Missions = missions;
        }

        public Mission GetCurrentMission()
        {
            return CurrentIndex < Missions.Count ? Missions[CurrentIndex] : null;
        }

        public void AdvanceMission()
        {
            if(CurrentIndex < Missions.Count) CurrentIndex++;
        }
    }
}
