using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooFantasy.BattlefieldData
{
    public class EnemyHeroData
    {
        private int health;
        public int Health { get => health; }

        public EnemyHeroData(int EnemyHeroHealth)
        {
            health = EnemyHeroHealth;
        }

        public void TakeDamage(int num)
        {
            health -= num;
        }
    }
}
