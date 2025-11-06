using System;
using System.Collections.Generic;

namespace ChicagoGrit
{
    class Mission
    {
        public string Id { get; }
        public string Title { get; }
        public string Description { get; }
        public bool IsCompleted { get; private set; }
        public Dictionary<string, string> NextBranches { get; }

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

        public void Complete(Player player, GangManager gangManager, string outcomeKey)
        {
            if (!IsCompleted)
            {
                IsCompleted = true;
                Console.WriteLine($"\nMission Complete: {Title}");
                onComplete?.Invoke(player, gangManager, outcomeKey);
            }
        }

        public string GetNextMissionId(string outcomeKey)
        {
            return NextBranches.ContainsKey(outcomeKey) ? NextBranches[outcomeKey] : null;
        }
    }

    class MissionManager
    {
        private Player player;
        private GangManager gangManager;
        private List<Mission> missions = new();

        public MissionManager(Player player, GangManager gangManager)
        {
            this.player = player;
            this.gangManager = gangManager;
        }

        public void CheckUnlocks()
        {
            if (!HasMission("kings_intro")) AddMission(CreateKingsIntro());
            if (!HasMission("reapers_intro")) AddMission(CreateReapersIntro());
            if (!HasMission("ghosts_intro")) AddMission(CreateGhostsIntro());
        }

        #region Mission Creators

        // --- South Side Kings ---
        private Mission CreateKingsIntro()
        {
            return new Mission(
                "kings_intro",
                "Kings’ First Test",
                "Collect payment from a local who’s behind on protection money.",
                new Dictionary<string, string>
                {
                    { "honor", "kings_followup" },
                    { "betray", "vultures_offer" }
                },
                (p, g, outcome) =>
                {
                    if (outcome == "honor")
                    {
                        Console.WriteLine("You deliver the money honestly. The Kings respect loyalty.");
                        p.AddMoney(50);
                        g.AdjustRelation("South Side Kings", 15);
                    }
                    else
                    {
                        Console.WriteLine("You skim from the take. Word spreads fast.");
                        p.AddMoney(30);
                        g.AdjustRelation("South Side Kings", -20);
                        g.AdjustRelation("Iron Vultures", 10);
                    }
                    p.AddReputation(10);
                }
            );
        }

        private Mission CreateKingsFollowup()
        {
            return new Mission(
                "kings_followup",
                "The Kings’ Delivery",
                "Deliver a locked package across town — no questions asked.",
                new Dictionary<string, string>
                {
                    { "success", "kings_heist" }
                },
                (p, g, _) =>
                {
                    Console.WriteLine("You deliver the package successfully.");
                    p.AddMoney(100);
                    g.AdjustRelation("South Side Kings", 20);
                }
            );
        }

        private Mission CreateKingsHeist()
        {
            return new Mission(
                "kings_heist",
                "The Kings’ Heist",
                "Help the Kings rob a rival crew’s stash. Will you go all in or play safe?",
                new Dictionary<string, string>
                {
                    { "all_in", null },
                    { "play_safe", null }
                },
                (p, g, outcome) =>
                {
                    if (outcome == "all_in")
                    {
                        Console.WriteLine("The heist was risky but totally successful!");
                        p.AddMoney(200);
                        p.AddReputation(30);
                        g.AdjustRelation("South Side Kings", 30);
                    }
                    else
                    {
                        Console.WriteLine("You played it safe. Less profit, less attention.");
                        p.AddMoney(100);
                        p.AddReputation(15);
                        g.AdjustRelation("South Side Kings", 15);
                    }
                }
            );
        }

        // --- Iron Vultures ---
        private Mission CreateVulturesOffer()
        {
            return new Mission(
                "vultures_offer",
                "The Vultures’ Proposition",
                "The Iron Vultures offer you protection for a price.",
                new Dictionary<string, string>
                {
                    { "success", "vultures_job" }
                },
                (p, g, _) =>
                {
                    Console.WriteLine("You join the Iron Vultures. Things just got complicated.");
                    g.AdjustRelation("Iron Vultures", 25);
                    p.AddMoney(75);
                    p.AddReputation(15);
                }
            );
        }

