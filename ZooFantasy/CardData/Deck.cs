using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooFantasy.CardData;

namespace ZooFantasy.CardData
{
    public class Deck
    {
        private string name;
        public string Name { get { return name; } }
        private List<MinionCard> minionCard;
        public List<MinionCard> MinionCard { get { return minionCard; } }
        private List<SpellCard> spellCard;
        public List<SpellCard> SpellCard { get { return spellCard; } }

        public Deck()
        {
            minionCard = new List<MinionCard>();
            spellCard = new List<SpellCard>();
        }
        public Deck(string Name)
        {
            name = Name;
            minionCard = new List<MinionCard>();
            spellCard = new List<SpellCard>();
        }

        public void AddMinionCardByList(List<MinionCard> minionCardList)
        {
            int i = 0;
            while (i < minionCardList.Count)
            {
                minionCard.Add(minionCardList[i]);
                i++;
            }
        }

        public void ChangeToCacheDeck()
        {
            name = "CacheDeck";
        }
        public void ChangeName(string newName)
        {
            name = newName;
        }
    }
}
