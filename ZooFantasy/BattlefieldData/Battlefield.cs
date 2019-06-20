using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooFantasy.MinionData;
using ZooFantasy.CardData;
using System.Collections.ObjectModel;

namespace ZooFantasy.BattlefieldData
{
    public class Battlefield
    {
        private PlayerData player;
        public PlayerData Player { get { return player; } }
        private int maxCost;
        public int MaxCost { get { return maxCost; } }
        private int currentCost;
        public int CurrentCost { get { return currentCost; } }

        public Battlefield()
        {
            player = new PlayerData();
        }

        public async Task BattlefieldInit()
        {
            currentCost = 0;
            maxCost = 0;
            Deck tmp = new Deck();
            tmp = await DeckManager.LoadCacheDeck();
            player.PlayerDataInit(tmp);
        }

        public void SummonPlayerMinion(Minion newMinion)
        {
            currentCost -= newMinion.OriginMinionCard.Cost;
        }
        public void CastPlayerSpell(SpellCard card)
        {
            currentCost -= card.Cost;
        }

        public void PlayerTurnInit()
        {
            HandCardManager.DrawCard(player);
            if(maxCost<10) CostPlus();
                currentCost = maxCost;
        }

        public void CostPlus()
        {
            maxCost++;
        }
        public void CostPlus(int num)
        {
            maxCost += num;
        }
    }
}
