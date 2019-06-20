using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using ZooFantasy.BattlefieldData;

namespace ZooFantasy.EffectData
{
    class EffectRequirementCheck
    {
        public static bool CheckRequirment(Grid PlayerHeroGrid, Grid EnemyHeroGrid, GridView PlayerMinionGridView, GridView EnemyMinionGridView, Battlefield bf, string requirement, string value)
        {
            switch (requirement)
            {
                case "HeroHealthMoreThan":
                    {
                        return EffectRequirement.HeroHealthMoreThan(PlayerHeroGrid, value);
                    }
            }
            return true;
        }
        public static bool CheckRequirments(Grid PlayerHeroGrid, Grid EnemyHeroGrid, GridView PlayerMinionGridView, GridView EnemyMinionGridView, Battlefield bf, List<string> requirements, List<string> values)
        {
            
            for (int i = 0; i < requirements.Count; i++)
            {
                if (CheckRequirment(PlayerHeroGrid, EnemyHeroGrid, PlayerMinionGridView, EnemyMinionGridView, bf, requirements[i], values[i]) == false) 
                {
                    return false;
                }
            }
            return true;
        }
        public static bool CheckRequirments(Grid PlayerHeroGrid, Grid EnemyHeroGrid, GridView PlayerMinionGridView, GridView EnemyMinionGridView, Battlefield bf, Effect effect)
        {
            return CheckRequirments(PlayerHeroGrid, EnemyHeroGrid, PlayerMinionGridView, EnemyMinionGridView, bf, effect.Requirements, effect.RequirementValues);   
        }
    }
}
