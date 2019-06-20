using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using ZooFantasy.BattlefieldData;
using ZooFantasy.MinionData;

namespace ZooFantasy.CardData
{
    class HandCardManager
    {
        public static void DrawCard(PlayerData player)
        {
            int totCount = player.CurrentDeck.MinionCard.Count + player.CurrentDeck.SpellCard.Count;
            if (totCount <= 0) return;
            Random r = new Random();
            int drawedCardNumber = r.Next(0, totCount);
            if (drawedCardNumber >= player.CurrentDeck.MinionCard.Count) 
            {
                drawedCardNumber -= player.CurrentDeck.MinionCard.Count;
                player.HandCard.Add(new Card(player.CurrentDeck.SpellCard[drawedCardNumber]));
                player.CurrentDeck.SpellCard.Remove(player.CurrentDeck.SpellCard[drawedCardNumber]);
            }
            else
            {
                player.HandCard.Add(new Card(player.CurrentDeck.MinionCard[drawedCardNumber]));
                player.CurrentDeck.MinionCard.Remove(player.CurrentDeck.MinionCard[drawedCardNumber]);
            }


        }
        public static void DrawCard(int num,PlayerData player)
        {
            for (int i = 0; i < num; i++)
            {
                DrawCard(player);
            }
        }

        public static void ShuffleIntoDeck(MinionCard card,PlayerData player)
        {
            player.CurrentDeck.MinionCard.Add(card);
        }
        public static void ShuffleIntoDeck(SpellCard card, PlayerData player)
        {
            player.CurrentDeck.SpellCard.Add(card);
        }
        public static void ShuffleIntoDeck(Minion minion, PlayerData player)
        {
            player.CurrentDeck.MinionCard.Add(minion.OriginMinionCard);
        }


        public static void UpdateHandCardGirdView(GridView gridView,PlayerData player)
        {
            gridView.Items.Clear();
            for (int i = 0; i < player.HandCard.Count; i++)
            {
                if (player.HandCard[i].CardType == CardTypes.MinionCard)
                {
                    CardDataManager.AddCardToGridView_Little(gridView, player.HandCard[i].CardData as MinionCard, player.HandCard[i]);
                }
                else
                {
                    CardDataManager.AddCardToGridView_Little(gridView, player.HandCard[i].CardData as SpellCard, player.HandCard[i]);
                }
            }
        }
    }
}
