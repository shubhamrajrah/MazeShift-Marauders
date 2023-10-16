using System;

namespace Analytic.DTO
{
    public class LevelInfo
    {
        public int LevelNum;
        public DateTime Start;
        public DateTime End;
        public long Interval;
        public int DeadTimes;
        public int DestroyedWalls;

        public LevelInfo(int levelNum, DateTime start)
        {
            LevelNum = levelNum;
            Start = start;
            Interval = 0;
            DeadTimes = 0;
            DestroyedWalls = 0;
        }
    }
    
}