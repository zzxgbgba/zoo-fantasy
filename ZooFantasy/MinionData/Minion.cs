using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooFantasy.CardData;
using ZooFantasy.EffectData;

namespace ZooFantasy.MinionData
{
    public class Minion
    {
        private MinionCard originMinionCard;
        public MinionCard OriginMinionCard { get { return originMinionCard; } }
        private int currentAttack;
        public int CurrentAttack { get { return currentAttack; } }
        private int currentHealth;
        public int CurrentHealth { get { return currentHealth; } }
        private bool canAttack;
        public bool CanAttack { get { return canAttack; } }
        private List<Effect> effects;
        public List<Effect> Effects { get => effects; }

        public Minion(MinionCard card)
        {
            effects = card.Effects;
            originMinionCard = card;
            currentAttack = card.Attack;
            currentHealth = card.Health;
            canAttack = false;
            
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            if (currentHealth < 0) currentHealth = 0;
        }

        public void ChangeCanAttack(bool can)
        {
            canAttack = can;
        }

    }
}
