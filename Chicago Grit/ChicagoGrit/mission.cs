using System;
using System.Collections.Generic;

namespace ChicagoGrit
{
    public class Mission
    {
        public string Id { get; }
        public string Title { get; }
        public string Description { get; }
        public bool IsCompleted { get; private set; }
        public Dictionary<string, string> NextBranches { get; }
        public int StartDay { get; private set; } = 0;
        public int EndDay { get; private set; } = 0;
        public bool IsActive => GameTime.CurrentDay >= StartDay && GameTime.CurrentDay <= EndDay;
        public bool IsExpired => GameTime.CurrentDay > EndDay;

        private Action<Player, GangManager, string> onComplete;

        public Mission(string id, string title, string desc,
                       Dictionary<string, string> nextBranches,
                       Action<Player, GangManager, string> completeAction)
        {
            Id = id;
            Title = title;
            Description = desc;
            NextBranches = nextBranches ?? new Dictionary<string, string>();
            onComplete = completeAction;
        }

        public void SetSchedule(int startDay, int duration)
        {
            StartDay = startDay;
            EndDay = startDay + duration;
        }

        public void Complete(Player player, GangManager gangManager, string outcomeKey)
        {
            if (!IsCompleted && !IsExpired)
            {
                IsCompleted = true;
                Console.WriteLine($"\nMission Complete: {Title}");
                onComplete?.Invoke(player, gangManager, outcomeKey);
            }
            else if (IsExpired)
            {
                Console.WriteLine($"\nMission Failed (Expired): {Title}");
            }
        }

        public string GetNextMissionId(string outcomeKey)
        {
            return NextBranches.ContainsKey(outcomeKey) ? NextBranches[outcomeKey] : null;
        }
    }
}
