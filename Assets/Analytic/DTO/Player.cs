using UnityEngine;

namespace Analytic.DTO
{
    public class Player
    {
        [SerializeField]
        private int age;
        [SerializeField]
        public string userId;

        public Player(int age, string userId)
        {
            this.age = age;
            this.userId = userId;
        }
    }
}