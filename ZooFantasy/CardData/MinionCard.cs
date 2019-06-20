using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using ZooFantasy.EffectData;

namespace ZooFantasy.CardData
{
    public class MinionCard : BaseCard
    {
        private int attack;
        public int Attack { get { return attack; } }
        private int health;
        public int Health { get { return health; } }
        private string imagePath;
        public string ImagePath { get { return imagePath; } }
        private string category;
        public string Category { get { return category; } }


        private List<Effect> effects;
        public List<Effect> Effects { get => effects; }

        public MinionCard()
        {
            effects = new List<Effect>();
        }
        public MinionCard(int CardCost, int CardAttack, int CardHealth, string CardName, string MinionCardCategory, string CardDescription) : base(CardCost, CardName, CardDescription)
        {
            effects = new List<Effect>();
            attack = CardAttack;
            health = CardHealth;
            category = MinionCardCategory;
            imagePath = string.Format("ms-appx:///Resource/CardData/MinionCardData/{0}/{1}.png", Category, Name);

        }
        public void AddEffect(Effect effect)
        {
            effects.Add(effect);
        }
    }
}
