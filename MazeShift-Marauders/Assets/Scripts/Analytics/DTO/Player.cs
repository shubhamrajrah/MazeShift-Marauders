using Newtonsoft.Json;

namespace Analytics.DTO
{
    [System.Serializable]
    public class Player
    {
        public string userId;
        public int age;

        public Player(string userId, int age)
        {
            this.userId = userId;
            this.age = age;
        }
    }
}