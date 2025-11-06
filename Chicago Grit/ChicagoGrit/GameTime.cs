using System;

namespace ChicagoGrit
{
    public class GameTime
    {
        public int Day { get; private set; } = 1;
        public int Hour { get; private set; } = 8;

        public static int CurrentDay { get; private set; } = 1;

        public void AdvanceHours(int hours)
        {
            Hour += hours;
            while (Hour >= 24)
            {
                Hour -= 24;
                Day++;
                CurrentDay = Day;
                Console.WriteLine($"\n--- DAY {Day} BEGINS ---");
            }
        }

        public string CurrentTime()
        {
            string period = Hour < 12 ? "AM" : "PM";
            int displayHour = Hour % 12;
            if (displayHour == 0) displayHour = 12;
            return $"{displayHour} {period}";
        }
    }
}
