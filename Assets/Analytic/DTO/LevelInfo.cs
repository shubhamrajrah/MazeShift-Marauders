using System;

namespace Analytic.DTO
{
    public class LevelInfo
    {
        public int LevelNum;
        public String Start;
        public String End;
        public long Interval;
        public int DeadTimes;
        public int CoinCollected;
        public int DestroyedWalls;

        public LevelInfo(int levelNum, DateTime start)
        {
            LevelNum = levelNum;
            Start = start.ToString("O");
            Interval = 0;
            DeadTimes = 0;
            CoinCollected = 0;
            DestroyedWalls = 0;
        }

        public void CalculateIntervalAndSend(DateTime end)
        {
            End = end.ToString("O");
            DateTime start = DateTime.Parse(Start);
            TimeSpan span = end - start;
            Interval = span.Seconds;
            // send data to firebase
            HttpSender.RecordData(this.GetType().Name, this);
        }
    }
    
}