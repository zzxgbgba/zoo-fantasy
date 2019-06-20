using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using ZooFantasy.BattlefieldData;
using ZooFantasy.CardData;
using ZooFantasy.MinionData;

namespace ZooFantasy.EffectData
{

    public class EffectResult
    {
        /// <summary>
        /// 对任意目标造成伤害
        /// </summary>
        /// <param name="value">格式:"int" </param>
        public static void DealDamage(object aim, string value)
        {
            int damage = int.Parse(value);
            string type = aim.GetType().ToString();
            switch (type)
            {
                case "ZooFantasy.MinionData.Minion":
                    {
                        (aim as Minion).TakeDamage(damage);
                        break;
                    }
                case "ZooFantasy.BattlefieldData.EnemyHeroData":
                    {
                        (aim as EnemyHeroData).TakeDamage(damage);
                        break;
                    }
                case "ZooFantasy.BattlefieldData.PlayerHeroData":
                    {
                        (aim as PlayerHeroData).TakeDamage(damage);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        /// <summary>
        /// 抽牌
        /// </summary>
        /// <param name="value">格式:"int" </param>
        public static void DrawCard(PlayerData player, string value)
        {
            int num = int.Parse(value);
            HandCardManager.DrawCard(num, player);
        }

        /// <summary>
        /// 对全体敌方随从造成伤害
        /// </summary>
        /// <param name="value">格式:"int" </param>
        public static void DealDamageToAllEnemyMinions(GridView gridView, string value)
        {
            int num = int.Parse(value);
            foreach(var item in gridView.Items)
            {
                ((item as Grid).DataContext as Minion).TakeDamage(num);
            }
        }

        /// <summary>
        /// 消灭所有随从
        /// </summary>
        /// <param name="value">格式:"" </param>
        public static void DestoryAllMinions(GridView PlayerMinionGridView, GridView EnemyMinionGridView, string value)
        {
            PlayerMinionGridView.Items.Clear();
            EnemyMinionGridView.Items.Clear();
        }

        /// <summary>
        /// 丢弃所有手牌
        /// </summary>
        /// <param name="value">格式:"" </param>
        public static void DiscardHand(PlayerData player, string value)
        {
            player.HandCard.Clear();
        }



    }
}
