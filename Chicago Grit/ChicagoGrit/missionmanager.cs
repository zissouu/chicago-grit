using System;
using System.Collections.Generic;

namespace ChicagoGrit
{
    public class MissionManager
    {
        private Player player;
        private GangManager gangManager;
        private List<Mission> missions = new();
        private List<StoryArc> storyArcs = new();
        private List<StreetEvent> streetEvents;
        private Random rng = new();

        public MissionManager(Player player, GangManager gangManager)
        {
            this.player = player;
            this.gangManager = gangManager;
            InitializeStreetEvents();
        }

        public void AddStoryArc(StoryArc arc)
        {
            storyArcs.Add(arc);
        }

        public void CheckStoryArcs()
        {
            foreach(var arc in storyArcs)
            {
                var mission = arc.GetCurrentMission();
                if(mission != null && mission.IsActive && !missions.Contains(mission))
                    AddMission(mission);
            }
        }

        private void AddMission(Mission mission)
        {
            missions.Add(mission);
            Console.WriteLine($"\nNew mission available: {mission.Title}");
        }

        private bool HasMission(string id) => missions.Exists(m => m.Id == id);

        public void ShowMissions()
        {
            missions.RemoveAll(m => m.IsExpired && !m.IsCompleted);

            if(missions.Count==0){ Console.WriteLine("\nNo active missions."); return;}

            Console.WriteLine("\nCurrent Missions:");
            for(int i=0;i<missions.Count;i++)
            {
                var m = missions[i];
                Console.WriteLine($"[{i+1}] {m.Title} {(m.IsCompleted?"(Completed)":"")}");
            }

            Console.WriteLine("Select a mission by number, or press Enter to skip.");
            string input = Console.ReadLine()?.Trim();
            if(int.TryParse(input,out int sel) && sel>=1 && sel<=missions.Count)
                AttemptMission(sel-1);
        }

        private void AttemptMission(int index)
        {
            Mission m = missions[index];
            if(m.IsCompleted){ Console.WriteLine("Already completed."); return;}
            Console.WriteLine($"\nAttempting: {m.Title}");
            Console.WriteLine(m.Description);

            string outcome="success";
            if(m.Id=="kings_intro")
            {
                Console.WriteLine("[1] Hand it over honestly [2] Skim cash");
                outcome = Console.ReadLine()?.Trim()=="2"?"betray":"honor";
            }

            m.Complete(player, gangManager, outcome);

            foreach(var arc in storyArcs)
                if(arc.Missions.Contains(m)) arc.AdvanceMission();
        }

        private void InitializeStreetEvents()
        {
            streetEvents = new List<StreetEvent>
            {
                new StreetEvent(
                    "A rival gang ambushes you. Fight or flee?",
                    (p,g)=>{
                        Console.WriteLine("[1] Fight [2] Flee");
                        string choice = Console.ReadLine()?.Trim();
                        if(choice=="1"){ p.AddReputation(10); g.AdjustRelation("Iron Vultures",-10);}
                        else{ p.AddReputation(-5);}
                    })
            };
        }

        public void TriggerRandomEvent()
        {
            if(rng.NextDouble()<0.3)
            {
                int index = rng.Next(streetEvents.Count);
                streetEvents[index].Trigger(player,gangManager,this);
            }
        }

        public Mission CreateKingsIntro()
        {
            var m = new Mission(
                "kings_intro",
                "Kings’ First Test",
                "Collect payment from a local who’s behind on protection money.",
                new Dictionary<string,string>{{"honor","kings_followup"},{"betray","vultures_offer"}},
                (p,g,outcome)=>{
                    if(outcome=="honor"){p.AddMoney(50); g.AdjustRelation("South Side Kings",15);}
                    else{p.AddMoney(30); g.AdjustRelation("South Side Kings",-20); g.AdjustRelation("Iron Vultures",10);}
                    p.AddReputation(10);
                });
            m.SetSchedule(GameTime.CurrentDay,3);
            return m;
        }
    }

    public class StreetEvent
    {
        public string Description { get; }
        public Action<Player,GangManager> ChoiceAction { get; }

        public StreetEvent(string desc, Action<Player,GangManager> action)
        {
            Description = desc;
            ChoiceAction = action;
        }

        public void Trigger(Player p, GangManager g, MissionManager mm)
        {
            Console.WriteLine("\n--- Random Event ---");
            Console.WriteLine(Description);
            ChoiceAction?.Invoke(p,g);
        }
    }
}
