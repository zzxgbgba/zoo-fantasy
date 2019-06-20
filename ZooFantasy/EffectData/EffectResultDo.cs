using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using ZooFantasy.BattlefieldData;

namespace ZooFantasy.EffectData
{
    class EffectResultDo
    {
        public static void DoResult(Grid PlayerHeroGrid, Grid EnemyHeroGrid, GridView PlayerMinionGridView, GridView EnemyMinionGridView, Battlefield bf, object aim, string result, string value)
        {
            switch (result)
            {
                case "DealDamage":
                    {
                        EffectResult.DealDamage(aim, value);
                        break;
                    }
                case "DrawCard":
                    {
                        EffectResult.DrawCard(bf.Player, value);
                        break;
                    }
                case "DealDamageToAllEnemyMinions":
                    {
                        EffectResult.DealDamageToAllEnemyMinions(EnemyMinionGridView, value);
                        break;
                    }
                case "DestoryAllMinions":
                    {
                        EffectResult.DestoryAllMinions(PlayerMinionGridView, EnemyMinionGridView, value);
                        break;
                    }
                case "DiscardHand":
                    {
                        EffectResult.DiscardHand(bf.Player, value);
                        break;
                    }

            }
        }
        public static void DoResults(Grid PlayerHeroGrid, Grid EnemyHeroGrid, GridView PlayerMinionGridView, GridView EnemyMinionGridView, Battlefield bf, object aim, List<string> results, List<string> values)
        {
            for (int i = 0; i < results.Count; i++)
            {
                DoResult(PlayerHeroGrid, EnemyHeroGrid, PlayerMinionGridView, EnemyMinionGridView, bf, aim, results[i], values[i]);
            }
        }
        public static void DoResults(Grid PlayerHeroGrid, Grid EnemyHeroGrid, GridView PlayerMinionGridView, GridView EnemyMinionGridView, Battlefield bf, object aim, Effect effect)
        {
            DoResults(PlayerHeroGrid, EnemyHeroGrid, PlayerMinionGridView, EnemyMinionGridView, bf, aim, effect.Results, effect.ResultValues);
        }
    }
}