        private Mission CreateVulturesJob()
        {
            return new Mission(
                "vultures_job",
                "Vultures’ Street Job",
                "The Iron Vultures want you to sabotage a rival. Will you go hard or avoid direct confrontation?",
                new Dictionary<string, string>
                {
                    { "aggressive", null },
                    { "cautious", null }
                },
                (p, g, outcome) =>
                {
                    if (outcome == "aggressive")
                    {
                        Console.WriteLine("You made a big impact for the Vultures!");
                        p.AddMoney(150);
                        p.AddReputation(25);
                        g.AdjustRelation("Iron Vultures", 25);
                    }
                    else
                    {
                        Console.WriteLine("You handled it carefully, staying under the radar.");
                        p.AddMoney(80);
                        p.AddReputation(10);
                        g.AdjustRelation("Iron Vultures", 10);
                    }
                }
            );
        }

        // --- West Side Reapers ---
        private Mission CreateReapersIntro()
        {
            return new Mission(
                "reapers_intro",
                "West Side Reapers’ Test",
                "The Reapers want to see if you can handle a street job. Will you join them?",
                new Dictionary<string, string>
                {
                    { "join", "reapers_heist" },
                    { "decline", null }
                },
                (p, g, outcome) =>
                {
                    if (outcome == "join")
                    {
                        Console.WriteLine("You join the West Side Reapers. Things just got dangerous!");
                        g.AdjustRelation("West Side Reapers", 25);
                        p.AddReputation(20);
                        p.AddMoney(50);
                    }
                    else
                    {
                        Console.WriteLine("You stay neutral. The Reapers respect caution, for now.");
                        g.AdjustRelation("West Side Reapers", 5);
                    }
                }
            );
        }

        private Mission CreateReapersHeist()
        {
            return new Mission(
                "reapers_heist",
                "Reapers’ Heist",
                "The Reapers want you to rob a rival gang. Go big or stay safe?",
                new Dictionary<string, string>
                {
                    { "all_in", null },
                    { "play_safe", null }
                },
                (p, g, outcome) =>
                {
                    if (outcome == "all_in")
                    {
                        Console.WriteLine("The Reapers’ heist was a total success!");
                        p.AddMoney(180);
                        p.AddReputation(25);
                        g.AdjustRelation("West Side Reapers", 30);
                    }
                    else
                    {
                        Console.WriteLine("You played it safe. Less loot, less respect.");
                        p.AddMoney(90);
                        p.AddReputation(10);
                        g.AdjustRelation("West Side Reapers", 15);
                    }
                }
            );
        }

        // --- North End Ghosts ---
        private Mission CreateGhostsIntro()
        {
            return new Mission(
                "ghosts_intro",
                "North End Ghosts’ Challenge",
                "The Ghosts are testing your street smarts. Impress them or offend them?",
                new Dictionary<string, string>
                {
                    { "impress", "ghosts_protection" },
                    { "offend", "ghosts_rivalry" }
                },
                (p, g, outcome) =>
                {
                    if (outcome == "impress")
                    {
                        Console.WriteLine("The Ghosts are impressed. You have their protection.");
                        g.AdjustRelation("North End Ghosts", 25);
                        p.AddReputation(20);
                    }
                    else
                    {
                        Console.WriteLine("You offended the Ghosts. Watch your back!");
                        g.AdjustRelation("North End Ghosts", -15);
                        p.AddReputation(-5);
                    }
                }
            );
        }

        private Mission CreateGhostsProtection()
        {
            return new Mission(
                "ghosts_protection",
                "Ghosts’ Protection",
                "You now have the Ghosts’ protection. They want a small favor. Do you accept?",
                new Dictionary<string, string>
                {
                    { "accept", null },
                    { "refuse", null }
                },
                (p, g, outcome) =>
                {
                    if (outcome == "accept")
                    {
                        Console.WriteLine("You helped the Ghosts. Respect gained!");
                        g.AdjustRelation("North End Ghosts", 20);
                        p.AddReputation(15);
                        p.AddMoney(50);
                    }
                    else
                    {
                        Console.WriteLine("You refused. Their respect decreases slightly.");
                        g.AdjustRelation("North End Ghosts", -10);
                    }
                }
            );
        }

        private Mission CreateGhostsRivalry()
        {
            return new Mission(
                "ghosts_rivalry",
                "Ghosts’ Rivalry",
                "You have made an enemy of the Ghosts. Expect trouble on the streets.",
                null,
                (p, g, _) =>
                {
                    Console.WriteLine("The Ghosts are watching you. Stay alert!");
                    g.AdjustRelation("North End Ghosts", -25);
                }
            );
        }

