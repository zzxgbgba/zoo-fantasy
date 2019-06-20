using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooFantasy.BattlefieldData
{
    public class PlayerHeroData
    {
        private int health;
        public int Health { get => health; }

        public PlayerHeroData(int PlayerHeroHealth)
        {
            health = PlayerHeroHealth;
        }
        public void TakeDamage(int num)
        {
            health -= num;
        }
    }
}
