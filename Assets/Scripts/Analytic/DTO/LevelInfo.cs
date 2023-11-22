using System;

namespace Analytic.DTO
{
    public class LevelInfo
    {
        public int LevelNum;
        public String Start;
        public String End;
        public long Interval;
        public bool IsSuccess;
        public int DeadTimesUp;
        
        public int SpeedCollected;
        public int SpeedUsed;
        public int FreezeCollected;
        public int FreezeUsed;
        public int FreezeTime;
        public int GhostUsed;
        public int GhostCollected;
        public float FutureSightUsedTime;
        public int KeyCollected;
        public int DestructionCollected;
        public int DestructionUsed;
        public int TrapStepped;
        public int Transported;

        public LevelInfo(int levelNum, DateTime start)
        {
            Random random = new Random();
            LevelNum = levelNum;
            Start = start.ToString("O");
            Interval = 0;
            DeadTimesUp = 0;
            SpeedCollected = 0;
            SpeedUsed = 0;
            FreezeTime = 0;
            GhostUsed = 0;
            GhostCollected = 0;
            FutureSightUsedTime = 0;
            
            // mock up data
            // DeadByChaser = random.Next(0, 10);
            // DestroyedWalls = random.Next(0, 3);
        }

        public void CalculateInterval(DateTime end)
        {
            End = end.ToString("O");
            DateTime start = DateTime.Parse(Start);
            TimeSpan span = end - start;
            Interval = (long)span.TotalSeconds;
        }

        public void SendData()
        {
            // send data to firebase
            HttpSender.RecordData(this.GetType().Name + "-GOLD", this);
        }
    }
    
}