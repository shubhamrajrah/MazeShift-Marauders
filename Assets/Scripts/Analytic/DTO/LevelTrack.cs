namespace Analytic.DTO
{
    public class LevelTrack
    {
        public int StartLevel;
        public int EndLevel;
        public bool IsReachTheEnd;

        public LevelTrack(int startLevel)
        {
            StartLevel = startLevel;
            IsReachTheEnd = false;
        }
        
        public void SendTrack()
        {
            // send data to firebase
            HttpSender.RecordData(this.GetType().Name, this);
        }
    }
    
}