using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooFantasy.CardData
{
    public class Card
    {
        private object cardData;
        public object CardData { get => cardData; }
        public Card(SpellCard spellCard)
        {
            cardData = spellCard;
            CardType = CardTypes.SpellCard;
        }
        public Card(MinionCard minionCard)
        {
            cardData = minionCard;
            CardType = CardTypes.MinionCard;
        }
        public CardTypes CardType;
    }
    public enum CardTypes
    { 
        MinionCard,
        SpellCard
    };
}
