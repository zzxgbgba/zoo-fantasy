using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooFantasy.EffectData;

namespace ZooFantasy.CardData
{
    public class SpellCard : BaseCard
    {
        private string imagePath;
        public string ImagePath { get { return imagePath; } }

        private Effect spellEffect;
        public Effect SpellEffect { get => spellEffect; }

        public SpellCard(int CardCost, string CardName,string CardDescription) : base(CardCost, CardName ,CardDescription)
        {
            spellEffect = new Effect();
            imagePath = string.Format("ms-appx:///Resource/CardData/SpellCardData/{0}.png", CardName);
        }

        public void SetEffect(Effect effect)
        {
            spellEffect = effect;
        }
    }
}