        #endregion

        #region Core Mission Management

        private void AddMission(Mission mission)
        {
            missions.Add(mission);
            Console.WriteLine($"\nNew mission available: {mission.Title}");
        }

        public void ShowMissions()
        {
            if (missions.Count == 0)
            {
                Console.WriteLine("\nNo active missions yet.");
                return;
            }

            Console.WriteLine("\nCurrent Missions:");
            for (int i = 0; i < missions.Count; i++)
            {
                var m = missions[i];
                Console.WriteLine($"[{i + 1}] {m.Title} {(m.IsCompleted ? "(Completed)" : "")}");
            }

            Console.WriteLine("\nSelect a mission by number, or press Enter to skip.");
            Console.Write("> ");
            string input = Console.ReadLine()?.Trim();

            if (int.TryParse(input, out int selected) && selected >= 1 && selected <= missions.Count)
            {
                AttemptMission(selected - 1);
            }
            else if (!string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Invalid selection.");
            }
        }

        private void AttemptMission(int index)
        {
            Mission mission = missions[index];
            if (mission.IsCompleted)
            {
                Console.WriteLine("You already completed that mission.");
                return;
            }

            Console.WriteLine($"\nAttempting: {mission.Title}");
            Console.WriteLine(mission.Description);

            string outcome = "success";

            switch (mission.Id)
            {
                case "kings_intro":
                    Console.WriteLine("[1] Hand it over honestly");
                    Console.WriteLine("[2] Skim some cash");
                    outcome = Console.ReadLine()?.Trim() == "2" ? "betray" : "honor";
                    break;

                case "kings_heist":
                    Console.WriteLine("[1] Go all in");
                    Console.WriteLine("[2] Play safe");
                    outcome = Console.ReadLine()?.Trim() == "1" ? "all_in" : "play_safe";
                    break;

                case "vultures_job":
                    Console.WriteLine("[1] Be aggressive");
                    Console.WriteLine("[2] Be cautious");
                    outcome = Console.ReadLine()?.Trim() == "1" ? "aggressive" : "cautious";
                    break;

                case "reapers_intro":
                    Console.WriteLine("[1] Join the Reapers");
                    Console.WriteLine("[2] Decline");
                    outcome = Console.ReadLine()?.Trim() == "1" ? "join" : "decline";
                    break;

                case "reapers_heist":
                    Console.WriteLine("[1] Go all in");
                    Console.WriteLine("[2] Play safe");
                    outcome = Console.ReadLine()?.Trim() == "1" ? "all_in" : "play_safe";
                    break;

                case "ghosts_intro":
                    Console.WriteLine("[1] Impress the Ghosts");
                    Console.WriteLine("[2] Offend the Ghosts");
                    outcome = Console.ReadLine()?.Trim() == "1" ? "impress" : "offend";
                    break;

                case "ghosts_protection":
                    Console.WriteLine("[1] Accept their favor");
                    Console.WriteLine("[2] Refuse");
                    outcome = Console.ReadLine()?.Trim() == "1" ? "accept" : "refuse";
                    break;

                default:
                    outcome = "success";
                    break;
            }

            mission.Complete(player, gangManager, outcome);
            UnlockNextMission(mission.GetNextMissionId(outcome));
        }

        private void UnlockNextMission(string id)
        {
            if (string.IsNullOrEmpty(id) || HasMission(id)) return;

            switch (id)
            {
                case "kings_followup": AddMission(CreateKingsFollowup()); break;
                case "vultures_offer": AddMission(CreateVulturesOffer()); break;
                case "kings_heist": AddMission(CreateKingsHeist()); break;
                case "vultures_job": AddMission(CreateVulturesJob()); break;
                case "reapers_intro": AddMission(CreateReapersIntro()); break;
                case "reapers_heist": AddMission(CreateReapersHeist()); break;
                case "ghosts_intro": AddMission(CreateGhostsIntro()); break;
                case "ghosts_protection": AddMission(CreateGhostsProtection()); break;
                case "ghosts_rivalry": AddMission(CreateGhostsRivalry()); break;
            }
        }

        private bool HasMission(string id) => missions.Exists(m => m.Id == id);

        #endregion
    }
}
