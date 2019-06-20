using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooFantasy.CardData;
using ZooFantasy.MinionData;

namespace ZooFantasy.BattlefieldData
{
    /// <summary>
    ///用来存储玩家生命值，手牌，牌库
    /// </summary>
    public class PlayerData
    {
        private Deck originDeck;
        public Deck OriginDeck { get { return originDeck; } }
        private Deck currentDeck;
        public Deck CurrentDeck { get { return currentDeck; } }
        private List<Card> handCard;
        public List<Card> HandCard { get { return handCard; } }



        public PlayerData()
        {
            handCard = new List<Card>();
            currentDeck = new Deck();
            originDeck = new Deck();
        }

        public void PlayerDataInit(Deck PlayerDeck)
        {
            originDeck = PlayerDeck;
            currentDeck = PlayerDeck;
        }


    }
}
